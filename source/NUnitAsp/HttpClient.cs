#region Copyright (c) 2002, 2003 Brian Knowles, Jim Little
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
#endregion

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Collections;
using NUnit.Framework;

namespace NUnit.Extensions.Asp 
{
	/// <summary>
	/// A web client, capable of communicating with a web server.
	/// </summary>
	public class HttpClient
	{
		private TimeSpan serverTime = new TimeSpan(0);
		private Hashtable cookies = new Hashtable();
		private Uri currentUrl = null;
		private WebPage currentPage = null;

		/// <summary>
		/// The user-agent string to send to the server.  Useful if you want to pretend to
		/// be a specific browser.
		/// </summary>
		public string UserAgent = "NUnitAsp";

		/// <summary>
		/// Retrieves a page from a web server.
		/// </summary>
		/// <param name="url">The URL of the page to get.</param>
		public void GetPage(string url) 
		{
			DoHttp(url, "get", "");
		}

		internal void SubmitForm(string url, string method)
		{
			DoHttp(url, method, currentPage.FormVariables);
		}

		internal void SetFormVariable(string name, string value) 
		{
			currentPage.SetFormVariable(name, value);
		}

		/// <summary>
		/// Checks to see if a cookie has been set.
		/// </summary>
		/// <param name="cookieName">The name of the cookie.</param>
		/// <returns>'true' if the cookie has been set.</returns>
		public bool HasCookie(string cookieName) 
		{
			return cookies.ContainsKey(cookieName);
		}

		/// <summary>
		/// Returns the value of a cookie.  Throws exception if the cookie hasn't been set.
		/// </summary>
		public string CookieValue(string cookieName)
		{
			if (!HasCookie(cookieName)) Assertion.Fail("Expected cookie '" + cookieName + "' to be set");
			return (string)cookies[cookieName];
		}

		/// <summary>
		/// The raw contents of the current page.
		/// </summary>
		public string CurrentPageText
		{
			get
			{
				return currentPage.ToString();
			}
		}

		internal XmlDocument CurrentPage
		{
			get 
			{
				if (currentPage == null) throw new NoPageException();
				return currentPage.Document;
			}
		}

		/// <summary>
		/// The total time this object has spent waiting for web servers to respond.
		/// </summary>
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
			HttpWebRequest request = CreateRequest(method, formVariables);
			SupportBasicAuth(request);
			HttpResponse response = SendRequest(request);
			ParseHttpResponse(response);
			if (response.IsRedirect) GetPage(response.RedirectUrl);
		}

		private void UpdateCurrentUrl(string url)
		{
			url = TrimFragmentIdentifier(url);
			if (currentUrl == null) currentUrl = new Uri(url);
			else currentUrl = new Uri(currentUrl, url);
		}

		/// <summary>
		/// A "fragment identifier" is the part of a URL that comes after a "#".  You don't
		/// see them too often.  It's a link within a document and web servers won't 
		/// recognize a URL that includes it.  This method strips off the fragment identifier, as 
		/// well as the pound sign (#) that precedes it.
		/// </summary>
		private string TrimFragmentIdentifier(string url)
		{
			int fragmentLocation = url.IndexOf('#');
			if (fragmentLocation < 0) return url;
			else return url.Substring(0, fragmentLocation);
		}

		private HttpWebRequest CreateRequest(string method, string formVariables)
		{
			HttpWebRequest request = null;
			if (method.ToLower() == "get") 
			{
				UriBuilder target = new UriBuilder(currentUrl);
				if (formVariables != "") 
				{
					target.Query += "?" + formVariables;
				}

				request = (HttpWebRequest)HttpWebRequest.Create(target.Uri);
				request.Method = "GET";
				request.UserAgent = UserAgent;
				request.AllowAutoRedirect = false;
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.SetCookies(target.Uri, CreateCookieString());
			} 
			else if (method.ToLower() == "post")
			{
				request = (HttpWebRequest)HttpWebRequest.Create(currentUrl);
				request.Method = "POST";
				request.AllowAutoRedirect = false;
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = formVariables.Length;
				StreamWriter sw = new StreamWriter(request.GetRequestStream());
				sw.Write(formVariables);
				sw.Close();
			}
			else
			{
				Assertion.Fail("Unknown HTTP method: " + method);
			}
			return request;
		}

