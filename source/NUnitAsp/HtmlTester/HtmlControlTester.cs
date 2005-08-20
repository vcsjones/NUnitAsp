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
		public HtmlControlTester(string aspId, Tester container) : base(aspId, container)
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
		[Obsolete("'runAtServer' parameter is no longer required.  Use one of the other constructors instead.")]
		public HtmlControlTester(string aspId, Tester container, bool runAtServer) :
			this(aspId, container)
		{
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
		/// Returns the value of the attribute on this tag or null if the attribute isn't present.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <returns>The value of the attribute or null if no such attribute</returns>
		public string OptionalAttribute(string name)
		{
			return Tag.OptionalAttribute(name);
		}

		/// <summary>
		/// Returns an attribute as an integer, or -1 if the attribute isn't present.  Throws
		/// an exception if the attribute isn't an integer.
		/// </summary>
		public int IntegerAttributeWithNegOneDefault(string name)
		{
			string attribute = Tag.OptionalAttribute(name);
			if (attribute == null) return -1;
			else return int.Parse(attribute);
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
		/// The HTML inside the tag being tested.
		/// </summary>
		public string InnerHtml
		{
			get
			{
				return Tag.Body;
			}
		}
	}
}
