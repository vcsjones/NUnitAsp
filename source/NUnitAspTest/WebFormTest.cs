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
	[TestFixture]
	public class WebFormTest : NUnitAspTestCase
	{
		private static readonly string TestUrl = BaseUrl + "WebFormTestPage.aspx";

		private WebFormTester form1;
		private WebFormTester form2;
		
		protected override void SetUp()
		{
			form1 = new WebFormTester("Form1", Browser);
			form2 = new WebFormTester("Form2", Browser);
			Browser.GetPage(TestUrl);
		}
		
		[Test]
		public void TestHiddenVariableParsing()
		{
			Console.WriteLine("form 1: " + form1.Variables);
			Console.WriteLine("form 2: " + form2.Variables);

			Assert.AreEqual("tag_one_value", form1.Variables.ValueOf("tag_one"), "tag one in form 1");
			Assert.IsFalse(form1.Variables.ContainsAny("tag_two"), "tag two shouldn't be in form 1");

			Assert.AreEqual("tag_two_value", form2.Variables.ValueOf("tag_two"), "tag two in form 2");
			Assert.IsFalse(form2.Variables.ContainsAny("tag_one"), "tag one shouldn't be in form 2");
		}

		[Test]
		public void TestNestedVariableParsing()
		{
			Assert.AreEqual("nested_tag_value", form1.Variables.ValueOf("nested_tag"));
		}

//		[Test]
//		[ExpectedException(typeof(WebAssertionException))]
//		public void TestCurrentWebForm_WhenMultipleForms()
//		{
//			FormVariables unused = CurrentWebForm.Variables;
//		}


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