		private void SupportBasicAuth(HttpWebRequest request)
		{
			if (currentUrl.UserInfo != null && !currentUrl.UserInfo.Equals("")) 
			{
				int delimiter = currentUrl.UserInfo.IndexOf(":");
				if (delimiter > 0) 
				{
					request.PreAuthenticate = true;
					string user = currentUrl.UserInfo.Substring(0, delimiter);
					string pwd = currentUrl.UserInfo.Substring(delimiter + 1);
					request.Credentials  = new NetworkCredential(user, pwd);
				}
			}
		}

		private HttpResponse SendRequest(HttpWebRequest request)
		{
			HttpWebResponse response = null;
			try 
			{
				DateTime start = DateTime.Now;
				response = (HttpWebResponse) request.GetResponse();
				serverTime += (DateTime.Now - start);
			} 
			catch (WebException wx) 
			{ 
				response = (HttpWebResponse)wx.Response;
				if (response == null) throw wx;
			}
			return new HttpResponse(response);
		}

		private void ParseHttpResponse(HttpResponse response)
		{
			if (response.IsNotFound) throw new NotFoundException(currentUrl);
			if (!response.IsOkay) 
			{
				Console.WriteLine(response.Body);
				throw new BadStatusException(response.StatusCode);
			}
			currentPage = new WebPage(response.Body);
			ParseCookies(response.Headers.GetValues("Set-Cookie"));
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

		private class HttpResponse
		{
			HttpWebResponse response;
			public string Body;

			public HttpResponse() { }

			public HttpResponse(HttpWebResponse wr) 
			{
				this.response = wr;
				StreamReader sr = new StreamReader(wr.GetResponseStream());
				Body = sr.ReadToEnd();
			}

			public WebHeaderCollection Headers 
			{
				get 
				{
					return response.Headers;
				}
			}

			public CookieCollection Cookies 
			{
				get 
				{
					return response.Cookies;
				}
			}

			public int StatusCode
			{
				get
				{
					return (int) response.StatusCode;
				}
			}

			public bool IsRedirect
			{
				get
				{
					return StatusCode == 302;
				}
			}

			public bool IsNotFound
			{
				get
				{
					return StatusCode == 404;
				}
			}

			public bool IsOkay
			{
				get
				{
					return IsRedirect || (StatusCode == 200);
				}
			}

			public string RedirectUrl
			{
				get
				{
					string location = response.Headers["Location"];
					if (location == null) throw new ApplicationException("Expected Location header in HTTP response");
					return location;
				}
			}
		}

		/// <summary>
		/// A request has been made that requires a page to have been loaded, but no
		/// page has been loaded yet.  Call GetPage() before calling the method that
		/// threw this exception.
		/// </summary>
		public class NoPageException : ApplicationException
		{
			internal NoPageException() : base("No pages have been loaded by the browser")
			{
			}
		}

		/// <summary>
		/// The requested URL was not found.  Correct the URL or determine what's wrong
		/// with the server.
		/// </summary>
		public class NotFoundException : ApplicationException
		{
			internal NotFoundException(Uri url) : base("404 Not Found for " + url)
			{
			}
		}

		/// <summary>
		/// The server returned an unexpected status code.  Determine what's wrong with
		/// the server.
		/// </summary>
		public class BadStatusException : ApplicationException
		{
			internal BadStatusException(int status) : base("Server returned error (status code: " + status + "}.  HTML copied to standard output.")
			{
			}
		}
	}
}