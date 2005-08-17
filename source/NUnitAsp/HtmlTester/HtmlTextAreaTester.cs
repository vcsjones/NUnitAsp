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

namespace NUnit.Extensions.Asp.HtmlTester
{
	/// <summary>
	/// Tester for System.Web.UI.HtmlControls.HtmlInputButton
	/// </summary>
	public class HtmlTextAreaTester : HtmlControlTester
	{
		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlTextAreaTester(string htmlId) : base(htmlId)
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
		public HtmlTextAreaTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// Return the width of the text area in columns, or -1 if undefined (default)
		/// </summary>
		public int Cols
		{
			get
			{
				return IntegerAttributeWithNegOneDefault("cols");
			}
		}

		/// <summary>
		/// Return the height of the text area in rows, or -1 if undefined (default).
		/// </summary>
		public int Rows
		{
			get
			{
				return IntegerAttributeWithNegOneDefault("rows");
			}
		}
	}
}
