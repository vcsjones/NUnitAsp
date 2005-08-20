#region Copyright (c) 2005, James Shore
/********************************************************************************************************************
'
' Copyright (c) 2005, James Shore
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
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class HtmlSelectTest : NUnitAspTestCase
	{
		protected override void SetUp()
		{
			Browser.GetPage(BaseUrl + "/HtmlTester/HtmlSelectTestPage.aspx");
		}

		[Test]
		public void TestDefaultProperties()
		{
			HtmlSelectTester def = new HtmlSelectTester("default");

			Assert.AreEqual(-1, def.SelectedIndex);
			Assert.AreEqual(1, def.Size);
			Assert.IsFalse(def.Multiple);
			WebAssert.AreEqual(new string[] {}, def.Items);
		}

		[Test]
		public void TestNonDefaults()
		{
			HtmlSelectTester nonDefault = new HtmlSelectTester("nonDefault");

			Assert.AreEqual(2, nonDefault.SelectedIndex);
//			Assert.AreEqual(5, nonDefault.Size);
//			Assert.IsTrue(nonDefault.Multiple);
//			WebAssert.AreEqual(new string[] {"one", "two", "three", "four", "five"}, nonDefault.Items);
		}
			

		//SelectedIndex
		//Size
		//Items
		//Multiple

		//set selected
		//auto post-back?
		//publicly-accessible ItemTags

	}
}
