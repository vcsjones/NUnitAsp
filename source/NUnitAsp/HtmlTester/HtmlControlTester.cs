#region Copyright (c) 2002, 2003 Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp.HtmlTester
{
	/// <summary>
	/// Base class for all testers in HtmlTester namespace
	/// </summary>
	public abstract class HtmlControlTester : ControlTester
	{
		private bool runAtServer;

		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  (In the page's ASP.NET
		/// source code, look for the tag that the control is nested in.  That's probably the
		/// control's container.  Use CurrentWebForm if the control is just nested in the form tag.)</param>
		/// <param name="runAtServer">Tells tester whether the control under test is running on the server side.</param>
		public HtmlControlTester(string aspId, Tester container, bool runAtServer) :
			base(aspId, container)
		{
			this.runAtServer = runAtServer;
		}

		/// <summary>
		/// Gets or sets a value indicating wheither the control is disabled.
		/// </summary>
		public bool Disabled
		{
			get
			{
				return IsDisabled;
			}
		}
	
		/// <summary>
		/// The HTML ID of the control being tested.  It corresponds to the
		/// ID of the HTML tag rendered by the server.  It's useful for looking at 
		/// raw HTML while debugging.
		/// </summary>
		public override string HtmlId
		{
			get
			{
				if (runAtServer)
				{
					return base.HtmlId;
				}
				else
				{
					return AspId;
				}
			}
		}
	}
}
