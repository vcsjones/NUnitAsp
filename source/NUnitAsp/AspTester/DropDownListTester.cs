/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Little
' Originally written by David Paxson.  Copyright assigned to Brian Knowles and Jim Little.
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

using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.UI.WebControls;

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Tester for System.Web.UI.WebControls.DropDownList
	/// </summary>
	public class DropDownListTester : ControlTester
	{
		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to link to.</param>
		/// <param name="container">The control that contains the control to link to</param>
		public DropDownListTester(string aspId, Control container) : base(aspId, container)
		{
		}

		/// <summary>
		/// The items in the list.
		/// </summary>
		public ListItemCollection Items 
		{
			get 
			{
				ListItemCollection items = new ListItemCollection();
				int i = 0;
				foreach (XmlNode option in OptionList)
				{
					ListItem item = new ListItem(option.InnerXml, option.Attributes["value"].Value);
					items.Add(item);
					i++;
				}
				return items;
			}
		}

		/// <summary>
		/// The currently-selected item in the list.
		/// </summary>
		public ListItem SelectedItem 
		{
			get 
			{
				ListItemCollection items = Items;
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
				foreach (XmlNode option in OptionList)
				{
					if (option.Attributes["selected"] != null)
					{
						return i;
					}
					i++;
				}
				throw new NoSelectionException();
			}
			set
			{
				ListItemCollection items = Items;
				if ((value > items.Count - 1) || (value < 0))
				{
					string message = string.Format("Tried to set index of '{0}', exceeding maximum index of {1} (or minimum index of 0), in {2}", value, items.Count - 1, HtmlIdAndDescription);
					throw new IllegalInputException(message);
				} 

				EnterInputValue(GetAttributeValue("name"), items[value].Value);
				OptionalPostBack(GetOptionalAttributeValue("onchange"));
			}
		}

		private XmlNodeList OptionList
		{
			get
			{
				return Element.SelectNodes("option");
			}
		}

		/// <summary>
		/// The index of the drop-down list was set to a value that doesn't correspond to a
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
		/// The test asked a drop-down list what item was selected when no items were selected.
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
