using System;
using NUnit.Framework;
using NUnit.Extensions.Asp;

namespace NUnit.Extensions.Asp.Test
{

	public class TestBrowser : WebFormTestCase {

		private const string BASE_URL = "http://localhost/NUnitAsp/NUnitAspTestPages";

		public TestBrowser(string name) : base(name) {
		}

		public override string PageName {
			get {
				return "BasicPage";
			}
		}

		public override string StartUrl {
			get {
				return "http://localhost/NUnitAsp/NUnitAspTestPages/BasicPage.html";
			}
		}

		public void TestRedirection() {
			_browser.GetPage("RedirectorPage.aspx");
			AssertPageName("RedirecteePage");
		}

		public void TestCookies() {
			_browser.GetPage("CookieDropPage.aspx");
			AssertCookieExists("testcookie");
			_browser.GetPage("CookieDisplayPage.aspx");
			AssertLabelText("cookies", "testcookievalue");
		}

		public void TestCookiesPreservedOverTime() {
			_browser.GetPage("CookieDropPage.aspx");
			AssertCookieExists("testcookie");
			_browser.GetPage("RedirectorPage.aspx");
			_browser.GetPage("CookieDisplayPage.aspx");
			AssertLabelText("cookies", "testcookievalue");
		}

		public void TestLinkButton() {
			_browser.GetPage("LinkButtonPage.aspx");
			AssertPageName("LinkButton");
			AssertLabelText("status", "unclicked");
			_browser.ClickLinkButton("link", "PostbackPage");
			AssertLabelText("status", "clicked");
		}

		public void TestEmbeddedControls() {
			// This test needs to be expanded to use actual embedded control
			_browser.EnterInputValue("_ctl0:account", "form");
		}
	}
}
