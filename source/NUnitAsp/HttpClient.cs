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
		private TimeSpan serverTime = new TimeSpan(0);
		public TimeSpan parseTime = new TimeSpan(0);
		private Hashtable cookies = new Hashtable();
		private Uri currentUrl = null;
		private WebPage currentPage = null;

		public void GetPage(string url) 
		{
			DoHttp(url, "get", "");
		}

		internal void SubmitForm(string url, string method)
		{
			DoHttp(url, method, currentPage.FormVariables);
		}

		public void SetFormVariable(string name, string value) 
		{
			currentPage.SetFormVariable(name, value);
		}

		public bool HasCookie(string cookieName) 
		{
			return cookies.ContainsKey(cookieName);
		}

		internal XmlDocument CurrentPage
		{
			get 
			{
				if (currentPage == null) throw new NoPageException();
				return currentPage.Document;
			}
		}

		public TimeSpan ElapsedServerTime 
		{
			get 
			{
				return serverTime;
			}
		}

		private void DoHttp(string url, string method, string formVariables)
		{
			UpdateCurrentUrl(url);
			WebRequest request = CreateWebRequest(method, formVariables);
			ReadHttpResponse(request);
		}

		private void UpdateCurrentUrl(string url)
		{
			if (currentUrl == null) currentUrl = new Uri(url);
			else currentUrl = new Uri(currentUrl, url);
		}

		private WebRequest CreateWebRequest(string method, string formVariables)
		{
			StreamWriter writer = null;
			try 
			{
				string url = currentUrl.ToString();
				if ((method.ToLower() == "get") && (formVariables != "")) url += "?" + formVariables;

				WebRequest request = WebRequest.Create(url);
				request.Method = method;
				request.Headers["Cookie"] = CreateCookieString();

				if (method.ToLower() == "post")
				{
					request.ContentType = "application/x-www-form-urlencoded";
					writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
					writer.WriteLine(formVariables);
					writer.Close();
				}
				return request;
			}
			finally
			{
				if (writer != null) writer.Close();
			}
		}

		private void ReadHttpResponse(WebRequest request)
		{
			WebResponse response = null;
			try
			{
				DateTime startTime = DateTime.Now;
				response = request.GetResponse();
				serverTime += (DateTime.Now - startTime);

				StreamReader pageReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
				currentPage = new WebPage(pageReader.ReadToEnd());
				ParseCookies(response.Headers.GetValues("Set-Cookie"));
			}
			finally
			{
				if (response != null) response.Close();
			}
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

		public class NoPageException : ApplicationException
		{
			internal NoPageException() : base("No pages have been loaded by the browser")
			{
			}
		}

	}
}