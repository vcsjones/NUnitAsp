/********************************************************************************************************************
'
' Copyright (c) 2004, 2005, James Shore
' Originally written by Kyle Heon.  Copyright assigned to Brian Knowles and James Shore on SourceForge
' "Patches" tracker, item #1024063, 7 Sept 2004.  Brian Knowles' copyright subquentially assigned to
' James Shore on nunitasp-devl mailing list, 22 Aug 2005.
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
using NUnit.Extensions.Asp.AspTester;
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test.AspTester
{
	public class ValidatorTest : NUnitAspTestCase
	{
		ButtonTester btnSubmit;

		protected override void SetUp()
		{
			base.SetUp();

			Browser.GetPage(BaseUrl + "/AspTester/ValidatorTestPage.aspx");

			btnSubmit = new ButtonTester("btnSubmit", CurrentWebForm);
		}

		public void TestRequiredFieldValidator()
		{
			RequiredFieldValidatorTester rfvName = new RequiredFieldValidatorTester("rfvName", CurrentWebForm);
			TextBoxTester txtName = new TextBoxTester("txtName", CurrentWebForm);

			txtName.Text = "";
			btnSubmit.Click();

			AssertVisibility(rfvName, true);
			AssertEquals("Name is required", rfvName.ErrorMessage);
		}

		public void TestCompareValidator()
		{
			CompareValidatorTester cvPassword = new CompareValidatorTester("cvPassword", CurrentWebForm);
			TextBoxTester txtPassword = new TextBoxTester("txtPassword", CurrentWebForm);
			TextBoxTester txtPassword2 = new TextBoxTester("txtPassword2", CurrentWebForm);

			txtPassword.Text = "ab";
			txtPassword2.Text = "abc";
			btnSubmit.Click();

			AssertVisibility(cvPassword, true);
			AssertEquals("Passwords do not match", cvPassword.ErrorMessage);
		}

		public void TestRangeValidator()
		{
			RangeValidatorTester rvAge = new RangeValidatorTester("rvAge", CurrentWebForm);
			TextBoxTester txtAge = new TextBoxTester("txtAge", CurrentWebForm);

			txtAge.Text = "1";
			btnSubmit.Click();

			AssertVisibility(rvAge, true);
			AssertEquals("Age must be 18 or higher", rvAge.ErrorMessage);
		}

		public void TestRegularExpressionValidator()
		{
			RegularExpressionValidatorTester revPhone = new RegularExpressionValidatorTester("revPhone", CurrentWebForm);
			TextBoxTester txtPhone = new TextBoxTester("txtPhone", CurrentWebForm);

			txtPhone.Text = "556 334-4456";
			btnSubmit.Click();

			AssertVisibility(revPhone, true);
			AssertEquals("Phone must be in (###) ###-#### format", revPhone.ErrorMessage);
		}

		public void TestCustomValidator()
		{
			CustomValidatorTester cuvValidateNum = new CustomValidatorTester("cuvValidateNum", CurrentWebForm);
			TextBoxTester txtNum = new TextBoxTester("txtNum", CurrentWebForm);

			txtNum.Text = "16";
			btnSubmit.Click();

			AssertVisibility(cuvValidateNum, true);
			AssertEquals("Value must be a multiple of 5", cuvValidateNum.ErrorMessage);
		}

		public void TestValidation_WhenClientSideScriptIsTrue_AndDisplayIsStatic()
		{
			RequiredFieldValidatorTester staticClientSideValidator = new RequiredFieldValidatorTester("staticClientSideValidator");
			TextBoxTester staticClientSideTextBox = new TextBoxTester("staticClientSideTextBox");

			WebAssert.NotVisible(staticClientSideValidator, "should not be visible at start");
			btnSubmit.Click();

			WebAssert.Visible(staticClientSideValidator, "should be visible after submit");
			staticClientSideTextBox.Text = "stuff";
			btnSubmit.Click();

			WebAssert.NotVisible(staticClientSideValidator, "should not be visible after filled in");
		}

		public void TestValidation_WhenClientSideScriptIsTrue_AndDisplayIsDynamic()
		{
			RequiredFieldValidatorTester dynamicClientSideValidator = new RequiredFieldValidatorTester("dynamicClientSideValidator");
			TextBoxTester dynamicClientSideTextBox = new TextBoxTester("dynamicClientSideTextBox");

			WebAssert.NotVisible(dynamicClientSideValidator, "should not be visible at start");
			btnSubmit.Click();

			WebAssert.Visible(dynamicClientSideValidator, "should be visible after submit");
			dynamicClientSideTextBox.Text = "stuff";
			btnSubmit.Click();

			WebAssert.NotVisible(dynamicClientSideValidator, "should not be visible after filled in");
		}

	
		public void TestValidation_WhenClientSideScriptIsTrue_AndDisplayIsNone()
		{
			RequiredFieldValidatorTester noneClientSideValidator = new RequiredFieldValidatorTester("noneClientSideValidator");
			TextBoxTester noneClientSideTextBox = new TextBoxTester("noneClientSideTextBox");

			WebAssert.NotVisible(noneClientSideValidator, "should not be visible at start");
			btnSubmit.Click();

			WebAssert.NotVisible(noneClientSideValidator, "should not be visible after submit");
			noneClientSideTextBox.Text = "stuff";
			btnSubmit.Click();

			WebAssert.NotVisible(noneClientSideValidator, "should not be visible after filled in");
		}
	}
}
