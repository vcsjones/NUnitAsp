#region Copyright (c) 2002, 2003, 2005, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, 2005, Brian Knowles, Jim Shore
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
    [TestFixture]
	public class CheckBoxTest : NUnitAspTestCase
	{
		protected CheckBoxTester CheckBox;
		protected CheckBoxTester DisabledCheckBox;
		protected LinkButtonTester Submit;
		
		protected override void SetUp()
		{
			base.SetUp();
			CheckBox = new CheckBoxTester("checkBox", CurrentWebForm);
			DisabledCheckBox = new CheckBoxTester("disabled", CurrentWebForm);
			Submit = new LinkButtonTester("submit", CurrentWebForm);
			Browser.GetPage(BaseUrl + "/AspTester/CheckBoxTestPage.aspx");
		}

        [Test]
		public void TestCheck()
		{
			AssertTrue("should not be checked", !CheckBox.Checked);
			CheckBox.Checked = true;
			AssertTrue("still shouldn't be checked - not submitted", !CheckBox.Checked);
			Submit.Click();
			AssertTrue("should be checked", CheckBox.Checked);
		}

        [Test]
		public virtual void TestUncheck()
		{
			TestCheck();

			CheckBox.Checked = false;
			AssertTrue("still should be checked - not submitted", CheckBox.Checked);
			Submit.Click();
			AssertTrue("shouldn't be checked", !CheckBox.Checked);
		}

        [Test]
		[ExpectedException(typeof(ControlDisabledException))]
		public void TestCheck_WhenDisabled()
		{
			DisabledCheckBox.Checked = true;
		}

        [Test]
		public void TestText()
		{
			AssertEquals("text", "Test me", CheckBox.Text);
		}

        [Test]
		public void TestText_WhenNone()
		{
			AssertEquals("no text", "", new CheckBoxTester("noText", CurrentWebForm).Text);
		}

        [Test]
		public void TestFormattedText()
		{
			AssertEquals("formatted text", "<b>bold!</b>", new CheckBoxTester("formattedText", CurrentWebForm).Text);
		}

        [Test]
		public void TestEnabled_True()
		{
			AssertEquals("enabled", true, CheckBox.Enabled);
		}

        [Test]
		public void TestEnabled_False()
		{
			AssertEquals("enabled", false, DisabledCheckBox.Enabled);
		}
	}
}
