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
using System.Text.RegularExpressions;

namespace NUnit.Extensions.Asp.AspTester
{
	public class LinkButtonTester : ControlTester
	{
		public LinkButtonTester(string aspId, Control container) : base(aspId, container)
		{
		}

		public void Click()
		{
			string postBackCall = GetAttributeValue("href");
			string postBackPattern = @"__doPostBack\('(?<target>.*?)',''\)";

			Match match = Regex.Match(postBackCall, postBackPattern, RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				throw new ParseException(HtmlIdAndDescription + " doesn't look like a link button");
			}

			string target = match.Groups["target"].Captures[0].Value;

			EnterInputValue("__EVENTTARGET", target);
			EnterInputValue("__EVENTARGUMENT", "");
			Submit();
		}

		public string Text
		{
			get
			{
				return Element.InnerXml;
			}
		}

		private class ParseException : ApplicationException
		{
			internal ParseException(string message) : base(message)
			{
			}
		}
								 
	}
}
