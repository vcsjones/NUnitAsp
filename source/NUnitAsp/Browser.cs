/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Little
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Collections;

namespace NUnit.Extensions.Asp {

	public class Browser : System.IDisposable {

		private XmlDocument _page;
		private string _pageText;
		private WebClient _client;
		private Hashtable _cookies = new Hashtable();

		//Create a browser with anonymous credentials.
		public Browser(string url) {
			_client = new WebClient();
			_client.BaseAddress = GetBaseAddress(url);
			GetPage(url);
		}

		//Create a browser with the provided credentials.
		public Browser(string url, string user, string password, string domain) : this(url) {
			_client.Credentials = new NetworkCredential(user, password, domain);																		  
		}

		public void Dispose() {
			_client.Dispose();
		}

		public NameValueCollection QueryString {
			get {
				return _client.QueryString;
			}
		}

		public XmlDocument Page {
			get {
				return _page;
			}
		}

		public string PageText {
			get {
				return _pageText;
			}
		}

		public void SelectDropDownListItem(string dropDownId, string textValue) {
			XmlNode selectNode = _page.GetElementById(dropDownId);
			string inputValue = null;
			bool found = false;

			foreach (XmlNode selectOption in selectNode.ChildNodes) {
				if (selectOption.InnerText == textValue) {
					found = true;
					XmlAttribute valueAttribute = selectOption.Attributes["value"];
					if (valueAttribute == null) {
						// Use the option text for the value
						EnterInputValue(dropDownId, textValue);
					}
					else {
						inputValue = valueAttribute.Value;
					}
				}
			}

			if (!found) {
				throw new XmlException(string.Format("DropDownList '{0}' does not have ListItem '{1}'", dropDownId, textValue), null);
			}

			EnterInputValue(dropDownId, inputValue);
		}

		public void EnterInputValue(string inputName, string inputValue) {
			_client.Headers.Remove(inputName);
			_client.Headers.Add(inputName, inputValue);
		}

		public void ClickLink(string id) {
			XmlElement link = Page.GetElementById(id);
			GetPage(link.GetAttribute("href"));
		}

		public void ClickLinkButton(string id, string formId) {
			XmlElement linkButton = Page.GetElementById(id);
			string postBackCall = linkButton.GetAttribute("href");
			string postBackPattern = @"javascript:__doPostBack\('(?<target>.*?)',''\)";

			Match match = Regex.Match(postBackCall, postBackPattern, RegexOptions.IgnoreCase);
			if (!match.Success) throw new XmlException("Link '" + id + "' doesn't look like a link button", null);
			string target = match.Groups["target"].Captures[0].Value;

			EnterInputValue("__EVENTTARGET", target);
			EnterInputValue("__EVENTARGUMENT", "");
			SubmitForm(formId);
		}

		public void PushButton(string buttonId, string formId) {
			EnterInputValue(buttonId, GetButtonValue(buttonId));
			SubmitForm(formId);
		}

		public void PushButtonInDataGrid(string dataGridId, int rowNumber, int controlColumnIndex, string formId) {
			string buttonName = string.Format("{0}:ctrl{1}:ctrl{2}", dataGridId, rowNumber, controlColumnIndex);
			PushButtonByName(buttonName, formId);
		}

		public void PushButtonByName(string buttonName, string formId) {
			EnterInputValue(buttonName, GetButtonValue(buttonName, formId));
			SubmitForm(formId);
		}

		public void GetPage(string url) {
			try {
				LoadPage(_client.DownloadData(url));
			}
			catch (WebException e) {
				throw new WebException("Error getting page...\n" + DumpErrorPage(e.Response.GetResponseStream()), e);
			}
		}

		private void SubmitForm(string formId) {
			string url = GetFormAction(formId);
			string method = GetFormMethod(formId);
			try {
				LoadPage(_client.UploadValues(url, method, _client.Headers));
			}
			catch (WebException e) {
				throw new WebException("Error submitting form...\n" + DumpErrorPage(e.Response.GetResponseStream()), e);
			}
		}

		private void LoadPage(byte[] output) {
			UTF8Encoding decoder = new UTF8Encoding();
			_pageText = decoder.GetString(output);
			_page = new XmlDocument();
			try {
				_page.LoadXml(_pageText);
			}
			catch (XmlException e) {
				throw new XmlException("Error parsing page...\n" +  _pageText, e);
			}
			ParseCookies();
			SetDefaultFormValues();
		}

		private string GetBaseAddress(string url) {
			Uri uri = new Uri(url);
			return uri.AbsoluteUri;
		}

		private string GetButtonValue(string buttonId) {
			XmlElement button = _page.GetElementById(buttonId);
			if (button == null) {
				throw new XmlException(String.Format("Button '{0}' is not on the page", buttonId), null);
			}
			return button.GetAttribute("value");
		}

