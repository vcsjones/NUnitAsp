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
using System.Net.Sockets;
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

		internal void SetFormVariable(string name, string value) 
		{
			currentPage.SetFormVariable(name, value);
		}

		public bool HasCookie(string cookieName) 
		{
			return cookies.ContainsKey(cookieName);
		}

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

		public TimeSpan ElapsedServerTime 
		{
			get 
			{
				return serverTime;
			}
		}

		private void DoHttp(string url, string method, string formVariables)
		{
			TcpClient tcp = null;
			UpdateCurrentUrl(url);

			// This is an ugly, ugly hack, so it deserves comment.
			// We've seen a weird, intermittent problem in which the server
			// closes the connection in the middle of returning data to us.
			// So the ugly ugly hack here is to simply retry, up to three times.
			// A real fix is needed.  Please fix me.  Please!
			int numTries = 0;
			bool socketProblemOccurred = true;
			while (socketProblemOccurred && numTries < 3)
			{
				try
				{
					socketProblemOccurred = false;
					numTries++;
					tcp = new TcpClient(currentUrl.Host, currentUrl.Port);
					NetworkStream stream = tcp.GetStream();
					SendHttpRequest(stream, method, formVariables);
					HttpResponse response = ReadHttpResponse(stream);
					tcp.Close();
					ParseHttpResponse(response);
					if (response.IsRedirect) GetPage(response.RedirectUrl);
				}
				catch (IOException e)
				{
					if (e.GetBaseException() is SocketException)
					{
						Console.WriteLine("Attempt #" + numTries + ": " + e);
						socketProblemOccurred = true;
					}
					else throw e;
				}
				finally
				{
					if (tcp != null) tcp.Close();
				}
			}
			if (socketProblemOccurred) throw new ApplicationException("Got socket exception three times in a row... giving up.  Somebody please fix this bug!");
		}

		private void UpdateCurrentUrl(string url)
		{
			if (currentUrl == null) currentUrl = new Uri(url);
			else currentUrl = new Uri(currentUrl, url);
		}

		private void SendHttpRequest(NetworkStream stream, string method, string formVariables)
		{
			string url = currentUrl.ToString();
			url = url.Replace(' ', '+');
			if (method.ToLower() == "get" && formVariables != "") url += "?" + formVariables;

			StreamWriter writer = new StreamWriter(stream, Encoding.ASCII);
			writer.NewLine = "\r\n";
			writer.WriteLine("{0} {1} HTTP/1.0", method.ToUpper(), url);
			writer.WriteLine("User-Agent: NUnitAsp/0.x");
			writer.WriteLine("Cookie: {0}", CreateCookieString());
			if (method.ToLower() == "get") 
			{
				writer.WriteLine();
			}
			else if (method.ToLower() == "post")
			{
				writer.WriteLine("Content-Type: application/x-www-form-urlencoded");
				writer.WriteLine("Content-Length: {0}", formVariables.Length);
				writer.WriteLine();
				writer.WriteLine(formVariables);
			}
			else
			{
				throw new ApplicationException("Unknown method: '" + method + "'");
			}
			writer.Flush();
		}

		private HttpResponse ReadHttpResponse(NetworkStream stream)
		{
			StreamReader reader = new StreamReader(stream, Encoding.UTF8);
			HttpResponse response = new HttpResponse();

			response.SetStatus(reader.ReadLine());
			for (string line = reader.ReadLine(); line != null && line != ""; line = reader.ReadLine()) 
			{
				response.AddHeader(line);
			}
			response.Body = reader.ReadToEnd();
			return response;
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

		private string GetBaseAddress(string url) 
		{
			Uri uri = new Uri(url);
			return uri.AbsoluteUri;
		}

		private class HttpResponse
		{
			public int statusCode;
			public NameValueCollection Headers = new NameValueCollection();
			public string Body;

			public void AddHeader(string headerLine)
			{
				Regex headerRegex = new Regex(@"^(?<name>.*?):[ \t]+(?<value>.*)$");
				if (!headerRegex.IsMatch(headerLine)) throw new ApplicationException("Expected '" + headerLine + "' to be a valid HTTP header");
				Match header = headerRegex.Match(headerLine);
				string name = header.Groups["name"].Captures[0].Value;
				string value = header.Groups["value"].Captures[0].Value;

				Headers.Add(name, value);
			}

			public void SetStatus(string statusLine)
			{
				string[] elements = statusLine.Split();
				statusCode = int.Parse(elements[1]);
			}

			public int StatusCode
			{
				get
				{
					return statusCode;
				}
			}

			public bool IsRedirect
			{
				get
				{
					return statusCode == 302;
				}
			}

			public bool IsNotFound
			{
				get
				{
					return statusCode == 404;
				}
			}

			public bool IsOkay
			{
				get
				{
					return IsRedirect || (statusCode == 200);
				}
			}

			public string RedirectUrl
			{
				get
				{
					string location = Headers["Location"];
					if (location == null) throw new ApplicationException("Expected Location header in HTTP response");
					return location;
				}
			}
		}

		public class NoPageException : ApplicationException
		{
			internal NoPageException() : base("No pages have been loaded by the browser")
			{
			}
		}

		public class NotFoundException : ApplicationException
		{
			internal NotFoundException(Uri url) : base("404 Not Found for " + url)
			{
			}
		}

		public class BadStatusException : ApplicationException
		{
			internal BadStatusException(int status) : base("Server returned error (status code: " + status + "}.  HTML copied to standard output.")
			{
			}
		}
	}
}