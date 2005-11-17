#region Copyright (c) 2004, 2005, James Shore
/********************************************************************************************************************
'
' Copyright (c) 2004, 2005, James Shore
' Originally written by Kyle Heon.  Copyright assigned to Brian Knowles and James Shore on SourceForge
' "Patches" tracker, item #1024063, 2004-09-07.  Brian Knowles' copyright subquentially assigned to
' James Shore on nunitasp-devl mailing list, 8/22/2005.
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
using System.Text.RegularExpressions;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Base class for all validator testers.
	/// </summary>
	public class ValidatorTester : AspControlTester
	{
		/// <summary>
		/// <p>Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.  Testers created with this constructor
		/// will test pages loaded by the <see cref="HttpClient.Default"/>
		/// HttpClient.</p>
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public ValidatorTester(string aspId) : base(aspId)
		{
		}

		/// <summary>
		/// Create a tester for a nested control.  Use this constructor when 
		/// the control you are testing is nested within another control,
		/// such as a DataGrid or UserControl.  You should also use this
		/// constructor when you're not using the 
		/// <see cref="HttpClient.Default"/> HttpClient.
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
		/// WebFormTester webForm = new WebFormTester(myHttpClient);
		/// LabelTester myTester = new LabelTester("id", webForm);</code>
		/// </example>
		public ValidatorTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		public HtmlSpanTester SpanTag
		{
			get
			{
				return new HtmlSpanTester(HtmlId, Form);
			}
		}

		/// <summary>
		/// Returns the error message.
		/// </summary>
		public string ErrorMessage
		{
			get 
			{ 
				return SpanTag.InnerHtml; 
			}
		}

		/// <summary>
		/// Returns the error message, rendered to a string as a web browser would do.
		/// </summary>
		public string RenderedErrorMessage
		{
			get 
			{ 
				return SpanTag.RenderedInnerHtml; 
			}
		}

		/// <summary>
		/// Unlike most controls, this control returns 'true' only if the validator is actually
		/// visible to the user.
		/// </summary>
		public override bool Visible
		{
			get
			{
				bool visible = SpanTag.Visible;
				if (!visible) return false;

				string style = SpanTag.OptionalAttribute("style");
				bool hidden = (style != null && style.IndexOf("visibility:hidden") != -1);
				bool displayNone = (style != null && style.IndexOf("display:none") != -1);

				if (visible && !hidden && !displayNone) return true;
				else return false;
			}
		}
	}
}
