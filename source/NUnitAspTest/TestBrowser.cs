using System;
using NUnit.Framework;
using NUnit.Extensions.Asp;

namespace NUnit.Extensions.Asp.Test
{

	public class TestBrowser : WebFormTestCase {

		private const string BASE_URL = "http://localhost/NUnitAsp/NUnitAspTestPages/";

		public TestBrowser(string name) : base(name) {
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

		public void TestEmbeddedControls() {
			// This test needs to be expanded to use actual embedded control
			//Browser.EnterInputValue("_ctl0:account", "form");
		}
	}
}
