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
	public class HtmlInputImageTest : NUnitAspTestCase
	{
		private HtmlInputImageTester image;
		private DataGridTester formVars;

		protected override void SetUp()
		{
			base.SetUp();
			image = new HtmlInputImageTester("image");
			formVars = new DataGridTester("formVars", CurrentWebForm);
			Browser.GetPage(BaseUrl + "HtmlTester/HtmlInputImageTestPage.aspx");
		}

		[Test]
		public void TestClick()
		{
			image.Click();
			AssertRedirected();
			WebAssert.AreEqualIgnoringOrder
			(
				formVars.RenderedCells,
				new String[][]
				{
					new String[] {"image.x", "0"},
					new String[] {"image.y", "0"},
					new string[] {"image", "foo"}
				}
			);
		}

		[Test]
		public void TestClickWithParameters()
		{
			image.Click(42, 24);
			AssertRedirected();
			WebAssert.AreEqualIgnoringOrder
			(
				formVars.RenderedCells,
				new String[][] 
				{
					new String[] {"image.x", "42"},
					new String[] {"image.y", "24"},
					new string[] {"image", "foo"}
				}
			);
		}

		[Test]
		public void TestNoName()
		{
			HtmlInputImageTester noName = new HtmlInputImageTester("noName");
			noName.Click();
			WebAssert.AreEqualIgnoringOrder
			(
				formVars.RenderedCells,
				new String[][]
				{
					new String[] {"x", "0"},
					new String[] {"y", "0"},
				}
			);
		}

		[Test]
		[ExpectedException(typeof(ControlDisabledException))]
		public void TestDisabled()
		{
			HtmlInputImageTester disabled = new HtmlInputImageTester("disabled");
			disabled.Click();
		}
	}
}
