#region Copyright (c) 2002-2005, Brian Knowles, James Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005, Brian Knowles, James Shore
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
using System.Xml;

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Tester for System.Web.UI.UserControl
	/// </summary>
	public class UserControlTester : NamingContainerTester
	{
		/// <summary>
		/// <p>Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.  Testers created with this constructor
		/// will test pages loaded by the <see cref="HttpClient.Default"/>
		/// HttpClient.</p>
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public UserControlTester(string aspId) : base(aspId)
		{
		}

		/// <summary>
		/// Create a tester for a nested control.  Use this constructor when 
		/// the control you are testing is nested within another control,
		/// such as a DataGrid or UserControl.  You should also use this
		/// constructor when you're not using the 
		/// <see cref="HttpClient.Default"/> HttpClient.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.)</param>
		/// 
		/// <example>
		/// This example demonstrates how to test a label that's inside
		/// of a user control:
		/// 
		/// <code>
		/// UserControlTester user1 = new UserControlTester("user1");
		/// LabelTester label = new LabelTester("label", user1);</code>
		/// </example>
		/// 
		/// <example>This example demonstrates how to use an HttpClient
		/// other than <see cref="HttpClient.Default"/>:
		/// 
		/// <code>
		/// HttpClient myHttpClient = new HttpClient();
		/// WebForm currentWebForm = new WebForm(myHttpClient);
		/// LabelTester myTester = new LabelTester("id", currentWebForm);</code>
		/// </example>
		public UserControlTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		protected override bool IsDisabled
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Visibility of user controls cannot be determined.  This method always throws an
		/// exception.
		/// </summary>
		public override bool Visible
		{
			get
			{
				throw new VisibilityException(this.GetType().Name);
			}
		}

		/// <summary>
		/// Exception: The test tried to check the visibility of a user control.  There's no way to 
		/// directly check user control visibility because they don't generate any HTML of
		/// their own.  Change the test to check the visibility of a control inside the user
		/// control instead.
		/// </summary>
		private class VisibilityException : ApplicationException
		{
			internal VisibilityException(string className) : base(className + "s cannot be tested for visibility because they don't directly generate HTML tags")
			{
			}
		}
	}
}
