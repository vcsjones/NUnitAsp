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
	/// Tester for System.Web.UI.HtmlControls.HtmlInputRadioButton
	/// </summary>
	public class HtmlInputRadioButtonTester : HtmlControlTester
	{
		#region Standard Constructors
		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlInputRadioButtonTester(string htmlId) : base(htmlId)
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
		public HtmlInputRadioButtonTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// Create a tester for an HTML tag using an XPath description.
		/// </summary>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A human-readable description of this tag (for error reporting).</param>
		public HtmlInputRadioButtonTester(string xpath, string description) : base(xpath, description)
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
		public HtmlInputRadioButtonTester(string xpath, string description, Tester container) : base(xpath, description, container)
		{
		}
		#endregion

		/// <summary>
		/// 'True' if the radio button is checked; 'false' if not.  Set this parameter to change
		/// the radio button value on post-back.  Only one radio button in the same group (that is,
		/// with the same 'name' attribute) may be checked at any given time.  If you set this
		/// radio button to true, any other radio buttons that are checked will be unchecked.
		/// </summary>
		public bool Checked
		{
			get
			{
				return HasAttribute("checked");
			}
			set
			{
				if (!value) throw new CannotUncheckException();
				if (Disabled) throw new ControlDisabledException(this);

				string valueAttribute = OptionalAttribute("value");
				if (valueAttribute == null) valueAttribute = "on";
				Form.Variables.ReplaceAll(Attribute("name"), valueAttribute);
			}
		}

		/// <summary>
		/// Test attempted to set radio button's Checked property to false,
		/// but radio buttons cannot be unchecked directly.  Check another 
		/// radio button in the same group instead.
		/// </summary>
		public class CannotUncheckException : InvalidOperationException
		{
			public CannotUncheckException() : 
				base("Cannot uncheck radio button, check another one in the same group instead.")
			{
			}
		}
	}
}
