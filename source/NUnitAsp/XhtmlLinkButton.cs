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
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace NUnit.Extensions.Asp
{

	public class XhtmlLinkButton : XhtmlElement
	{

		internal XhtmlLinkButton(Browser browser, XmlElement element, string aspId, string containerDescription)
			: base(browser, element, aspId, containerDescription)
		{
		}

		protected override string ElementType 
		{
			get 
			{
				return "link button";
			}
		}
		public XhtmlWebForm Click() {
			string id = GetAttributeValue("id");
			string postBackCall = GetAttributeValue("href");
			string postBackPattern = @"__doPostBack\('(?<target>.*?)',''\)";

			Match match = Regex.Match(postBackCall, postBackPattern, RegexOptions.IgnoreCase);
			string message = string.Format("{0} doesn't look like a link button", Description);
			Assertion.Assert(message, match.Success);

			string target = match.Groups["target"].Captures[0].Value;

			Browser.EnterInputValue("__EVENTTARGET", target);
			Browser.EnterInputValue("__EVENTARGUMENT", "");
			return Browser.SubmitForm();
		}

	}
}
