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

namespace NUnit.Extensions.Asp.AspTester
{

	public class UserControlTester : ControlTester
	{
		private string aspId;
		private Control container;

		public UserControlTester(string aspId, Control container) : base(aspId, container)
		{
			this.aspId = aspId;
			this.container = container;
		}

		internal override string GetChildElementHtmlId(string aspId)
		{
			return HtmlId + "_" + aspId;
		}

		public override bool Visible
		{
			get
			{
				throw new VisibilityException(this.GetType().Name);
			}
		}

		private class VisibilityException : ApplicationException
		{
			internal VisibilityException(string className) : base(className + "s cannot be tested for visibility because they don't directly generate HTML tags")
			{
			}
		}

	}

}
