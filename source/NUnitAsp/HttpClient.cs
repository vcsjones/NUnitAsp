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

	public class HttpClient
	{
		// Lifetime of object
		private TimeSpan serverTime = new TimeSpan(0);
		private Hashtable cookies = new Hashtable();

		// Lifetime of current page
		private Uri currentUrl = null;
		private XmlDocument page;
		private string pageText;
		private NameValueCollection formVariables = new NameValueCollection();
			
		public void GetPage(string url) 
		{
			formVariables.Clear();
			DoHttp(url, "get");
		}

		internal void SubmitForm(string url, string method)
		{
			DoHttp(url, method);
		}

		public void SetFormVariable(string name, string value) 
		{
			formVariables.Remove(name);
			formVariables.Add(name, value);
		}

		public bool HasCookie(string cookieName) 
		{
			return cookies.ContainsKey(cookieName);
		}

		internal XmlDocument CurrentPage
		{
			get 
			{
				if (page == null) throw new NoPageException();
				return page;
			}
		}

		public TimeSpan ElapsedServerTime 
		{
			get 
			{
				return serverTime;
			}
		}

		private void DoHttp(string url, string method)
		{
			UpdateCurrentUrl(url);
			WebRequest request = CreateWebRequest(method);
			string[] newCookies = ReadHttpResponse(request);
			ParseHttpResponse(newCookies);
		}

		private void UpdateCurrentUrl(string url)
		{
			if (currentUrl == null) currentUrl = new Uri(url);
			else currentUrl = new Uri(currentUrl, url);
		}

		private WebRequest CreateWebRequest(string method)
		{
			StreamWriter writer = null;
			try 
			{
				string parameters = CreateParameterString();
				string url = currentUrl.ToString();
				if (method.ToLower() == "get" && formVariables.Count > 0) url += "?" + parameters;

				WebRequest request = WebRequest.Create(url);
				request.Method = method;
				request.Headers["Cookie"] = CreateCookieString();

				if (method.ToLower() == "post")
				{
					request.ContentType = "application/x-www-form-urlencoded";
					writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
					writer.WriteLine(parameters);
					writer.Close();
				}
				return request;
			}
			finally
			{
				if (writer != null) writer.Close();
			}
		}

		private string[] ReadHttpResponse(WebRequest request)
		{
			WebResponse response = null;
			try
			{
				DateTime startTime = DateTime.Now;
				response = request.GetResponse();
				serverTime += (DateTime.Now - startTime);

				StreamReader pageReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
				pageText = pageReader.ReadToEnd();

				return response.Headers.GetValues("Set-Cookie");
			}
			finally
			{
				if (response != null) response.Close();
			}
		}

		private void ParseHttpResponse(string[] newCookies) 
		{
			try 
			{
				page = new XmlDocument();
				page.LoadXml(pageText);
				ParseCookies(newCookies);
				ParseDefaultFormVariables();
			}
			catch (XmlException e) 
			{
				throw new XmlException("Error parsing page...\n" +  pageText, e);
			}
		}

		private string CreateParameterString()
		{
			string joiner = "";
			string result = "";
			foreach (string key in formVariables.Keys)
			{
				result += joiner + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(formVariables.Get(key));
				joiner = "&";
			}
			return result;
		}

		private string CreateCookieString() 
		{
			string result = "";
			string cookieJoiner = "";
			foreach (DictionaryEntry cookie in cookies) 
			{
				result += cookieJoiner + cookie.Key + "=" + cookie.Value;
				cookieJoiner = "; ";
			}
			return result;
		}

		private string GetBaseAddress(string url) 
		{
			Uri uri = new Uri(url);
			return uri.AbsoluteUri;
		}

		private void ParseDefaultFormVariables() 
		{
			formVariables = new NameValueCollection();
			ParseFormHiddenValues();
			ParseFormTextValues("//input[@type='file']");
			ParseFormTextValues("//input[@type='password']");
			ParseFormTextValues("//input[@type='text']");
			ParseFormTextValues("//input[@type='radio'][@checked]");
			ParseFormTextAreaValues();
			ParseFormSelectValues();
		}

		private void ParseCookies(string[] newCookies) 
		{
			if (newCookies == null) return;

			foreach (string cookieString in newCookies) 
			{
				string[] cookieParameters = cookieString.Split(new char[] {';'});
				string[] nameValue = cookieParameters[0].Split(new char[] {'='});
				string name = nameValue[0];
				if (cookies.Contains(name)) cookies.Remove(name);
				cookies.Add(name, nameValue[1]);
			}
		}

		private void ParseFormHiddenValues() 
		{
			XmlNodeList nodes = page.SelectNodes("//input[@type='hidden']");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				XmlAttribute name = item.Attributes["name"];
				XmlAttribute aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) 
				{
					SetFormVariable(name.Value, aValue.Value);
				}
			}
		}

		private void ParseFormTextValues(string expression) 
		{
			XmlAttribute name;
			XmlAttribute aValue;
			XmlNodeList nodes = page.SelectNodes(expression);
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) 
				{
					SetFormVariable(name.Value, aValue.Value);
				}
			}
		}

		private void ParseFormTextAreaValues() 
		{
			XmlAttribute name;
			XmlNodeList nodes = page.SelectNodes("//textarea");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				if (name != null) 
				{
					SetFormVariable(name.Value, item.InnerText.Trim());
				}
			}
		}

		private void ParseFormSelectValues() 
		{
			string expression = "//select";
			XmlAttribute name;
			XmlNode aValue = null;
			XmlNodeList nodes = page.SelectNodes(expression);

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
					SetFormVariable(name.Value, String.Empty);
				}
				else 
				{
					SetFormVariable(name.Value, aValue.Value);
				}
			}
		}

		public class NoPageException : ApplicationException
		{
			internal NoPageException() : base("No pages have been loaded by the browser")
			{
			}
		}

	}
}