#region Copyright (c) 2002-2005 Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005 Brian Knowles, Jim Shore
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
using System.IO;
using System.Net;
using System.Xml;

namespace NUnit.Extensions.Asp 
{
	/// <summary>
	/// A web client, capable of communicating with a web server.
	/// </summary>
	public class HttpClient
	{
		private const int MAX_REDIRECTS = 10;

		private TimeSpan serverTime = new TimeSpan(0);
		private Uri currentUrl = null;
		internal WebPage currentPage = null;
		private CookieContainer cookies = new CookieContainer();

		/// <summary>
		/// A User Agent string provided by Firefox 1.0.  Use this with <see cref="UserAgent"/>
		/// when you want NUnitAsp to see what Firefox sees.
		/// </summary>
		public const string FIREFOX_USER_AGENT = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.5) Gecko/20041107 Firefox/1.0";

		/// <summary>
		/// A user agent string provided by Internet Explorer 6.0.  Use this with <see cref="UserAgent"/>
		/// when you want NUnitAsp to see what IE sees.
		/// </summary>
		public const string IE_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.0.3705; .NET CLR 1.1.4322)";

		/// <summary>
		/// <p>The HttpClient that's used by default in all tests.</p>
		/// 
		/// <p>If you're creating a custom tester, don't use this property!
		/// Use ControlTester.<see cref="ControlTester.Browser"/> instead.</p>
		/// </summary>
		public static HttpClient Default = new HttpClient();

		/// <summary>
		/// <p>The user-agent string to send to the server. Useful if you want to pretend to
		/// be a specific browser.  Defaults to <see cref="IE_USER_AGENT"/>.</p>
		/// <p>Be careful when changing this, as ASP.NET renders controls differently
		/// for different user agents.  NUnitAsp's testers may not be able to recognize
		/// a control that was rendered for a different browser.</p>
		/// </summary>
		public string UserAgent = IE_USER_AGENT;

		/// <summary>
		/// The language-tag elements to send to the server (null if none). These appear 
		/// in the Request.UserLanguages array in the target page.
		/// </summary>
		public string[] UserLanguages = null;

		/// <summary>
		/// Username and password (null if none). Set automatically if the username and
		/// password are supplied in the URL (i.e., "http://username:password@host").
		/// Can be used with both "basic" and "Windows Integrated" (NTLM) authentication
		/// methods. Set this property to <c>CredentialCache.DefaultCredentials</c> 
		/// to use your current Windows login.
		/// </summary>
		public ICredentials Credentials = null;

		/// <summary>
		/// The proxy server information to use to proxy HTTP requests.
		/// If this property is set to null, the default value returned by
		/// GlobalProxySelection.Select is used.
		/// </summary>
		/// <example>
		///	Browser.Proxy = new WebProxy("http://myproxy:8080");
		/// </example>
		public IWebProxy Proxy = null;

		/// <summary>
		/// URL containing the hyperlink or form that caused the browser to
		/// load the current url (null if none).  Fragments aren't included
		/// (the part of the URL that comes after a '#').
		/// </summary>
		public Uri UrlReferrer = null;

		/// <summary>
		/// URL the browser most recently retrieved (null if none).  Fragments aren't
		/// included (the part of the URL that comes after a '#').
		/// </summary>
		public Uri CurrentUrl 
		{
			get 
			{
				return currentUrl;
			}
		}

		/// <summary>
		/// The cookies sent to the server.  These are usually set by the server but
		/// your test can add cookies to the container and they'll be sent too.
		/// </summary>
		public CookieContainer Cookies
		{
			get
			{
				return cookies;
			}
		}

		/// <summary>
		/// Retrieves a page from a web server.
		/// </summary>
		/// <param name="url">The URL of the page to get.</param>
		public void GetPage(string url)
		{
			DoWebRequest(url, "GET", string.Empty);
		}

		/// <summary>
		/// Retrieves a page from a web server.  Different from
		/// <see cref="GetPage"/> in that it sets the current page as the
		/// referrer.  You should use normally use <see cref="GetPage"/>.
		/// </summary>
		/// <param name="url"></param>
		public void FollowLink(string url)
		{
			TrackUrlReferrer();
			GetPage(url);
		}

		internal void SubmitForm(WebFormTester form)
		{
			TrackUrlReferrer();
			DoWebRequest(form.Action, form.Method, form.Variables.ToString());
		}

		/// <summary>
		/// Checks to see if a cookie has been set.
		/// </summary>
		/// <param name="cookieName">The name of the cookie.</param>
		/// <returns>'true' if the cookie has been set.</returns>
		public bool HasCookie(string cookieName) 
		{
			CookieCollection cc = cookies.GetCookies(currentUrl);
			return (cc[cookieName] != null);
		}

		/// <summary>
		/// Returns the value of a cookie.  Throws exception if the cookie hasn't been set.
		/// </summary>
		public string CookieValue(string cookieName)
		{
			if (!HasCookie(cookieName)) WebAssert.Fail("Expected cookie '" + cookieName + "' to be set");
			CookieCollection cc = cookies.GetCookies(currentUrl);
			return cc[cookieName].Value;
		}

		/// <summary>
		/// The raw contents of the current page.
		/// </summary>
		public string CurrentPageText
		{
			get
			{
				return CurrentPage.ToString();
			}
		}

		internal WebPage CurrentPage
		{
			get
			{
				if (currentPage == null) throw new NoPageException();
				return currentPage;
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

		private void DoWebRequest(string url, string method, string formVariables)
		{
			bool isPost = method.ToLower() == "post";
			if (!isPost && method.ToLower() != "get")
			{
				throw new ArgumentException("Unknown HTTP method: " + method, "method");
			}
			UpdateCurrentUrl(url, !isPost, formVariables);
			UpdateCredentialsFromUrl();

			HttpWebRequest request = CreateRequest(method, formVariables);
			if (isPost)
			{
				WriteRequestBody(request, formVariables);
			}
			HttpWebResponse response = SendRequest(request);
			currentUrl = GetUrlFromResponse(response);
			ReadHttpResponse(response);
		}

		private string TrimFragmentIdentifier(string url)
		{
			// A "fragment identifier" is the part of a URL that comes after a "#".  You don't
			// see them too often.  It's a link within a document and web servers won't 
			// recognize a URL that includes it.  This strips off the fragment identifier, as 
			// well as the pound sign (#) that precedes it.
			int fragmentLocation = url.IndexOf('#');

			if (fragmentLocation < 0)
			{
				return url;
			}
			else
			{
				return url.Substring(0, fragmentLocation);
			}
		}

		private void UpdateCurrentUrl(string url, bool isGet, string formVariables)
		{
			url = TrimFragmentIdentifier(url);
			if (currentUrl == null) 
			{
				currentUrl = new Uri(url);
			}
			else 
			{	
				currentUrl = new Uri(currentUrl, url);
			}

			if (isGet && formVariables != "")
			{
				UriBuilder target = new UriBuilder(currentUrl);

				target.Query += "?" + formVariables;
				currentUrl = target.Uri;
			}
		}

		private Uri GetUrlFromResponse(HttpWebResponse response)
		{
			return new Uri(TrimFragmentIdentifier(response.ResponseUri.AbsoluteUri));
		}

		private HttpWebRequest CreateRequest(string method, string formVariables)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(currentUrl);

			request.Method = method.ToUpper();
			request.CookieContainer = cookies;
			request.UserAgent = UserAgent;
			request.AllowAutoRedirect = true;
			request.MaximumAutomaticRedirections = MAX_REDIRECTS;

			if (UrlReferrer != null)
			{
				request.Referer = UrlReferrer.AbsoluteUri;
			}

			if (Credentials != null)
			{
				request.PreAuthenticate = true;
				request.Credentials = Credentials;
			}

			if (Proxy != null)
			{
				request.Proxy = Proxy;
			}

			AddUserLanguageHeaders(request);
			return request;
		}

		private void WriteRequestBody(HttpWebRequest request, string formVariables)
		{
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = formVariables.Length;

			using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
			{
				writer.Write(formVariables);
			}
		}

		private string GetRedirectUrl(HttpWebResponse response)
		{
			string location = response.Headers["Location"];
			if (location == null)
			{
				throw new ApplicationException("Expected Location header in HTTP response");
			}
			return location;
		}

		private	void AddUserLanguageHeaders(HttpWebRequest request)
		{
			if (UserLanguages == null) return;

			string languages = "";
			string separator = "";
			foreach (string language in UserLanguages)
			{
				languages += separator + language;
				separator = ", ";
			}
			request.Headers.Add("Accept-Language", languages);
		}

		private void UpdateCredentialsFromUrl()
		{
			if (currentUrl.UserInfo == null) return;

			int delimiter = currentUrl.UserInfo.IndexOf(":");
			if (delimiter == -1) return;

			string user = currentUrl.UserInfo.Substring(0, delimiter);
			string pwd = currentUrl.UserInfo.Substring(delimiter + 1);
			Credentials = new NetworkCredential(user, pwd);
		}

		private void TrackUrlReferrer()
		{
			UrlReferrer = CurrentUrl;
		}

		private HttpWebResponse SendRequest(HttpWebRequest request)
		{
			try 
			{
				DateTime start = DateTime.Now;
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				serverTime += (DateTime.Now - start);

				return response;
			} 
			catch (WebException e) 
			{ 
				if (e.Response == null)
				{
					throw;
				}
				return (HttpWebResponse)e.Response;
			}
		}

		private void ReadHttpResponse(HttpWebResponse response)
		{
			switch (response.StatusCode)
			{
				case HttpStatusCode.NotFound:
					throw new NotFoundException(currentUrl);
				case HttpStatusCode.Redirect:
					throw new RedirectLoopException(GetRedirectUrl(response));
			}

			string body;
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				body = reader.ReadToEnd();
			}

			if (response.StatusCode != HttpStatusCode.OK)
			{
				if (response.StatusCode == HttpStatusCode.InternalServerError)
				{
					string exceptionMessage = ParseStackTrace(body);
					if (exceptionMessage != null) throw new AspServerException(exceptionMessage);
				}

				Console.WriteLine(body);
				throw new BadStatusException(response.StatusCode);
			}
			currentPage = new WebPage(body);
		}

		/// <summary>
		/// Returns null if stack trace couldn't be found.
		/// </summary>
		private string ParseStackTrace(string aspExceptionPageHtml)
		{
			XmlNodeList errorInfo = new WebPage(aspExceptionPageHtml).Document.ChildNodes;
			if (errorInfo.Count == 2 && errorInfo[1] is XmlComment)
			{
				return errorInfo[1].Value.Trim();
			}
			return null;
		}


		/// <summary>
		/// Exception: A request has been made that requires a page to have been loaded, but no
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
		/// Exception: The requested URL was not found.  Correct the URL or determine what's wrong
		/// with the server.
		/// </summary>
		public class NotFoundException : ApplicationException
		{
			internal NotFoundException(Uri url) : base("404 Not Found for " + url)
			{
			}
		}

		/// <summary>
		/// Exception: The requested URL caused an unhandled exception on the ASP.NET server.
		/// Fix the production code so it doesn't throw the exception.
		/// </summary>
		public class AspServerException : ApplicationException
		{
			internal AspServerException(string exceptionStackTrace) : 
				base("Server threw an exception:\r\n" + exceptionStackTrace)
			{
			}
		}

		/// <summary>
		/// Exception: The server returned an unexpected status code.  Determine what's wrong with
		/// the server.
		/// </summary>
		public class BadStatusException : ApplicationException
		{
			/// <summary>
			/// The HTTP status code returned by the server
			/// </summary>
			public readonly HttpStatusCode Status;

			internal BadStatusException(HttpStatusCode status) : 
				base("Server returned error (status code: " + (int)status + ").  HTML copied to standard output.")
			{
				Status = status;
			}
		}

		/// <summary>
		/// Exception: Too many HTTP redirects were detected. Check for infinite redirection loop.
		/// </summary>
		public class RedirectLoopException : ApplicationException
		{
			/// <summary>
			/// The target URL of the failed redirect
			/// </summary>
			public readonly string TargetUrl;

			internal RedirectLoopException(string targetUrl) : base(GetMessage(targetUrl))
			{
				TargetUrl = targetUrl;
			}

			private static string GetMessage(string targetUrl)
			{
				return string.Format(
					"Redirect loop detected: more than {0} redirections.  Most recent redirect was to {1}",
					MAX_REDIRECTS, targetUrl);
			}
		}
	}
}
