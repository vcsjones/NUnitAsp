#region Copyright (c) 2002, 2005 Brian Knowles, James Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2005, Brian Knowles, James Shore
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
using System.Web.UI.WebControls;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Tester for System.Web.UI.WebControls.TextBox
	/// </summary>
	public class TextBoxTester : AspControlTester
	{
		/// <summary>
		/// <p>Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.  Testers created with this constructor
		/// will test pages loaded by the <see cref="HttpClient.Default"/>
		/// HttpClient.</p>
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public TextBoxTester(string aspId) : base(aspId)
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
		/// WebForm currentWebForm = new WebForm(myHttpClient);
		/// LabelTester myTester = new LabelTester("id", currentWebForm);</code>
		/// </example>
		public TextBoxTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// The text in the text box.
		/// </summary>
		public string Text 
		{
			set 
			{
				AssertVisible();
				EnterInputValue(Tag.Attribute("name"), value);
			}
			get
			{
				AssertVisible();
				string text;
				if (TextMode == TextBoxMode.MultiLine)
				{
					text = Tag.BodyNoTags;
				}
				else 
				{                                  
					text = Tag.OptionalAttribute("value");
				}

				if (text == null) return "";
				return text;
			}
		}

		/// <summary>
		/// The kind of text box.
		/// </summary>
		public TextBoxMode TextMode 
		{
			get 
			{
				AssertVisible();
				if (Tag.Name == "textarea") 
				{
					return TextBoxMode.MultiLine;
				}
				else 
				{
					WebAssert.AreEqual("input", Tag.Name, "tag name");
					string type = Tag.Attribute("type");
					if (type == "password") return TextBoxMode.Password;
					else 
					{
						WebAssert.AreEqual("text", type, "type");
						return TextBoxMode.SingleLine;
					}
				}
			}
		}

		/// <summary>
		/// Maximum number of characters to display in the text box.  
		/// Returns 0 if there is no max length.
		/// </summary>
		public int MaxLength
		{
			get
			{
				AssertVisible();
				if (TextMode == TextBoxMode.MultiLine) 
				{
					throw new ApplicationException("max length is ignored on a TextBox when TextMode is MultiLine");
				}

				string maxLength = Tag.OptionalAttribute("maxlength");
				if (maxLength == null || maxLength == "") return 0;
				else return int.Parse(maxLength);
			}
		}
	}
}
