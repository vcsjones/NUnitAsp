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
using NUnit.Extensions.Asp.AspTester;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class HtmlInputButtonTest : NUnitAspTestCase
	{
		private HtmlInputButtonTester button;
		private DataGridTester formVars;

		protected override void SetUp()
		{
			base.SetUp();
			button = new HtmlInputButtonTester("button");
			formVars = new DataGridTester("formVars", CurrentWebForm);
			Browser.GetPage(BaseUrl + "HtmlTester/HtmlInputButtonTestPage.aspx");
		}

		[Test]
		public void TestClick()
		{
			button.Click();
			Assert.AreEqual("RedirectionTarget", CurrentWebForm.AspId);
			WebAssert.TableContainsRow(formVars.TrimmedCells, "button", "This is a button.");
		}

		[Test]
		public void TestText()
		{
			Assert.AreEqual("This is a button.", button.Text);
		}
	}
}
