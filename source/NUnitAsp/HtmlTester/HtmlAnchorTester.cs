#region Copyright (c) 2002-2004, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2004, Brian Knowles, Jim Shore
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
using System.Text.RegularExpressions;
using System.Web;

namespace NUnit.Extensions.Asp.HtmlTester
{
	/// <summary>
	/// Tester for System.Web.UI.HtmlControls.HtmlAnchor
	/// </summary>
	public class HtmlAnchorTester : HtmlControlTester
	{
		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  (In the page's ASP.NET
		/// source code, look for the tag that the control is nested in.  That's probably the
		/// control's container.  Use CurrentWebForm if the control is just nested in the form tag.)</param>
		/// <param name="runAtServer">Tells tester whether the control under test is running on the server side.</param>
		public HtmlAnchorTester(string aspId, Tester container, bool runAtServer) : base(aspId, container, runAtServer)
		{
		}

		/// <summary>
		/// Click the link.  Supports postback and pop-up windows.
		/// </summary>
		public void Click()
		{
			string popupLink = PopupLink;
			if (popupLink != null)
			{
				Browser.FollowLink(popupLink);
				return;
			}
			string hRef = HRef;
			if (Form.IsPostBack(hRef))
			{
				Form.PostBack(hRef);
				return;
			}
			Browser.FollowLink(hRef);
		}
		
		/// <summary>
		/// The HRef of the link.
		/// </summary>
		public string HRef
		{
			get
			{
				return HttpUtility.HtmlDecode(Tag.Attribute("href"));
			}
		}

		/// <summary>
		/// The HRef of the pop-up window's link.  Null if this anchor doesn't have 
		/// a recognizable pop-up link.
		/// </summary>
		public string PopupLink
		{
			get
			{
				string onClick = Tag.OptionalAttribute("onclick");
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
					string message = string.Format("Found more than one 'window.open' call in onclick attribute of {0}, but only expected to find one", HtmlIdAndDescription);
					throw new ParseException(message);
				}
			}
		}
	}
}
