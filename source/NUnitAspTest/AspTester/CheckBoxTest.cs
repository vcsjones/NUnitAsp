#region Copyright (c) 2002, 2003, Brian Knowles, Jim Little
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, Brian Knowles, Jim Little
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
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test.AspTester
{
	public class CheckBoxTest : NUnitAspTestCase
	{
		private CheckBoxTester checkBox;
		private CheckBoxTester disabledCheckBox;
		private LinkButtonTester submit;
		
		protected override void SetUp()
		{
			base.SetUp();
			checkBox = new CheckBoxTester("checkBox", CurrentWebForm);
			disabledCheckBox = new CheckBoxTester("disabled", CurrentWebForm);
			submit = new LinkButtonTester("submit", CurrentWebForm);
			Browser.GetPage(BaseUrl + "/AspTester/CheckBoxTestPage.aspx");
		}

		public void TestCheck()
		{
			Assert("should not be checked", !checkBox.Checked);
			checkBox.Checked = true;
			Assert("still shouldn't be checked - not submitted", !checkBox.Checked);
			submit.Click();
			Assert("should be checked", checkBox.Checked);
		}

		public void TestUncheck()
		{
			TestCheck();

			checkBox.Checked = false;
			Assert("still should be checked - not submitted", checkBox.Checked);
			submit.Click();
			Assert("shouldn't be checked", !checkBox.Checked);
		}

        [ExpectedException(typeof(ControlDisabledException))]
        public void TestCheck_WhenDisabled()
		{
			disabledCheckBox.Checked = true;
		}

		public void TestText()
		{
			AssertEquals("text", "Test me", checkBox.Text);
		}

		public void TestText_WhenNone()
		{
			AssertEquals("no text", "", new CheckBoxTester("noText", CurrentWebForm).Text);
		}

		public void TestFormattedText()
		{
			AssertEquals("formatted text", "<b>bold!</b>", new CheckBoxTester("formattedText", CurrentWebForm).Text);
		}

		public void TestEnabled_True()
		{
			AssertEquals("enabled", true, checkBox.Enabled);
		}

		public void TestEnabled_False()
		{
			AssertEquals("enabled", false, disabledCheckBox.Enabled);
		}
	}
}
