/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Little
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

using System;
using System.Xml;
using System.Web.UI.WebControls;
using NUnit.Framework;

namespace NUnit.Extensions.Asp
{

	public class XhtmlTextBox : XhtmlElement
	{

		internal XhtmlTextBox(Browser browser, XmlElement element, string aspId, string containerDescription)
			: base(browser, element, aspId, containerDescription)
		{
		}

		protected override string ElementType 
		{
			get 
			{
				return "text box";
			}
		}

		public string Text {
			set {
				Browser.EnterInputValue(GetAttributeValue("name"), value);
			}
		}

		public void AssertTextMode(TextBoxMode expectedMode) 
		{
			AssertEquals(expectedMode, TextMode);
		}

		private TextBoxMode TextMode 
		{
			get 
			{
				if (GetAttributeValue("type") == "password") return TextBoxMode.Password;
				else return TextBoxMode.SingleLine;
			}
		}

	}
}
