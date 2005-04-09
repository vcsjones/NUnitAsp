#region Copyright (c) 2005, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2005, Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp.Test
{
	public class WebFormTest : NUnitAspTestCase
	{
		private static readonly string TestUrl = BaseUrl + "MultipleFormTestPage.aspx";

		private WebFormTester form1;
		
		protected override void SetUp()
		{
			form1 = new WebFormTester("MultipleFormTestPage_1", Browser);
		}
		
//		public void TestHiddenVariableParsing()
//		{
//			Browser.GetPage(TestUrl);
//
//			Console.WriteLine(form1.Variables);
//			Assert.IsTrue(form1.Variables.ContainsKey("one_tag"), "expected one_tag");
//			Assert.IsFalse(form1.Variables.ContainsKey("two_tag"), "didn't expect two_tag");
//		}
//
//		[Ignore("not ready")]
//		public void TestMultipleForms()
//		{
//			WebFormTester form1 = new WebFormTester("MultipleFormTestPage_1", Browser);
//			LinkButtonTester one = new LinkButtonTester("one", form1);
//			LabelTester submitted = new LabelTester("submitted", form1);
//
//			Browser.GetPage(TestUrl);
//
//			Assert.AreEqual("one", one.Text);
//			one.Click();
//			Assert.AreEqual("form 1", submitted.Text);
//		}

		[ExpectedException(typeof(WebAssertionException))]
		public void TestMultipleForms_WhenBadAspId()
		{
			WebFormTester form1 = new WebFormTester("badId", Browser);
			LinkButtonTester one = new LinkButtonTester("one", form1);

			Browser.GetPage(TestUrl);
			one.Click();
		}
	}
}
