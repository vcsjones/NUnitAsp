#region Copyright (c) 2002, 2003, Brian Knowles, Jim Little
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, Brian Knowles, Jim Little
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
	/// Tester for System.Web.UI.WebControls.CheckBox
	/// </summary>
	public class CheckBoxTester : AspControlTester
	{
		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to link to.</param>
		/// <param name="container">The control that contains the control to link to</param>
		public CheckBoxTester(string aspId, Control container) : base(aspId, container)
		{
		}

		/// <summary>
		/// True if the checkbox is checked, false if not.
		/// </summary>
		public bool Checked
		{
			get
			{
				return GetOptionalAttributeValue("checked") != null;
			}
			set
			{
				string inputName = GetAttributeValue("name");
				if (value == true) 
				{
					string checkBoxValue = GetOptionalAttributeValue("value");
					if (checkBoxValue == null) checkBoxValue = "on";
					EnterInputValue(inputName, checkBoxValue);
				}
				else
				{
					RemoveInputValue(inputName);
				}
			}
		}	

		protected override bool IsDisabled
		{
			get
			{
				return Element.ParentNode.Attributes["disabled"] != null;
			}
		}
	}
}
