#region Copyright (c) 2002, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Tester for System.Web.UI.WebControls.TextBox
	/// </summary>
	public class TextBoxTester : AspControlTester
	{
		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  (In the page's ASP.NET
		/// source code, look for the tag that the control is nested in.  That's probably the
		/// control's container.  Use CurrentWebForm if the control is just nested in the form tag.)</param>
		public TextBoxTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// The text in the text box.
		/// </summary>
		public string Text {
			set 
			{
				EnterInputValue(Tag.Attribute("name"), value);
			}
			get
			{
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
				if (TextMode != TextBoxMode.MultiLine) 
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
