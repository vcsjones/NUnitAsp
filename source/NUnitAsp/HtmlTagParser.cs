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
	public class HtmlTagParser
	{
		private string html;

		public HtmlTagParser(string htmlIn)
		{
			html = htmlIn;
		}

		/// <summary>
		/// Returns null if no tag matches.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public string GetTagById(string id)
		{
			string whiteSpace = "\\s*";
			string requiredWhiteSpace = "\\s+";
			string elementName = "(?<name>\\w*)";
			string optionalQuote = "\"?";
			string optionalLeadingAttributes = "(.*?\\s)?";
			string optionalTrailingAttributes = "(\\s.*?)?";
			string idPattern = "id" + whiteSpace + "=" + whiteSpace + optionalQuote + id + optionalQuote;
			string backReferenceToElementName = "\\k<name>";

			string basicPattern = "<" + whiteSpace + elementName + requiredWhiteSpace + optionalLeadingAttributes + idPattern + optionalTrailingAttributes + whiteSpace;
			string leafPattern = basicPattern + "/" + whiteSpace + ">";
			string branchPattern = basicPattern + ">.*?<" + whiteSpace + "/" + whiteSpace + backReferenceToElementName + whiteSpace + ">";

			Match match = Regex.Match(html, leafPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				match = Regex.Match(html, branchPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
				if (!match.Success) return null;
			}
			return match.Captures[0].Value;
		}
	}
}
