#region Copyright (c) 2002-2005 Brian Knowles, James Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005, Brian Knowles, James Shore
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
	/// Base class for all testers in HtmlTester namespace
	/// </summary>
	public abstract class HtmlControlTester : ControlTester
	{
		private bool runAtServer = false;   // note: delete me

		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlControlTester(string htmlId) : base(htmlId)
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
		public HtmlControlTester(string aspId, Tester container) : this(aspId, container, true)
		{
		}

		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  (In the page's ASP.NET
		/// source code, look for the tag that the control is nested in.  That's probably the
		/// control's container.  Use CurrentWebForm if the control is just nested in the form tag.)</param>
		/// <param name="runAtServer">Tells tester whether the control under test is running on the server side.</param>
		// NOTE::::: Mark me obsolete!
		public HtmlControlTester(string aspId, Tester container, bool runAtServer) :
			base(aspId, container)
		{
			this.runAtServer = runAtServer;
		}

		/// <summary>
		/// Returns the value of an attribute on this tag or throws an exception if the attribute
		/// isn't present.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <returns>The value of the attribute</returns>
		public string Attribute(string name) 
		{
			return Tag.Attribute(name);
		}

		/// <summary>
		/// Gets or sets a value indicating wheither the control is disabled.
		/// </summary>
		public bool Disabled
		{
			get
			{
				return IsDisabled;
			}
		}
	
		/// <summary>
		/// The HTML ID of the control being tested.  It corresponds to the
		/// ID of the HTML tag rendered by the server.  It's useful for looking at 
		/// raw HTML while debugging.
		/// </summary>
		public override string HtmlId
		{
			get
			{
				if (runAtServer)
				{
					return base.HtmlId;
				}
				else
				{
					return AspId;
				}
			}
		}

		/// <summary>
		/// The HTML inside the tag being tested.
		/// </summary>
		public string InnerHtml
		{
			get
			{
				return Tag.Body;
			}
		}

		/// <summary>
		/// Use with caution--<see cref="InnerHtml"/> is probably a better choice.  
		/// Provided for consistency with ASP.NET framework API.  This method is the same as
		/// InnerHtml, but with HTML entities (such as &amp;gt;) converted to
		/// characters (such as &gt;).
		/// </summary>
		public string InnerText
		{
			get
			{
				return System.Web.HttpUtility.HtmlDecode(Tag.Body);
			}
		}
	}
}
