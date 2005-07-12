#region Copyright (c) 2002-2005, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005, Brian Knowles, Jim Shore
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
' DEALINGS IN THE SOFTWARE.w
'
'*******************************************************************************************************************/
#endregion

using System;
using System.Text.RegularExpressions;
using System.Web;

namespace NUnit.Extensions.Asp.HtmlTester
{
	/// <summary>
	/// Tester for System.Web.UI.HtmlControls.HtmlButton
	/// </summary>
	public class HtmlButtonTester : HtmlControlTester
	{
		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlButtonTester(string htmlId) : base(htmlId)
		{
		}

		/// <summary>
		/// Create a tester for a server-side HTML control.  Use this constructor
		/// when the HTML tag you are testing has the "runat='server'" attribute.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.  Use "CurrentWebForm" if you're not sure; it will
		/// probably work.)</param>
		public HtmlButtonTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// Click the button.
		/// </summary>
		public void Click()
		{
			if (Disabled) throw new ControlDisabledException(this);

			string name = Tag.OptionalAttribute("name");
			if (name != null) Form.Variables.Add(name, Tag.Attribute("value"));
			Form.Submit();
		}

		/// <summary>
		/// Get the text on the button
		/// </summary>
		public virtual string Text
		{
			get
			{
				return InnerHtml;
			}
		}
	}
}
