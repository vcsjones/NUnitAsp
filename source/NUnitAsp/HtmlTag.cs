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

namespace NUnit.Extensions.Asp
{
	/// <summary>
	/// An HTML tag.
	/// 
	/// Not intended for third-party use.  The API for this class will change in 
	/// future releases.  This class may not be present in future releases.
	/// </summary>
	public class HtmlTag
	{
		string html;

		public HtmlTag(string html)
		{
			this.html = html;
		}

		public string GetAttributeValue(string attributeName)
		{
			string whiteSpace = "\\s*";
			string leftQuote = "(?<leftQuote>['\"])";
			string rightQuote = "\\k<leftQuote>";
			string attributePattern = attributeName + whiteSpace + "=" + whiteSpace + leftQuote + "(?<value>.*?)" + rightQuote;

			Match match = Regex.Match(html, attributePattern, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
			if (!match.Success) return null;
			return match.Groups["value"].Value;
		}
	}
}
