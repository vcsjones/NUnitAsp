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

namespace NUnit.Extensions.Asp.Test
{

	public class BrowserTest : WebFormTestCase {

		private const string BASE_URL = "http://localhost/NUnitAsp/NUnitAspTestPages/";

		public BrowserTest(string name) : base(name) {
		}

		protected override void SetUp() {
			Browser = new Browser(BASE_URL + "BasicPage.html");
		}

		public void TestRedirection() {
			Browser.GetPage("RedirectorPage.aspx");
			Page.AssertIdEquals("RedirecteePage");
		}

		public void TestCookies() {
			Browser.GetPage("CookieDropPage.aspx");
			Assert(Browser.HasCookie("testcookie"));
			Browser.GetPage("CookieDisplayPage.aspx");
			Page.GetLabel("cookies").AssertTextEquals("testcookievalue");
		}

		public void TestCookiesPreservedOverTime() {
			Browser.GetPage("CookieDropPage.aspx");
			Assert(Browser.HasCookie("testcookie"));
			Browser.GetPage("RedirectorPage.aspx");
			Browser.GetPage("CookieDisplayPage.aspx");
			Page.GetLabel("cookies").AssertTextEquals("testcookievalue");
		}

		public void TestLinkButton() {
			Browser.GetPage("LinkButtonPage.aspx");
			Page.AssertIdEquals("LinkButton");
			Page.GetLabel("status").AssertTextEquals("unclicked");
			Page.GetLinkButton("link").Click();
			Page.GetLabel("status").AssertTextEquals("clicked");
		}

	}
}
