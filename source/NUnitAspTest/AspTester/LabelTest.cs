#region Copyright (c) 2002-2005, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005, Brian Knowles, Jim Shore
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
using NUnit.Extensions.Asp.AspTester;
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test.AspTester
{
    [TestFixture]
	public class LabelTest : NUnitAspTestCase
	{
		protected override void SetUp()
		{
			base.SetUp();
			Browser.GetPage(BaseUrl + "/AspTester/LabelTestPage.aspx");
		}

        [Test]
		public void TestOneParameterConstructor()
		{
			LabelTester textLabel = new LabelTester("textLabel");
			AssertEquals("text", "foo", textLabel.Text);
		}

        [Test]
		public void TestText() 
		{
			LabelTester textLabel = new LabelTester("textLabel");
			AssertEquals("text", "foo", textLabel.Text);
		}

        [Test]
		public void TestSpace()
		{
			LabelTester spaceLabel = new LabelTester("spaceLabel");
			AssertEquals("space", "foo ", spaceLabel.Text);
		}

        [Test]
		public void TestFormatted()
		{
			LabelTester formattedLabel = new LabelTester("formattedLabel");
			AssertEquals("formatted", "a <i>HTML</i> tag", formattedLabel.Text);
		}

        [Test]
		public void TestNested()
		{
			LabelTester outerLabel = new LabelTester("outerLabel");
			LabelTester innerLabel = new LabelTester("innerLabel", outerLabel);
			AssertEquals("inner", "inner", innerLabel.Text);
		}
	}
}
