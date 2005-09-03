#region Copyright (c) 2005 James Shore
/********************************************************************************************************************
'
' Copyright (c) 2005, James Shore
' Created by Ben Monro.  Copyright assigned to Brian Knowles and Jim Shore on SourceForge "Patches"
' tracker, item #1184020, 15 April 2005.
' Includes ideas by Peter Jaffe.  Copyright assigned to NUnitAsp project on Jim Shore on SourceForge
' "Patches" tracker, item #1055819, 31 August 2005.
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
using System.Web.UI.WebControls;

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Tester for System.Web.UI.WebControls.Repeater.  Generally, you don't
	/// test through this tester.  Instead, you use the <see cref="GetItem"/>
	/// method to use as a container for testing controls that are inside the
	/// repeater.  The <see cref="ItemCount"/> method may also be useful.
	/// 
	/// <example>
	/// This example demonstrates how to test a button that's in the third
	/// item of a repeater:
	/// 
	/// <code>
	/// RepeaterTester repeater = new RepeaterTester("repeater");
	/// ButtonTester button = new ButtonTester("button", repeater.GetItem(2));</code>
	/// </example>
	/// 
	/// </summary>
	public class RepeaterTester : AspControlTester
	{
		private bool hasHeaderTemplate;
		private bool hasSeparatorTemplate;
		private bool hasFooterTemplate;

		/// <summary>
		/// <p>Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.  Testers created with this constructor
		/// will test pages loaded by the <see cref="HttpClient.Default"/>
		/// HttpClient.  NOTE: This constructor assumes that the repeater being
		/// tested ONLY has ItemTemplates.  If it has a SeparatorTemplate or
		/// any other template, use a different constructor.</p>
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public RepeaterTester(string aspId) : base(aspId)
		{
		}

		/// <summary>
		/// <p>Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.  Testers created with this constructor
		/// will test pages loaded by the <see cref="HttpClient.Default"/>
		/// HttpClient.</p>
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="hasHeaderTemplate">"True" if the repeater being tested has
		/// a HeaderTemplate.</param>
		/// <param name="hasSeparatorTemplate">"True" if the repeater being tested
		/// has a SeparatorTemplate.</param>
		/// <param name="hasFooterTemplate">"True" if the repeater bein tested
		/// has a FooterTemplate.</param>
		public RepeaterTester(string aspId, bool hasHeaderTemplate, bool hasSeparatorTemplate, bool hasFooterTemplate) : base(aspId)
		{
			this.hasHeaderTemplate = hasHeaderTemplate;
			this.hasSeparatorTemplate = hasSeparatorTemplate;
			this.hasFooterTemplate = hasFooterTemplate;
		}

		/// <summary>
		/// Create a tester for a nested control.  Use this constructor when 
		/// the control you are testing is nested within another control,
		/// such as a DataGrid or UserControl.  You should also use this
		/// constructor when you're not using the 
		/// <see cref="HttpClient.Default"/> HttpClient.
		/// NOTE: This constructor assumes that the repeater being
		/// tested ONLY has ItemTemplates.  If it has a SeparatorTemplate or
		/// any other template, use a different constructor.
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
		public RepeaterTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// Create a tester for a nested control.  Use this constructor when 
		/// the control you are testing is nested within another control,
		/// such as a DataGrid or UserControl.  You should also use this
		/// constructor when you're not using the 
		/// <see cref="HttpClient.Default"/> HttpClient.
		/// NOTE: This constructor assumes that the repeater being
		/// tested ONLY has ItemTemplates.  If it has a SeparatorTemplate or
		/// any other template, use a different constructor.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.)</param>
		/// <param name="hasHeaderTemplate">"True" if the repeater being tested has
		/// a HeaderTemplate.</param>
		/// <param name="hasSeparatorTemplate">"True" if the repeater being tested
		/// has a SeparatorTemplate.</param>
		/// <param name="hasFooterTemplate">"True" if the repeater bein tested
		/// has a FooterTemplate.</param>
		public RepeaterTester(string aspId, bool hasHeaderTemplate, bool hasSeparatorTemplate, bool hasFooterTemplate, Tester container) : base(aspId, container)
		{
			this.hasHeaderTemplate = hasHeaderTemplate;
			this.hasSeparatorTemplate = hasSeparatorTemplate;
			this.hasFooterTemplate = hasFooterTemplate;
		}

		/// <summary>
		/// Returns the HTML ID of a child control.  Useful when implementing
		/// testers for container controls that do HTML ID mangling.  This method
		/// is very likely to change in a future release.
		/// </summary>
		protected internal override string GetChildElementHtmlId(string aspId)
		{
			try
			{
				int itemNumber = int.Parse(aspId);
				return HtmlId + "__ctl" + ConvertItemNumberToId(itemNumber);
			}
			catch (FormatException)
			{
				throw new ContainerMustBeItemException(aspId, this);
			}
		}

		private int ConvertItemNumberToId(int itemNumber)
		{
			int id = itemNumber;
			if (hasSeparatorTemplate) id *= 2;
			if (hasHeaderTemplate) id++;
			return id;
		}

		/// <summary>
		/// Don't call this method--repeaters don't have a single tag.  Instead, all of the
		/// tags inside the repeater are rendered multiple times.
		/// </summary>
		protected override HtmlTagTester Tag
		{
			get
			{
				WebAssert.Fail("Repeaters don't have a single containing tag.");
				return null;  // unreachable; exception was thrown
			}
		}

		/// <summary>
		/// Returns true if the control is visible on the current page.  There's no way
		/// to distinguish between a repeater that is "visible" but has zero items and
		/// a repeater that is not visible, so if the repeater has zero items, this method
		/// will return false.  Similarly, if the repeater doesn't contain any server controls
		/// (controls with "runat='server'"), it will also appear to be invisible.
		/// </summary>
		public override bool Visible
		{
			get
			{
				return ItemCount > 0;
			}
		}

		/// <summary>
		/// The number of items in the repeater.  There's no way to distinguish between
		/// a repeater that is "visible" but has zero items and a repeater that is
		/// not visible, so if the repeater is invisible, this method will return zero
		/// items.  Similarly, if the repeater doesn't contain any server controls
		/// (controls with "runat='server'"), it will also appear to have zero items.
		/// </summary>
		public int ItemCount
		{
			get
			{
				HtmlTagTester lastChild = new HtmlTagTester(
					"//*[starts-with(@id, '" + HtmlId + "__ctl')][position() = last()]", 
					"last child of " + Description
				);
				if (!lastChild.Visible) return 0;

				string childId = lastChild.Attribute("id");
				Match m = Regex.Match(childId, this.HtmlId + @"__ctl(\d+)");
				int id = Convert.ToInt32(m.Groups[1].Captures[0].Value) + 1;

				return ConvertIdToItemNumber(id);
			}
		}

		private int ConvertIdToItemNumber(int id)
		{
			int itemCount = id;
			if (hasHeaderTemplate) itemCount--;
			if (hasSeparatorTemplate) itemCount = (itemCount + 1) / 2;

			return itemCount;
		}

		/// <summary>
		/// Returns the requested RepeaterItemTester.  This method does NOT check to see if
		/// the repeater exists or if the item number is legal on the current page.  
		/// </summary>
		/// <param name="itemNum">The item to get (zero-based)</param>
		/// <returns>A tester for the requested item</returns>
		public RepeaterItemTester GetItem(int itemNum)
		{
			return new RepeaterItemTester(itemNum, this);
		}

		/// <summary>
		/// Tester for System.Web.UI.WebControls.RepeaterItem.  Use this tester as a container
		/// for other testers.
		/// 
		/// <example>
		/// This example demonstrates how to test a button that's in the third
		/// item of a repeater:
		/// 
		/// <code>
		/// RepeaterTester repeater = new RepeaterTester("repeater");
		/// ButtonTester button = new ButtonTester("button", repeater.GetItem(2));</code>
		/// </example>
		/// </summary>
		public class RepeaterItemTester : AspControlTester
		{
			/// <summary>
			/// Create a tester for a specific item in a repeater.
			/// </summary>
			/// <param name="itemNum">The item number to test (zero-based).</param>
			/// <param name="container">The repeater tester this item is contained
			/// within.</param>
			public RepeaterItemTester(int itemNum, RepeaterTester container) : base(itemNum.ToString(), container)
			{
			}

			/// <summary>
			/// Don't call this method--repeaters aren't rendered as tags.
			/// They're just naming containers for their contents.
			/// </summary>
			protected override HtmlTagTester Tag
			{
				get
				{
					WebAssert.Fail("RepeaterItems aren't rendered and have no tag.");
					return null;  // unreachable; exception was thrown
				}
			}

			protected internal override string GetChildElementHtmlId(string aspId)
			{
				return HtmlId + "_" + aspId;
			}
		}
  
		/// <summary>
		/// Exception: The container of the control being tested was a RepeaterTester, but
		/// it should be a RepeaterItemTester.  Change "new MyTester("foo", repeater)" to 
		/// "new MyTester("foo", repeater.GetItem(itemNum))".
		/// </summary>
		public class ContainerMustBeItemException : ApplicationException 
		{
			internal ContainerMustBeItemException(string aspId, RepeaterTester repeater) 
				: base(GetMessage(aspId, repeater))
			{
			}

			private static string GetMessage(string aspId, RepeaterTester repeater) 
			{
				return String.Format(
					"Tester '{0}' has RepeaterTester '{1}' as its container. That isn't allowed. "
					+ "It should be a RepeaterTester.RepeaterItemTester. When constructing {0}, pass '{1}.GetItem(#)' "
					+ "as the container argument.",
					aspId, repeater.AspId);
			}
		}
	}
}