#region Copyright (c) 2005, James Shore
/********************************************************************************************************************
'
' Copyright (c) 2005, James Shore
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
using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.HtmlTester;
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class HtmlInputRadioButtonTest : NUnitAspTestCase
	{
		private HtmlInputRadioButtonTester button1A;
		private HtmlInputRadioButtonTester button1B;
		private LinkButtonTester postback;
		private DataGridTester formVars;

		protected override void SetUp()
		{
			button1A = new HtmlInputRadioButtonTester("button1A");
			button1B = new HtmlInputRadioButtonTester("button1B");
			postback = new LinkButtonTester("postback");
			formVars = new DataGridTester("formVars", CurrentWebForm);

			Browser.GetPage(BaseUrl + "HtmlTester/HtmlInputRadioButtonTestPage.aspx");
		}
		
		[Test]
		public void TestChecked_OnGet()
		{
			Assert.IsFalse(button1A.Checked, "button 1A should be unchecked");
			Assert.IsTrue(button1B.Checked, "button 1B should be checked");
		}

		[Test]
		public void TestChecked_OnSetToTrue()
		{
			button1A.Checked = true;
			postback.Click();

			Assert.IsTrue(button1A.Checked, "button 1A should be checked");
			Assert.IsFalse(button1B.Checked, "button 1B should be unchecked");
		}

		[Test]
		[ExpectedException(typeof(HtmlInputRadioButtonTester.CannotUncheckException))]
		public void TestChecked_OnSetToFalse()
		{
			button1B.Checked = false;
		}

		[Test]
		public void TestNoValueAttribute()
		{
			HtmlInputRadioButtonTester button2A = new HtmlInputRadioButtonTester("button2A");
			button2A.Checked = true;
			postback.Click();

			WebAssert.TableContainsRow(formVars.TrimmedCells, new string[] {"group2", "on"});
		}

		[Test]
		[ExpectedException(typeof(ControlDisabledException))]
		public void TestDisabled()
		{
			HtmlInputRadioButtonTester button3A = new HtmlInputRadioButtonTester("button3A");
			button3A.Checked = true;
		}
	}
}
