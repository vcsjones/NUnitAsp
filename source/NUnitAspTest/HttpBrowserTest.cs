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

namespace NUnit.Extensions.Asp.Test
{

	public class HttpBrowserTest : NUnitAspTestCase
	{
		private static readonly string TestUrl = BaseUrl + "HttpBrowserTestPage.aspx";

		private AspLinkButton redirect;
		private AspLinkButton dropCookie;
		private AspLinkButton dropCookieAndRedirect;
		private AspLabel cookie;

		public HttpBrowserTest(string name) : base(name)
		{
		}

		protected override void SetUp()
		{
			base.SetUp();
			redirect = new AspLinkButton("redirect", CurrentWebForm);
			dropCookie = new AspLinkButton("dropCookie", CurrentWebForm);
			dropCookieAndRedirect = new AspLinkButton("dropCookieAndRedirect", CurrentWebForm);
			cookie = new AspLabel("cookie", CurrentWebForm);
		}

		public void TestGetPage()
		{
			Browser.GetPage(TestUrl);
			AssertEquals("HttpBrowserTestPage", CurrentWebForm.AspId);
		}

		public void TestGetNonExistentPage()
		{
			try
			{
				Browser.GetPage("foodle");
				Fail("Expected exception");
			}
			catch(WebException)
			{
				Assert("Expected", true);
			}
		}

		public void TestRedirect()
		{
			Browser.GetPage(TestUrl);
			redirect.Click();
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		public void TestCookies()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookie.Click();
			AssertCookieSet();
		}

		public void TestCookiesPreserved()
		{
			Browser.GetPage(TestUrl);
			AssertCookieNotSet();
			dropCookie.Click();
			AssertCookieSet();
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

		private void AssertCookieNotSet()
		{
			AssertEquals("Not Set", cookie.Text);
		}

		private void AssertCookieSet()
		{
			AssertEquals("TestCookieValue", cookie.Text);
		}

	}
}
