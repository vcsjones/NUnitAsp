#region Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp
{
	/// <summary>
	/// <p>Base class for all NUnitAsp testers.  To create your own tester 
	/// classes, you should usually extend <see cref="AspTester.AspControlTester"/>
	/// or <see cref="HtmlTester.HtmlControlTester"/> instead.</p>
	///
	/// <p>Not intended for third-party use.  The API for this class will change 
	/// in future releases.  Currently, this class assumes that every tester
	/// corresponds to exactly one tag in the web page.  That may change in the
	/// future.</p>
	/// </summary>
	public abstract class Tester
	{
		/// <summary>
		/// The ASP.NET ID of the thing being tested.  It corresponds to the
		/// ID in the ASP.NET source code.
		/// </summary>
		public abstract string AspId
		{
			get;
		}

		/// <summary>
		/// The HTML tag we're testing.
		/// </summary>
		protected virtual HtmlTag Tag
		{
			get
			{
				return new HtmlTag(Browser, HtmlId, this);
			}
		}

		/// <summary>
		/// Returns true if the control is visible on the current page.
		/// </summary>
		public virtual bool Visible
		{
			get
			{
				return Tag.Visible;
			}
		}

		/// <summary>
		/// The HTML ID of the tag being tested.  It corresponds to the
		/// ID of the HTML tag rendered by the server.  It's useful for looking at 
		/// raw HTML while debugging.
		/// </summary>
		public abstract string HtmlId
		{
			get;
		}

		/// <summary>
		/// Returns the HTML ID of a child control.  Useful when implementing
		/// testers for container controls that do HTML ID mangling.  This method
		/// is very likely to change in a future release.
		/// </summary>
		protected internal abstract string GetChildElementHtmlId(string aspId);

		/// <summary>
		/// Post this page to the server.  (That is, the page that contains the thing being tested.)
		/// </summary>
		protected internal abstract void Submit();

		/// <summary>
		/// A human-readable description of the location of the control.
		/// </summary>
		public abstract string Description
		{
			get;
		}

		/// <summary>
		/// The browser instance used to load the page containing the thing being tested.
		/// </summary>
		protected internal abstract HttpClient Browser
		{
			get;
		}

		/// <summary>
		/// The tester for the form containing the thing being tested.
		/// </summary>
		protected internal abstract WebFormTester Form
		{
			get;
		}

		public class AttributeMissingException : ApplicationException
		{
			internal AttributeMissingException(string name, string containerDescription) : base("Expected attribute '" + name + "' on " + containerDescription)
			{
			}
		}
	}
}