		private string GetButtonValue(string buttonName, string formId) {
			XmlElement form = _page.GetElementById(formId);
			string expression = String.Format("//input[@type='submit'][@name='{0}']", buttonName);
			XmlNode button = form.SelectSingleNode(expression);
			if (button == null) {
				throw new XmlException(String.Format("Button '{0}' for form '{1}' is not on the page..." + _pageText, buttonName, formId), null);
			}
			string returnValue = String.Empty;
			foreach (XmlAttribute attribute in button.Attributes) {
				if (attribute.Name.Equals("value")) {
					returnValue = attribute.Value;
				}
			}
			return returnValue;
		}

		private string GetFormMethod(string formId) {
			return GetAttribute("form", formId, "method");
		}

		private string GetFormAction(string formId) {
			return GetAttribute("form", formId, "action");
		}

		private string GetAttribute(string element, string id, string attribute) {
			XmlElement xmlElement = _page.GetElementById(id);
			if (xmlElement == null) {
				throw new XmlException(String.Format("Element '{0}' with id '{1}' is not on the page", element, id), null);
			}
			return xmlElement.GetAttribute(attribute);
		}

		private void SetDefaultFormValues() {
			_client.Headers.Clear();
			AddFormHiddenValuesToHeader();
			AddFormTextValuesToHeader("//input[@type='file']");
			AddFormTextValuesToHeader("//input[@type='password']");
			AddFormTextValuesToHeader("//input[@type='text']");
			AddFormTextAreaValuesToHeader();
			AddFormSelectValuesToHeader();
			AddCookiesToHeader();
		}

		private void AddCookiesToHeader() {
			bool firstCookie = true;
			string cookieHeader = "";
			foreach (DictionaryEntry cookie in _cookies) {
				if (!firstCookie) cookieHeader += "; ";
				cookieHeader += cookie.Key + "=" + cookie.Value;
				firstCookie = false;
			}
			EnterInputValue("Cookie", cookieHeader);
		}

		private void ParseCookies() {
			string setCookie = _client.ResponseHeaders["Set-Cookie"];
			if (setCookie == null) return;

			string[] cookies = setCookie.Split(new char[] {','});
			foreach (string cookie in cookies) {
				string[] cookieParameters = cookie.Split(new char[] {';'});
				string[] nameValue = cookieParameters[0].Split(new char[] {'='});
				_cookies.Add(nameValue[0], nameValue[1]);
			}
		}

		private void AddFormHiddenValuesToHeader() {
			XmlNodeList nodes = _page.SelectNodes("//input[@type='hidden']");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) {
				XmlAttribute name = item.Attributes["name"];
				XmlAttribute aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) {
					EnterInputValue(name.Value, aValue.Value);
				}
			}
		}

		private void AddFormTextValuesToHeader(string expression) {
			XmlAttribute name;
			XmlAttribute aValue;
			XmlNodeList nodes = _page.SelectNodes(expression);
			if (nodes == null) return;

			foreach (XmlNode item in nodes) {
				name = item.Attributes["name"];
				aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) {
					EnterInputValue(name.Value, aValue.Value);
				}
			}
		}

		private void AddFormTextAreaValuesToHeader() {
			XmlAttribute name;
			XmlNodeList nodes = _page.SelectNodes("//textarea");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) {
				name = item.Attributes["name"];
				if (name != null) {
					EnterInputValue(name.Value, item.InnerText.Trim());
				}
			}
		}

		private void AddFormSelectValuesToHeader() {
			string expression = "//select";
			XmlAttribute name;
			XmlNode aValue = null;
			XmlNodeList nodes = _page.SelectNodes(expression);

			if (nodes == null) return;

			foreach (XmlNode item in nodes) {
				name = item.Attributes["name"];
				if (name == null) {
					throw new XmlException("A select form element on the page does not have a name.", null);
				}

				// Look for the option that is selected
				foreach (XmlNode child in item.ChildNodes) {
					if (child.Attributes["selected"] != null) {
						aValue = child.Attributes["value"];
					}
				}

				// If there is no value then we will just set it as empty
				if (aValue == null) {
					EnterInputValue(name.Value, String.Empty);
				}
				else {
					EnterInputValue(name.Value, aValue.Value);
				}
			}
		}

		private string DumpErrorPage(Stream ReceiveStream) {
			Encoding encode;
			StreamReader sr;
			string errorPage = null;
			try {
				encode = System.Text.Encoding.GetEncoding("utf-8");
				sr = new StreamReader(ReceiveStream, encode);

				char[] read = new char[256];
				int count = sr.Read(read, 0, 256);

				while (count > 0) {
					string str = new string(read, 0, count);
					errorPage += str;
					count = sr.Read(read, 0, 256);
				}
			}
			catch (Exception) {
				errorPage += "Error parsing returned stream";
			}

			return errorPage;
		}

		public bool HasCookie(string cookieName) {
			return _cookies.ContainsKey(cookieName);
		}

	}
}