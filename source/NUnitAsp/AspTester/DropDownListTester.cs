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
	public class DropDownListTester : ControlTester
	{
		public DropDownListTester(string aspId, Control container) : base(aspId, container)
		{
		}

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

		public class IllegalInputException : ApplicationException
		{
			internal IllegalInputException(string message) : base(message)
			{
			}
		}

		public class NoSelectionException : ApplicationException
		{
			internal NoSelectionException() : base("None of the list items have been selected.")
			{
			}
		}
	}
}
