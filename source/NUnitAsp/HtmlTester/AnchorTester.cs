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
using System.Web;

namespace NUnit.Extensions.Asp.HtmlTester
{
	public class AnchorTester : ControlTester
	{
		private bool runAtServer;

		public AnchorTester(string aspId, Control container, bool runAtServer) : base(aspId, container)
		{
			this.runAtServer = runAtServer;
		}

		public void Click()
		{
			string link = PopupLink;
			if (link == null) link = HRef;
			Browser.GetPage(link);
		}			
		
		public string HRef
		{
			get
			{
				return HttpUtility.HtmlDecode(GetAttributeValue("href"));
			}
		}

		/// <summary>
		/// Null if this anchor doesn't have a recognizable pop-up link.
		/// </summary>
		public string PopupLink
		{
			get
			{
				string onClick = GetOptionalAttributeValue("onclick");
				if (onClick == null) return null;

				RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
				Match match = Regex.Match(onClick, "window.open\\('(?<link>.*?)'", options);
				if (match.Captures.Count == 1)
				{
					return match.Groups["link"].Value;
				}

				if (match.Captures.Count == 0) return null;
				else
				{
					string message = string.Format("Found two 'window.open' calls in onclick attribute of {0}, but only expected to find one", HtmlIdAndDescription);
					throw new ParseException(message);
				}
			}
		}

			public override string HtmlId
		{
			get
			{
				if (runAtServer) return base.HtmlId;
				else return AspId;
			}
		}
	}
}
