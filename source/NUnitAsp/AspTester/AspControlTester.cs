#region Copyright (c) 2002-2005 Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005 Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Base class for testing all ASP.NET server controls.  Extend this class
	/// if you're creating a tester for a custom control.
	/// </summary>
	public abstract class AspControlTester : ControlTester
	{
		/// <summary>
		/// Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public AspControlTester(string aspId) : base(aspId)
		{
		}

		/// <summary>
		/// Create a tester for a nested control.  Use this constructor when 
		/// the control you are testing is nested within another control,
		/// such as a DataGrid or UserControl.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.)</param>
		public AspControlTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// True if the control is enabled.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return !IsDisabled;
			}
		}
	}
}
