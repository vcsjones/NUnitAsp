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

namespace NUnit.Extensions.Asp 
{

	public class Browser : System.IDisposable 
	{

		private XhtmlWebForm _page;
		private XmlDocument _pageDocument;
		private string _pageDocumentText;
		private WebClient _client = new WebClient();
		private Hashtable _cookies = new Hashtable();
		private NameValueCollection _headers;
		private TimeSpan _serverTime = new TimeSpan(0);

		public Browser() 
		{
		}

		public Browser(string url) 
		{
			_client.BaseAddress = GetBaseAddress(url);
			GetPage(url);
		}

		public void Dispose() 
		{
			_client.Dispose();
		}

		public XhtmlWebForm CurrentPage 
		{
			
			get 
			{
				return _page;
			}
		}

		public void GetPage(string url) 
		{
			try 
			{
				AddCookiesToHeader(_client.Headers);
				DateTime startTime = DateTime.Now;
				byte[] response = _client.DownloadData(url);
				_serverTime += (DateTime.Now - startTime);
				LoadPage(response);
			}
			catch (WebException e) 
			{
				throw new WebException("Error getting page...\n" + DumpErrorPage(e.Response.GetResponseStream()), e);
			}
		}

		internal XhtmlWebForm SubmitForm() 
		{
			string url = _page.GetForm().Action;
			string method = _page.GetForm().Method;
			try 
			{
				DateTime startTime = DateTime.Now;
				byte[] response = _client.UploadValues(url, method, _headers);
				_serverTime += (DateTime.Now - startTime);
				LoadPage(response);
				return _page;
			}
			catch (WebException e) 
			{
				throw new WebException("Error submitting form...\n" + DumpErrorPage(e.Response.GetResponseStream()), e);
			}
		}

		private void LoadPage(byte[] output) 
		{
			_headers = new NameValueCollection(_client.Headers);
			UTF8Encoding decoder = new UTF8Encoding();
			_pageDocumentText = decoder.GetString(output);
			_pageDocument = new XmlDocument();
			_page = new XhtmlWebForm(this, output);
			try 
			{
				_pageDocument.LoadXml(_pageDocumentText);
			}
			catch (XmlException e) 
			{
				throw new XmlException("Error parsing page...\n" +  _pageDocumentText, e);
			}
			ParseCookies();
			SetDefaultFormValues();
		}

		private string GetBaseAddress(string url) 
		{
			Uri uri = new Uri(url);
			return uri.AbsoluteUri;
		}

		private string GetAttribute(string element, string id, string attribute) 
		{
			XmlElement xmlElement = _pageDocument.GetElementById(id);
			if (xmlElement == null) 
			{
				throw new XmlException(String.Format("Element '{0}' with id '{1}' is not on the page", element, id), null);
			}
			return xmlElement.GetAttribute(attribute);
		}

		private void SetDefaultFormValues() 
		{
			_headers.Clear();
			AddFormHiddenValuesToHeader();
			AddFormTextValuesToHeader("//input[@type='file']");
			AddFormTextValuesToHeader("//input[@type='password']");
			AddFormTextValuesToHeader("//input[@type='text']");
			AddFormTextAreaValuesToHeader();
			AddFormSelectValuesToHeader();
			AddCookiesToHeader(_headers);
		}

		private void AddCookiesToHeader(NameValueCollection header) 
		{
			bool firstCookie = true;
			string cookieHeader = "";
			foreach (DictionaryEntry cookie in _cookies) 
			{
				if (!firstCookie) cookieHeader += "; ";
				cookieHeader += cookie.Key + "=" + cookie.Value;
				firstCookie = false;
			}
			header["Cookie"] = cookieHeader;
		}

		private void ParseCookies() 
		{
			string[] cookieStrings = _client.ResponseHeaders.GetValues("Set-Cookie");
			if (cookieStrings == null) return;

			foreach (string cookieString in cookieStrings) 
			{
				string[] cookieParameters = cookieString.Split(new char[] {';'});
				string[] nameValue = cookieParameters[0].Split(new char[] {'='});
				string name = nameValue[0];
				if (_cookies.Contains(name)) _cookies.Remove(name);
				_cookies.Add(name, nameValue[1]);
			}
		}

		private void AddFormHiddenValuesToHeader() 
		{
			XmlNodeList nodes = _pageDocument.SelectNodes("//input[@type='hidden']");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				XmlAttribute name = item.Attributes["name"];
				XmlAttribute aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) 
				{
					EnterInputValue(name.Value, aValue.Value);
				}
			}
		}

		internal void EnterInputValue(string inputName, string inputValue) 
		{
			_headers.Remove(inputName);
			_headers.Add(inputName, inputValue);
		}

		private void AddFormTextValuesToHeader(string expression) 
		{
			XmlAttribute name;
			XmlAttribute aValue;
			XmlNodeList nodes = _pageDocument.SelectNodes(expression);
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) 
				{
					EnterInputValue(name.Value, aValue.Value);
				}
			}
		}

		private void AddFormTextAreaValuesToHeader() 
		{
			XmlAttribute name;
			XmlNodeList nodes = _pageDocument.SelectNodes("//textarea");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				if (name != null) 
				{
					EnterInputValue(name.Value, item.InnerText.Trim());
				}
			}
		}

		private void AddFormSelectValuesToHeader() 
		{
			string expression = "//select";
			XmlAttribute name;
			XmlNode aValue = null;
			XmlNodeList nodes = _pageDocument.SelectNodes(expression);

			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				if (name == null) 
				{
					throw new XmlException("A select form element on the page does not have a name.", null);
				}

				// Look for the option that is selected
				foreach (XmlNode child in item.ChildNodes) 
				{
					if (child.Attributes["selected"] != null) 
					{
						aValue = child.Attributes["value"];
					}
				}

				// If there is no value then we will just set it as empty
				if (aValue == null) 
				{
					EnterInputValue(name.Value, String.Empty);
				}
				else 
				{
					EnterInputValue(name.Value, aValue.Value);
				}
			}
		}

		private string DumpErrorPage(Stream ReceiveStream) 
		{
			Encoding encode;
			StreamReader sr;
			string errorPage = null;
			try 
			{
				encode = System.Text.Encoding.GetEncoding("utf-8");
				sr = new StreamReader(ReceiveStream, encode);

				char[] read = new char[256];
				int count = sr.Read(read, 0, 256);

				while (count > 0) 
				{
					string str = new string(read, 0, count);
					errorPage += str;
					count = sr.Read(read, 0, 256);
				}
			}
			catch (Exception) 
			{
				errorPage += "Error parsing returned stream";
			}

			return errorPage;
		}

		public bool HasCookie(string cookieName) 
		{
			return _cookies.ContainsKey(cookieName);
		}

		public TimeSpan ElapsedServerTime 
		{
			get 
			{
				return _serverTime;
			}
		}

		public string DumpCookies() 
		{
			string result = "";
			foreach (object key in _cookies.Keys) 
			{
				result += string.Format("[{0}]: [{1}]", key, _cookies["key"]);
			}
			return result;
		}

	}
}