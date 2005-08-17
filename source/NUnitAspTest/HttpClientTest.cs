#region Copyright (c) 2002-2004, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2004, Brian Knowles, Jim Shore
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
using System.Net;

using NUnit.Framework;
using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.AspTester;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test
{
	public class HttpClientTest : NUnitAspTestCase
	{
		public const string TEST_COOKIE_NAME = "TestCookieName";
		public const string TEST_COOKIE_VALUE = "TestCookieValue";
		private static readonly string TestUrl = BaseUrl + "HttpClientTestPage.aspx";

		private LinkButtonTester redirect;
		private LinkButtonTester dropCookie;
		private LinkButtonTester dropCookieAndRedirect;
		private LinkButtonTester dropCookieWithExpiry;
		private LinkButtonTester postBack;
		private HtmlAnchorTester followLink;
		private LabelTester cookie;
		private LabelTester testParm;
		private LabelTester urlReferrer;

		protected override void SetUp()
		{
			base.SetUp();
			redirect = new LinkButtonTester("redirect", CurrentWebForm);
			dropCookie = new LinkButtonTester("dropCookie", CurrentWebForm);
			dropCookieAndRedirect = new LinkButtonTester("dropCookieAndRedirect", CurrentWebForm);
			dropCookieWithExpiry = new LinkButtonTester("dropCookieWithExpiry", CurrentWebForm);
			postBack = new LinkButtonTester("postBack", CurrentWebForm);
			followLink = new HtmlAnchorTester("followLink", CurrentWebForm);
			cookie = new LabelTester("cookie", CurrentWebForm);
			testParm = new LabelTester("testParm", CurrentWebForm);
			urlReferrer = new LabelTester("urlReferrer", CurrentWebForm);
		}

		[Test]
		public void TestGetAndPostPage()
		{
			AssertNull("current url should be null", Browser.CurrentUrl);
			Browser.GetPage(TestUrl);
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
			AssertEquals("current url", TestUrl, Browser.CurrentUrl.AbsoluteUri);
			postBack.Click();
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
			AssertEquals("Clicked", new LabelTester("postBackStatus", CurrentWebForm).Text);
		}

		[Test]
		public void TestRelativeGet()
		{
			Browser.GetPage(TestUrl);
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
			Browser.GetPage("RedirectionTarget.aspx");
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		[Test]
		public void TestGetWithFragment()
		{
			Browser.GetPage(TestUrl + "#fragment");
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
			AssertEquals("current url", TestUrl, Browser.CurrentUrl.AbsoluteUri);
		}

		[Test]
		public void TestGetWithUrlEncoding()
		{
			string query = "?testparm=some+%2b+text";
			Browser.GetPage(TestUrl + query + "#fragment");

			AssertEquals("some + text", testParm.Text);
			AssertEquals("current url", TestUrl + query, Browser.CurrentUrl.AbsoluteUri);
		}

		[Test]
		public void TestGetThroughProxy()
		{
			UriBuilder target = new UriBuilder(TestUrl);
			Browser.Proxy = new WebProxy(target.Host, target.Port);

			target.Host = "no.such.host.anywhere";
			Browser.GetPage(target.Uri.AbsoluteUri);

			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
		}

		[Test]
		public void TestRedirect()
		{
			Browser.GetPage(TestUrl);
			redirect.Click();
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
			AssertEquals(BaseUrl + "RedirectionTarget.aspx", Browser.CurrentUrl.AbsoluteUri);
		}

		[Test]
		[ExpectedException(typeof(HttpClient.RedirectLoopException))]
		public void TestInfiniteRedirect()
		{
			Browser.GetPage(BaseUrl + "InfiniteRedirector.aspx");
		}

		[Test]
		public void TestRedirectWhenRedirectorPageIsUnparseable()
		{
			Browser.GetPage(BaseUrl + "MalformedRedirector.aspx");
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		[Test]
		public void TestCookies()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookie.Click();
			Browser.GetPage(TestUrl);
			AssertCookieSet();
			AssertEquals("Browser.CookieValue", TEST_COOKIE_VALUE, Browser.CookieValue(TEST_COOKIE_NAME));
		}

		[Test]
		public void TestCookiesPreserved()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookie.Click();
			Browser.GetPage(TestUrl);
			AssertCookieSet();
			redirect.Click();
			Browser.GetPage(TestUrl);
			AssertCookieSet();
		}

		[Test]
		public void TestCookieDuringRedirect()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookieAndRedirect.Click();
			Browser.GetPage(TestUrl);
			AssertCookieSet();
		}

		[Test]
		public void TestCookiesWithExpiry()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookieWithExpiry.Click();
			Browser.GetPage(TestUrl);
			AssertCookieSet();
			AssertEquals("Browser.CookieValue", TEST_COOKIE_VALUE, Browser.CookieValue(TEST_COOKIE_NAME));
		}

		[Test]
		[ExpectedException(typeof(HttpClient.NotFoundException))]
		public void Test404NotFound()
		{
			Browser.GetPage(BaseUrl + "NonExistant.html");
		}

		[Test]
		public void TestUserAgent()
		{
			LabelTester userAgent = new LabelTester("userAgent", CurrentWebForm);

			Browser.GetPage(TestUrl);
			AssertEquals("default user agent", HttpClient.IE_USER_AGENT, userAgent.Text);
			Browser.UserAgent = "Foo Explorer/4.2";
			Browser.GetPage(TestUrl);
			AssertEquals("modified user agent", "Foo Explorer/4.2", userAgent.Text);
		}

		[Test]
		public void TestElapsedServerTime()
		{
			Browser.GetPage(BaseUrl + "ServerTimeTestPage.aspx");
			AssertTrue("Elapsed server time should not be zero", Browser.ElapsedServerTime > TimeSpan.Zero);
		}

		[Test]
		public void TestUserLanguages()
		{
			LabelTester userLanguages = new LabelTester("userLanguages", CurrentWebForm);

			Browser.GetPage(TestUrl);
			AssertEquals("default user language", "Not Set", userLanguages.Text);

			Browser.UserLanguages = new string[] {"en-gb"};
			Browser.GetPage(TestUrl);
			AssertEquals("modified single user language", "[en-gb]", userLanguages.Text);

			Browser.UserLanguages = new string[] {"en-us", "en-gb"};
			Browser.GetPage(TestUrl);
			AssertEquals("modified multiple user languages", "[en-us][en-gb]", userLanguages.Text);
		}

		[Test]
		public void TestAspErrorParsing()
		{
			try
			{
				Browser.GetPage(BaseUrl + "AspErrorTestPage.aspx");
				Fail("Expected AspServerException");
			}
			catch (HttpClient.AspServerException e)
			{
				string expectedMessage = "[ApplicationException]: This is an ASP.NET exception message";
				AssertTrue("incorrect exception message", e.Message.IndexOf(expectedMessage) >= 0);
			}
		}

		[Test]
		public void TestGetPageDoesNotTrackReferrer()
		{
			Browser.GetPage(TestUrl);
			// one more time
			Browser.GetPage(TestUrl);

			AssertNull(Browser.UrlReferrer);
			AssertEquals("Not Set", urlReferrer.Text);
		}

		[Test]
		public void TestFollowLinkDoesTrackReferrer()
		{
			Browser.GetPage(TestUrl);
			followLink.Click();

			AssertEquals(Browser.CurrentUrl, Browser.UrlReferrer);
			AssertEquals(TestUrl, urlReferrer.Text);
		}

		[Test]
		public void TestPostBackDoesTrackReferrer()
		{
			Browser.GetPage(TestUrl);
			postBack.Click();

			AssertEquals(Browser.CurrentUrl, Browser.UrlReferrer);
			AssertEquals(TestUrl, urlReferrer.Text);
		}

		[Test]
		public void TestCanExplicitlySetUrlReferrer()
		{
			Browser.UrlReferrer = new Uri("http://somewhere/something");
			Browser.GetPage(TestUrl);

			AssertEquals("http://somewhere/something", urlReferrer.Text);
		}

		private CookieCollection GetActiveCookies()
		{
			return Browser.Cookies.GetCookies(Browser.CurrentUrl);
		}

		private void AssertCookieNotSet()
		{
			AssertEquals("Not Set", cookie.Text);
			AssertNull("Must be null", GetActiveCookies()[TEST_COOKIE_NAME]);
		}

		private void AssertCookieSet()
		{
			AssertEquals(TEST_COOKIE_VALUE, cookie.Text);

			Cookie theCookie = GetActiveCookies()[TEST_COOKIE_NAME];
			AssertNotNull("Must not be null", theCookie);
			AssertEquals(TEST_COOKIE_VALUE, theCookie.Value);
		}
	}
}
