#region Copyright (c) 2003, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Shore
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
using System.Web.UI.WebControls;
using NUnit.Framework;
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test.AspTester
{
	public class DropDownListTest : NUnitAspTestCase
	{
		private DropDownListTester list;
		private DropDownListTester emptyList;
		private DropDownListTester disabledList;
		private LinkButtonTester submit;
		private LinkButtonTester clearSelection;
		private CheckBoxTester autoPostBack;
		private LabelTester indexChanged;

		protected override void SetUp()
		{
			base.SetUp();
			list = new DropDownListTester("list", CurrentWebForm);
			emptyList = new DropDownListTester("emptyList", CurrentWebForm);
			disabledList = new DropDownListTester("disabledList", CurrentWebForm);
			submit = new LinkButtonTester("submit", CurrentWebForm);
			clearSelection = new LinkButtonTester("clearSelection", CurrentWebForm);
			autoPostBack = new CheckBoxTester("auto", CurrentWebForm);
			indexChanged = new LabelTester("indexChanged", CurrentWebForm);

			Browser.GetPage(BaseUrl + "/AspTester/DropDownListTestPage.aspx");
		}

		public void TestEnabled_True()
		{
			AssertEquals("enabled", true, list.Enabled);
		}

		public void TestEnabled_False()
		{
			AssertEquals("enabled", false, disabledList.Enabled);
		}

		public void TestSelectedIndex()
		{
			AssertEquals("selected index", 1, list.SelectedIndex);
		}

		public void TestSelectedItem()
		{
			ListItem item = list.SelectedItem;
			AssertEquals("text", "two", item.Text);
			AssertEquals("value", "2", item.Value);
		}

		public void TestItems()
		{
			ListItemCollection expectedItems = new ListItemCollection();
			ListItem item1 = new ListItem("one", "1");
			ListItem item2 = new ListItem("two", "2");
			ListItem item3 = new ListItem("three", "3");
			expectedItems.Add(item1);
			expectedItems.Add(item2);
			expectedItems.Add(item3);

			ListItemCollection actualItems = list.Items;
			AssertEquals("# of items", 3, actualItems.Count);
			for (int i = 0; i < actualItems.Count; i++)
			{
				AssertEquals("Item #" + i, expectedItems[i], actualItems[i]);
			}
		}

		public void TestSetSelectedIndex()
		{
			AssertEquals(1, list.SelectedIndex);
			list.SelectedIndex = 2;
			AssertEquals("No", indexChanged.Text);
			AssertEquals(1, list.SelectedIndex);
			submit.Click();
			AssertEquals("Yes", indexChanged.Text);
			AssertEquals(2, list.SelectedIndex);
		}

        [ExpectedException(typeof(ControlDisabledException))]
        public void TestSetSelectedIndex_WhenDisabled()
		{
			disabledList.SelectedIndex = 1;
		}

		public void TestImmediatePostBack()
		{
			autoPostBack.Checked = true;
			submit.Click();
			AssertEquals("selected index (before modification)", 1, list.SelectedIndex);
			list.SelectedIndex = 2;
			AssertEquals("Yes", indexChanged.Text);
			AssertEquals("selected index (after modification)", 2, list.SelectedIndex);
		}

		public void TestSetSelectedIndexOutOfRange()
		{
			try
			{
				list.SelectedIndex = 5;
				Fail("Expected IllegalInputException");
			}
			catch (DropDownListTester.IllegalInputException)
			{
				// expected
			}
		}

		public void TestServerSideClearSelection()
		{
			try
			{
				clearSelection.Click();
				int unused = list.SelectedIndex;
				Fail("Expected NoSelectionException");
			}
			catch (DropDownListTester.NoSelectionException)
			{
				// expected
			}
		}

		public void TestSelectedIndex_WhenEmptyList()
		{
			try
			{
				int unused = emptyList.SelectedIndex;
				Fail("Expected NoSelectionException");
			}
			catch (DropDownListTester.NoSelectionException)
			{
				// expected
			}
		}

		public void TestSelectedItem_WhenEmptyList()
		{
			try
			{
				ListItem unused = emptyList.SelectedItem;
				Fail("Expected NoSelectionException");
			}
			catch (DropDownListTester.NoSelectionException)
			{
				// expected
			}
		}

		public void TestItems_WhenEmptyList()
		{
			AssertEquals("# of items", 0, emptyList.Items.Count);
		}

		public void TestSetSelectedIndex_WhenEmptyList()
		{
			try
			{
				emptyList.SelectedIndex = 0;
				Fail("Expected IllegalInputException");
			}
			catch (DropDownListTester.IllegalInputException)
			{
				// expected
			}
		}

		public void TestSelectedIndexChangedEvent_WhenItemAddedToList()
		{
			ButtonTester addItem = new ButtonTester("add", CurrentWebForm);
			
			autoPostBack.Checked = true;
			submit.Click();
			addItem.Click();
			AssertEquals("No", indexChanged.Text);
		}
	}
}
