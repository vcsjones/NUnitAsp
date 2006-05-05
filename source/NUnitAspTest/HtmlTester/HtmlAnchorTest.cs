#region Copyright (c) 2002-2005, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005, Brian Knowles, Jim Shore
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
using NUnit.Framework;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class HtmlAnchorTest : NUnitAspTestCase
	{
		private HtmlAnchorTester testLink;
		private HtmlAnchorTester popupLink;
		private HtmlAnchorTester serverLink;
		private HtmlAnchorTester disabledLink;

		protected override void SetUp()
		{
			base.SetUp();
			testLink = new HtmlAnchorTester("testLink", CurrentWebForm);
			popupLink = new HtmlAnchorTester("popupLink");
			serverLink = new HtmlAnchorTester("serverLink");
			disabledLink = new HtmlAnchorTester("disabledLink");
			Browser.GetPage(BaseUrl + "HtmlTester/HtmlAnchorTestPage.aspx");
		}

		[Test]
		public void TestHRef()
		{
			AssertEquals("url", "../RedirectionTarget.aspx?a=a&b=b", testLink.HRef);
		}

		[Test]
		public void TestDisabled_True()
		{
			AssertEquals("disabled", true, disabledLink.Disabled);
		}

		[Test]
		public void TestDisabled_False()
		{
			AssertEquals("disabled", false, testLink.Disabled);
		}

		[Test]
		public void TestClick()
		{
			testLink.Click();
			AssertRedirected();
		}

		[Test]
		public void TestClick_WhenDisabled()
		{
			disabledLink.Click();
			// Yes, you can click a disabled link (at least in IE)
			AssertRedirected();
		}

		[Test]
		public void TestServerClick()
		{
			serverLink.Click();
			AssertRedirected();
		}

		[Test]
		public void TestPopupLink()
		{
			AssertEquals("popup", "../RedirectionTarget.aspx", popupLink.PopupLink);
		}

		[Test]
		public void TestPopupWindowClick()
		{
			popupLink.Click();
			AssertRedirected();
		}
	}
}
