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
using NUnit.Extensions.Asp.AspTester;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class HtmlSelectTest : NUnitAspTestCase
	{
		private HtmlSelectTester def;
		private HtmlSelectTester nonDefault;
		private HtmlSelectTester singleSelect;
		private LinkButtonTester submit;

		protected override void SetUp()
		{
			def = new HtmlSelectTester("default");
			nonDefault = new HtmlSelectTester("nonDefault");
			singleSelect = new HtmlSelectTester("singleSelect");
			submit = new LinkButtonTester("submit");

			Browser.GetPage(BaseUrl + "/HtmlTester/HtmlSelectTestPage.aspx");
		}

		[Test]
		public void TestDefaultProperties()
		{
			Assert.AreEqual(-1, def.SelectedIndex, "selected index");
			Assert.AreEqual(1, def.Size, "size");
			Assert.IsFalse(def.Multiple, "multiple should be false");
			WebAssert.AreEqual(new string[] {}, def.RenderedItems, "items");
		}

		[Test]
		public void TestNonDefaultProperties()
		{
			Assert.AreEqual(2, nonDefault.SelectedIndex, "selected index");
			Assert.AreEqual(3, nonDefault.Size, "size");
			Assert.IsTrue(nonDefault.Multiple, "multiple should be true");
			WebAssert.AreEqual(new string[] {"one ", "two ", "three ", "four ", "five "}, nonDefault.RenderedItems, "items");
		}
			
		[Test]
		public void TestItems()
		{
			Assert.AreEqual(0, def.Items.Count, "default");
			Assert.AreEqual(5, nonDefault.Items.Count, "non-default");

			Assert.AreEqual("two ", nonDefault.Items[1].RenderedText);
		}

		[Test]
		public void TestSelectedIndex()
		{
			nonDefault.SelectedIndex = 4;
			submit.Click();
			AssertItemsSelected(nonDefault, false, false, false, false, true);
		}

		[Test]
		public void TestSelectedIndex_WhenNoSelection()
		{
			Assert.AreEqual(-1, def.SelectedIndex);
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException), "Can't get SelectedIndex when multiple items are selected; use Items[##].Selected instead")]
		public void TestSelectedIndex_WhenMultipleSelections()
		{
			nonDefault.Items[4].Selected = true;
			submit.Click();
			int unused = nonDefault.SelectedIndex;
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException), "Can't call SelectedIndex with index less than zero (was -1)")]
		public void TestSelectedIndex_WhenSettingBelowZero()
		{
			nonDefault.SelectedIndex = -1;
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException), "Tried to select item #10, but largest index is 4")]
		public void TestSelectedIndex_WhenSettingAboveMaximum()
		{
			nonDefault.SelectedIndex = 10;
		}

		[Test]
		public void TestSelection_WithMultipleSelect()
		{
			nonDefault.Items[4].Selected = true;
			submit.Click();
			AssertItemsSelected(nonDefault, false, false, true, false, true);
		}

		[Test]
		public void TestSelection_WithSingleSelect()
		{
			singleSelect.Items[4].Selected = true;
			submit.Click();
			AssertItemsSelected(singleSelect, false, false, false, false, true);
		}

		[Test]
		public void TestSelection_WhenDeselectingMultiSelect()
		{
			nonDefault.Items[2].Selected = false;
			submit.Click();
			AssertItemsSelected(nonDefault, false, false, false, false, false);
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException), "Can't deselect items unless list box is multi-select")]
		public void TestSelection_WhenDeselectingSingleSelect()
		{
			singleSelect.Items[2].Selected = false;
		}

		private void AssertItemsSelected(HtmlSelectTester tester, bool one, bool two, bool three, bool four, bool five)
		{
			Assert.AreEqual(one, tester.Items[0].Selected, "one in " + tester.Description);
			Assert.AreEqual(two, tester.Items[1].Selected, "two in " + tester.Description);
			Assert.AreEqual(three, tester.Items[2].Selected, "three in " + tester.Description);
			Assert.AreEqual(four, tester.Items[3].Selected, "four in " + tester.Description);
			Assert.AreEqual(five, tester.Items[4].Selected, "five in " + tester.Description);
		}
	}
}
