#region Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
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
using NUnit.Framework;
using NUnit.Extensions.Asp.AspTester;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class ListItemCollectionTest : NUnitAspTestCase
	{
		private ListItemCollectionTester collectionOne;
		private ListItemCollectionTester collectionTwo;
		
		protected override void SetUp()
		{
			base.SetUp();

			Browser.GetPage(BaseUrl + "/AspTester/ListBoxTestPage.aspx");
			collectionOne = new ListBoxTester("list", CurrentWebForm).Items;
			collectionTwo = new ListBoxTester("disabledList", CurrentWebForm).Items;
		}

		[Test]
		public void TestContains_True()
		{
			AssertEquals(true, collectionOne.Contains(collectionOne[1]));
		}

		[Test]
		public void TestContains_False()
		{
			AssertEquals(false, collectionOne.Contains(collectionTwo[0]));
		}

		[Test]
		public void TestFindByText()
		{
			AssertEquals("two", collectionOne.FindByText("two").RenderedText);
		}

		[Test]
		public void TestFindByValue()
		{
			AssertEquals("2", collectionOne.FindByValue("2").Value);
		}
	}
}
