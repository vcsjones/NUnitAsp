#region Copyright (c) 2003, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2003, Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Base class for list testers.
	/// </summary>
	public abstract class ListControlTester : AspControlTester
	{
		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to link to.</param>
		/// <param name="container">The control that contains the control to link to</param>
		public ListControlTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// The items in the list.
		/// </summary>
		public ListItemCollectionTester Items 
		{
			get 
			{
				return new ListItemCollectionTester(this, OptionList);
			}
		}

		/// <summary>
		/// The currently-selected item in the list.
		/// </summary>
		public ListItemTester SelectedItem 
		{
			get 
			{
				ListItemCollectionTester items = Items;
				if (Items != null)
				{
					return Items[SelectedIndex];
				} 
				else 
				{
					return null;
				}
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
				int i = 0;
				int selected = -1;
				foreach (ListItemTester item in Items)
				{
					if (item.Selected)
					{
						if (selected != -1) throw new MultipleSelectionException();
						selected = i;
					}
					i++;
				}
				if (selected == -1) throw new NoSelectionException();
				return selected;
			}
			set
			{
				ListItemCollectionTester items = Items;
				if ((value > items.Count - 1) || (value < 0))
				{
					string message = string.Format(
						"Tried to set index of '{0}', exceeding maximum index of {1} (or minimum index of 0), in {2}", 
						value, items.Count - 1, HtmlIdAndDescription);
					throw new IllegalInputException(message);
				} 

				SetListSelection(items[value], true);
			}
		}

		protected void SetListSelection(ListItemTester item, bool selected)
		{
			string name = Tag.Attribute("name");
			foreach (XmlElement option in OptionList)
			{
				RemoveInputValue(option, name);
			}
			if (selected) EnterInputValue(item.Element, name, item.Value);
			OptionalPostBack(Tag.OptionalAttribute("onchange"));
		}

		protected void ToggleItemSelection(ListItemTester item, bool selected)
		{
			if (selected) 
			{
				EnterInputValue(item.Element, Tag.Attribute("name"), item.Value);
			}
			else
			{
				RemoveInputValue(item.Element, Tag.Attribute("name"));
			}
			OptionalPostBack(Tag.OptionalAttribute("onchange"));
		}

		protected internal virtual void ChangeItemSelectState(ListItemTester item, bool selected)
		{
			SetListSelection(item, selected);
		}

		private XmlNodeList OptionList
		{
			get
			{
				return Element.SelectNodes("option");
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

		/// <summary>
		/// The test asked a list what item was selected when multiple items were selected.
		/// Modify the test to look at the Selected property of individual list items, or
		/// fix the production code so that only one item is selected.
		/// </summary>
		public class MultipleSelectionException : ApplicationException
		{
			internal MultipleSelectionException() : base("Multiple list items are selected.")
			{
			}
		}
	}
}
