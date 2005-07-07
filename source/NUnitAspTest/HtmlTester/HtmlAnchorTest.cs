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
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{

	public class HtmlAnchorTest : NUnitAspTestCase
	{
		private HtmlAnchorTester testLink;
		private HtmlAnchorTester popupLink;
		private HtmlAnchorTester serverLink;
		private HtmlAnchorTester disabledLink;

		protected override void SetUp()
		{
			base.SetUp();
			testLink = new HtmlAnchorTester("testLink", CurrentWebForm, true);
			popupLink = new HtmlAnchorTester("popupLink", CurrentWebForm, false);
			serverLink = new HtmlAnchorTester("serverLink", CurrentWebForm, false);
			disabledLink = new HtmlAnchorTester("disabledLink", CurrentWebForm, false);
			Browser.GetPage(BaseUrl + "HtmlTester/HtmlAnchorTestPage.aspx");
		}

		public void TestHRef()
		{
			AssertEquals("url", "../RedirectionTarget.aspx?a=a&b=b", testLink.HRef);
		}

		public void TestDisabled_True()
		{
			AssertEquals("disabled", true, disabledLink.Disabled);
		}

		public void TestDisabled_False()
		{
			AssertEquals("disabled", false, testLink.Disabled);
		}

		public void TestClick()
		{
			testLink.Click();
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		public void TestClick_WhenDisabled()
		{
			disabledLink.Click();
			// Yes, you can click a disabled link (at least in IE)
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		public void TestServerClick()
		{
			serverLink.Click();
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

		public void TestPopupLink()
		{
			AssertEquals("popup", "../RedirectionTarget.aspx", popupLink.PopupLink);
		}

		public void TestPopupWindowClick()
		{
			popupLink.Click();
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}
	}
}
