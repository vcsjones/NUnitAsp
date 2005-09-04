#region Copyright (c) 2005 James Shore
/********************************************************************************************************************
'
' Copyright (c) 2005, James Shore
' Created by Ben Monro.  Copyright assigned to Brian Knowles and Jim Shore on SourceForge "Patches"
' tracker, item #1184020, 15 April 2005.
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
	public class RepeaterTest : NUnitAspTestCase
	{
		private RepeaterTester repeater1;
		private RepeaterTester repeater2;
		private RepeaterTester repeater3;

		protected override void SetUp()
		{
			base.SetUp();

			repeater1 = new RepeaterTester("Repeater1", CurrentWebForm);
			repeater2 = new RepeaterTester("Repeater2", CurrentWebForm);
			repeater3 = new RepeaterTester("Repeater3", CurrentWebForm);

			Browser.GetPage(BaseUrl + "/AspTester/RepeaterTestPage.aspx");
		}

		[Test]
		public void TestTwoRepeaters()
		{
			Assert.AreEqual(2, repeater1.ItemCount);
			Assert.AreEqual(1, repeater2.ItemCount);
		}

		[Test]
		public void TestEmptyRepeater()
		{
			Assert.AreEqual(0, repeater3.ItemCount);
		}

		[Test]
		public void TestVisibility()
		{
			WebAssert.Visible(repeater1);
			WebAssert.Visible(repeater2);
			WebAssert.NotVisible(repeater3);
		}

		[Test]
		public void TestLabelInRepeaterRow()
		{
			LabelTester label1 = new LabelTester("Label1", repeater2.Item(0));
			Assert.AreEqual("Go Suns!", label1.Text);
		}

		[Test]
		public void TestNestedRepeater()
		{
			RepeaterTester innerRepeater1 = new RepeaterTester("innerRepeater1", repeater1.Item(0));
			RepeaterTester innerRepeater2 = new RepeaterTester("innerRepeater1", repeater1.Item(1));

			Assert.AreEqual(2, innerRepeater1.ItemCount);
			Assert.AreEqual(4, innerRepeater2.ItemCount);
		}

		[Test]
		public void TestButtonClickInRepeater()
		{
			ButtonTester button1 = new ButtonTester("Button1", CurrentWebForm);
			LabelTester label1 = new LabelTester("Label1", CurrentWebForm);

			button1.Click();

			ButtonTester innerButton1 = new ButtonTester("btnInner", repeater3.Item(0));
			ButtonTester innerButton2 = new ButtonTester("btnInner", repeater3.Item(1));

			innerButton1.Click();
			Assert.AreEqual("Thing 1", label1.Text);

			innerButton2.Click();
			Assert.AreEqual("Thing 2", label1.Text);
		}

		[Test]
		public void TestHeaderTemplate()
		{
			RepeaterTester header = new RepeaterTester("headerRepeater", true, false, false);
			AssertFiveThings(header, "headerLabel");
			Assert.AreEqual("header", new LabelTester("nestedHeaderLabel", header.Header).Text);
		}

		[Test]
		public void TestSeparatorTemplate()
		{
			RepeaterTester separator = new RepeaterTester("separatorRepeater", false, true, false);
			AssertFiveThings(separator, "separatorLabel");
			AssertFourSeparators(separator, "nestedSeparatorLabel");
		}

		[Test]
		public void TestFooterTemplate()
		{
			RepeaterTester footer = new RepeaterTester("footerRepeater", false, false, true);
			AssertFiveThings(footer, "footerLabel");
			Assert.AreEqual("footer", new LabelTester("nestedFooterLabel", footer.Footer).Text);
		}

		[Test]
		public void TestAlternatingItemTemplate()
		{
			RepeaterTester alternating = new RepeaterTester("alternatingRepeater");
			AssertFiveThings(alternating, "alternatingLabel");
		}

		[Test]
		public void TestAllTemplates()
		{
			RepeaterTester all = new RepeaterTester("allRepeater", true, true, true);
			AssertFiveThings(all, "allLabel");
			AssertFourSeparators(all, "allSeparatorTemplateLabel");
			Assert.AreEqual("header", new LabelTester("allHeaderTemplateLabel", all.Header).Text);
			Assert.AreEqual("footer", new LabelTester("allFooterTemplateLabel", all.Footer).Text);
		}

		[Test]
		[ExpectedException(typeof(RepeaterTester.ContainerMustBeItemException))]
		public void TestContainerMustBeItemException()
		{
			Assert.AreEqual("foo", new LabelTester("foo", new RepeaterTester("bar")).Text);
		}

		[Test]
		public void TestDescriptions()
		{
			RepeaterTester all = new RepeaterTester("allRepeater", true, true, true);

			AssertDescription("RepeaterTester 'allRepeater'", all);
			AssertDescription("HeaderTemplate in RepeaterTester 'allRepeater'", all.Header);
			AssertDescription("Item #3 in RepeaterTester 'allRepeater'", all.Item(2));
			AssertDescription("Separator #2 in RepeaterTester 'allRepeater'", all.Separator(1));
			AssertDescription("FooterTemplate in RepeaterTester 'allRepeater'", all.Footer);
		}

		private void AssertFiveThings(RepeaterTester tester, string labelId)
		{
			Assert.AreEqual(5, tester.ItemCount);
			Assert.AreEqual("one", new LabelTester(labelId, tester.Item(0)).Text);
			Assert.AreEqual("two", new LabelTester(labelId, tester.Item(1)).Text);
			Assert.AreEqual("three", new LabelTester(labelId, tester.Item(2)).Text);
			Assert.AreEqual("four", new LabelTester(labelId, tester.Item(3)).Text);
			Assert.AreEqual("five", new LabelTester(labelId, tester.Item(4)).Text);
		}

		private void AssertFourSeparators(RepeaterTester tester, string separatorLabelId)
		{
			Assert.AreEqual(" | ", new LabelTester(separatorLabelId, tester.Separator(0)).Text);
			Assert.AreEqual(" | ", new LabelTester(separatorLabelId, tester.Separator(1)).Text);
			Assert.AreEqual(" | ", new LabelTester(separatorLabelId, tester.Separator(2)).Text);
			Assert.AreEqual(" | ", new LabelTester(separatorLabelId, tester.Separator(3)).Text);
		}
	}
}
