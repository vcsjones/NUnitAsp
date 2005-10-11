#region Copyright (c) 2003-2004, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2003-2004, Brian Knowles, Jim Shore
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
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.UI.WebControls;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Base class for list testers.
	/// </summary>
	public abstract class ListControlTester : AspControlTester
	{
		/// <summary>
		/// <p>Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.  Testers created with this constructor
		/// will test pages loaded by the <see cref="HttpClient.Default"/>
		/// HttpClient.</p>
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public ListControlTester(string aspId) : base(aspId)
		{
		}

		/// <summary>
		/// Create a tester for a nested control.  Use this constructor when 
		/// the control you are testing is nested within another control,
		/// such as a DataGrid or UserControl.  You should also use this
		/// constructor when you're not using the 
		/// <see cref="HttpClient.Default"/> HttpClient.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.)</param>
		/// 
		/// <example>
		/// This example demonstrates how to test a label that's inside
		/// of a user control:
		/// 
		/// <code>
		/// UserControlTester user1 = new UserControlTester("user1");
		/// LabelTester label = new LabelTester("label", user1);</code>
		/// </example>
		/// 
		/// <example>This example demonstrates how to use an HttpClient
		/// other than <see cref="HttpClient.Default"/>:
		/// 
		/// <code>
		/// HttpClient myHttpClient = new HttpClient();
		/// WebForm currentWebForm = new WebForm(myHttpClient);
		/// LabelTester myTester = new LabelTester("id", currentWebForm);</code>
		/// </example>
		public ListControlTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// A tester for the underlying HTML tag used to implement this ASP.NET control.
		/// </summary>
		public HtmlSelectTester ListTag
		{
			get
			{
				return new HtmlSelectTester(HtmlId);
			}
		}

		/// <summary>
		/// The items in the list.
		/// </summary>
		public ListItemCollectionTester Items 
		{
			get 
			{
				AssertVisible();
				return ListTag.Items;
			}
		}

		/// <summary>
		/// The currently-selected item in the list.
		/// </summary>
		public ListItemTester SelectedItem 
		{
			get 
			{
				return ListTag.Items[SelectedIndex];
			}
		}

		/// <summary>
		/// The index of the currently-selected item in the list.  If this is changed and 
		/// auto post-back is turned on, the form will be submitted.
		/// </summary>
		public int SelectedIndex
		{
			get
			{
				int selected = ListTag.SelectedIndex;
				if (selected == -1) throw new NoSelectionException();
				return selected;
			}
			set
			{
				ListTag.SelectedIndex = value;
			}
		}

		/// <summary>
		/// The value of the currently-selected item in the list.  If this is changed and 
		/// auto post-back is turned on, the form will be submitted.
		/// </summary>
		public String SelectedValue
		{
			get
			{
				return SelectedItem.Value;
			}
			set
			{
				ListItemTester itemToSelect = Items.FindByValue(value);
				if (itemToSelect == null)
				{
					string message = string.Format(
						"Tried to select item with value of '{0}', which does not exist in the items of {1}", 
						value, HtmlIdAndDescription);
					throw new IllegalInputException(message);
				}
				itemToSelect.Selected = true;
			}
		}

		/// <summary>
		/// The index of the list was set to a value that doesn't correspond to a
		/// list item.  Fix the test so that it sets the value correctly, or fix the production
		/// code so that it generates the correct number of list items.
		/// </summary>
		public class IllegalInputException : ApplicationException
		{
			internal IllegalInputException(string message) : base(message)
			{
			}
		}

		/// <summary>
		/// The test asked a list what item was selected when no items were selected.
		/// Fix the test so that it doesn't ask the question, or fix the production code so
		/// that a list item is selected.
		/// </summary>
		public class NoSelectionException : ApplicationException
		{
			internal NoSelectionException() : base("None of the list items have been selected.")
			{
			}
		}
	}
}

