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
using NUnit.Framework;
using NUnit.Extensions.Asp;
using System.Net;
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test
{
	public class HttpClientTest : NUnitAspTestCase
	{
		public const string TEST_COOKIE_NAME = "TestCookieName";
		public  const string TEST_COOKIE_VALUE = "TestCookieValue";
		private static readonly string TestUrl = BaseUrl + "HttpClientTestPage.aspx";

		private LinkButtonTester redirect;
		private LinkButtonTester dropCookie;
		private LinkButtonTester dropCookieAndRedirect;
		private LinkButtonTester postBack;
		private LabelTester cookie;

		protected override void SetUp()
		{
			base.SetUp();
			redirect = new LinkButtonTester("redirect", CurrentWebForm);
			dropCookie = new LinkButtonTester("dropCookie", CurrentWebForm);
			dropCookieAndRedirect = new LinkButtonTester("dropCookieAndRedirect", CurrentWebForm);
			postBack = new LinkButtonTester("postBack", CurrentWebForm);
			cookie = new LabelTester("cookie", CurrentWebForm);
		}

		public void TestGetAndPostPage()
		{
			Browser.GetPage(TestUrl);
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
			postBack.Click();
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
			AssertEquals("Clicked", new LabelTester("postBackStatus", CurrentWebForm).Text);
		}

		public void TestRelativeGet()
		{
			Browser.GetPage(TestUrl);
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
			Browser.GetPage("RedirectionTarget.aspx");
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		public void TestGetWithFragment()
		{
			Browser.GetPage(TestUrl + "#fragment");
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
		}

		public void TestRedirect()
		{
			Browser.GetPage(TestUrl);
			redirect.Click();
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		public void TestRedirectWhenRedirectorPageIsUnparseable()
		{
			Browser.GetPage(BaseUrl + "MalformedRedirector.aspx");
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		public void TestCookies()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookie.Click();
			Browser.GetPage(TestUrl);
			AssertCookieSet();
			AssertEquals("Browser.CookieValue", TEST_COOKIE_VALUE, Browser.CookieValue(TEST_COOKIE_NAME));
		}

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

		public void TestCookieDuringRedirect()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookieAndRedirect.Click();
			Browser.GetPage(TestUrl);
			AssertCookieSet();
		}

		public void Test404NotFound()
		{
			try
			{
				Browser.GetPage("http://localhost/nonexistant.html");
				Fail("Expected NotFoundException");
			}
			catch (HttpClient.NotFoundException)
			{
				Assert("correct behavior", true);
			}
		}

		public void TestUserAgent()
		{
			LabelTester userAgent = new LabelTester("userAgent", CurrentWebForm);

			Browser.GetPage(TestUrl);
			AssertEquals("default user agent", "NUnitAsp", userAgent.Text);
			Browser.UserAgent = "Foo Explorer/4.2";
			Browser.GetPage(TestUrl);
			AssertEquals("modified user agent", "Foo Explorer/4.2", userAgent.Text);
		}

		private void AssertCookieNotSet()
		{
			AssertEquals("Not Set", cookie.Text);
		}

		private void AssertCookieSet()
		{
			AssertEquals(TEST_COOKIE_VALUE, cookie.Text);
		}
	}
}
