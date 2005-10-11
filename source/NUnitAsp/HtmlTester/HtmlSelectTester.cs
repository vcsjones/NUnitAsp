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
using System.Xml;

namespace NUnit.Extensions.Asp.HtmlTester
{
	/// <summary>
	/// Tester for System.Web.UI.HtmlControls.HtmlSelect
	/// </summary>
	public class HtmlSelectTester : HtmlControlTester
	{
		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlSelectTester(string htmlId) : base(htmlId)
	{
	}

		/// <summary>
		/// Create a tester for a server-side HTML control or a tag that's on a 
		/// page with multiple forms.  Use this constructor when the HTML tag you
		/// are testing has the "runat='server'" attribute.
		/// Also use this tester when using the non-default webform or HttpClient.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.)  If testing a page with multiple forms or a non-default
		/// HttpClient, pass in the WebFormTester for the form this tag is within.</param>
		public HtmlSelectTester(string aspId, Tester container) : base(aspId, container)
	{
	}

		/// <summary>
		/// Create a tester for an HTML tag using an XPath description.
		/// </summary>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A human-readable description of this tag (for error reporting).</param>
		public HtmlSelectTester(string xpath, string description) : base(xpath, description)
	{
	}

		/// <summary>
		/// Create a tester for an HTML tag that's on a page with multiple forms using
		/// an XPath description.
		/// </summary>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A human-readable description of this tag (for error reporting).</param>
		/// <param name="container">A tester for the control's container.  A WebFormTester
		/// will usually be most appropriate.</param>
		public HtmlSelectTester(string xpath, string description, Tester container) : base(xpath, description, container)
		{
		}

		/// <summary>
		/// The index of the currently-selected option
		/// </summary>
		public int SelectedIndex
		{
			get
			{
				int result = -1;
				HtmlTagTester[] items = OptionTags;
				for (int i = 0; i < items.Length; i++)
				{
					HtmlTagTester item = items[i];
					if (item.HasAttribute("selected")) 
					{
						WebAssert.True(result == -1, "Can't get SelectedIndex when multiple items are selected; use Items[##].Selected instead");
						result = i;
					}
				}
				return result;
			}
			set
			{
				int maxIndex = Items.Count - 1;
				WebAssert.True(value >= 0, "Can't call SelectedIndex with index less than zero (was " + value + ")");
				WebAssert.True(value <= maxIndex, "Tried to select item #" + value + ", but largest index is " + maxIndex);
				
				Form.Variables.RemoveAll(Attribute("name"));
				ChangeItemSelectState(Items[value], true);
			}
		}

		/// <summary>
		/// For internal use only.
		/// </summary>
		protected internal void ChangeItemSelectState(ListItemTester item, bool selected)
		{
			if (Disabled) throw new ControlDisabledException(this);

			string name = Attribute("name");
			if (!Multiple) Form.Variables.RemoveAll(name);
			if (selected)
			{
				Form.Variables.Add(name, item.Value);
			}
			else
			{
				WebAssert.True(Multiple, "Can't deselect items unless list box is multi-select");
				Form.Variables.Remove(name, item.Value);
			}
			Form.OptionalPostBack(Tag.OptionalAttribute("onchange"));
		}

		/// <summary>
		/// The &lt;option&gt; tags contained within this &lt;select&gt; tag.
		/// </summary>
		public HtmlTagTester[] OptionTags
		{
			get
			{
				return Children("option");
			}
		}

		/// <summary>
		/// The number of rows specified for the browser to display, or '1' if not specified.
		/// </summary>
		public int Size
		{
			get
			{
				string size = OptionalAttribute("size");
				if (size == null) return 1;
				else return int.Parse(Attribute("size"));
			}
		}

		/// <summary>
		/// Whether the 'multiple-select' attribute has been set.
		/// </summary>
		public bool Multiple
		{
			get
			{
				return HasAttribute("multiple");
			}
		}

		/// <summary>
		/// The text of all of the options for this list box, rendered as strings.
		/// </summary>
		public string[] RenderedItems
		{
			get
			{
				HtmlTagTester[] options = OptionTags;
				string[] result = new string[options.Length];

				for (int i = 0; i < options.Length; i++)
				{
					result[i] = options[i].RenderedInnerHtml;
				}
				return result;
			}
		}

		/// <summary>
		/// The options in this select tag, rendered as ListItemTesters in order to be consistent
		/// with ASP.NET.
		/// </summary>
		public ListItemCollectionTester Items
		{
			get
			{
				return new ListItemCollectionTester(OptionTags, this);
			}
		}
	}
}
