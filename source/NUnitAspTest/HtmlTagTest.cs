#region Copyright (c) 2003, 2005 Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2003, 2005, Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp.Test
{
	[TestFixture]
	public class HtmlTagTest
	{
		private HtmlTagTester tag;

		[SetUp]
		public void SetUp()
		{
			string test = "<html><myTag id='tag' attr='one'>The body</myTag></html>";
			tag = HtmlTagTester.TestInstance(test, "tag", test);
		}

		[Test]
		public void TestVisibility()
		{
			string test = "<html><myTag id='tagOne' /></html>";
			HtmlTagTester tagOne = HtmlTagTester.TestInstance(test, "tagOne", test);
			HtmlTagTester tagNone = HtmlTagTester.TestInstance(test, "tagNone", test);
			Assert.IsTrue(tagOne.Visible);
			Assert.IsFalse(tagNone.Visible);
		}

		[Test]
		public void TestXPath()
		{
			string test = "<html><myTag foo='bar' /></html>";
			HtmlTagTester tag = HtmlTagTester.TestInstance(test, "//*[@foo='bar']");
			WebAssert.Visible(tag);
		}

		[Test]
		[ExpectedException(typeof(HtmlTagTester.NoHtmlIdException))]
		public void TestHtmlId_WhenNoHtmlId()
		{
			string test = "<html><myTag foo='bar' /></html>";
			HtmlTagTester tag = HtmlTagTester.TestInstance(test, "//*[@foo='bar']");
			string unused = tag.HtmlId;
		}

		[Test]
		public void TestName() 
		{
			Assert.AreEqual("myTag", tag.Name);
		}

		[Test]
		public void TestAttributes()
		{
			Assert.AreEqual("one", tag.Attribute("attr"));
			Assert.IsNull(tag.OptionalAttribute("not-present"));
			Assert.IsTrue(tag.HasAttribute("attr"));
			Assert.IsFalse(tag.HasAttribute("not-present"));
		}

		[Test]
		[ExpectedException(typeof(Tester.AttributeMissingException))]
		public void TestMissingRequiredAttribute()
		{
			tag.Attribute("not-present");
		}

		[Test]
		public void TestBody()
		{
			Assert.AreEqual("The body", tag.InnerHtml);
		}

		[Test]
		public void TestParent()
		{
			Assert.AreEqual("html", tag.Parent.Name);
		}

		[Test]
		public void TestChildrenAndHasChildren()
		{
			string test = "<html><c>Left</c><p id='parent'><c>0</c><c>1</c><r>red herring</r><c>2</c><c>3<c>nested</c></c><c>4</c></p><c>Right</c></html>";
			HtmlTagTester tag = HtmlTagTester.TestInstance(test, "parent", test);

			Assert.IsTrue(tag.HasChildren("c"));
			Assert.IsFalse(tag.HasChildren("x"));

			Assert.AreEqual("0", tag.Children("c")[0].InnerHtml);
			Assert.AreEqual("1", tag.Children("c")[1].InnerHtml);
			Assert.AreEqual("2", tag.Children("c")[2].InnerHtml);
			Assert.AreEqual("3<c>nested</c>", tag.Children("c")[3].InnerHtml);
			Assert.AreEqual("4", tag.Children("c")[4].InnerHtml);
		}

		[Test]
		public void TestChild()
		{
			string test = "<p id='parent'><one>1</one><two>A</two><two>B</two></p>";
			HtmlTagTester tag = HtmlTagTester.TestInstance(test, "parent", test);

			Assert.AreEqual("1", tag.Child("one").InnerHtml);
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestChild_WhenNoChildren()
		{
			string test = "<p id='parent'><one>1</one><two>A</two><two>B</two></p>";
			HtmlTagTester tag = HtmlTagTester.TestInstance(test, "parent", test);

			tag.Child("none");
		}
		
		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestChild_WhenTwoManyChildren()
		{
			string test = "<p id='parent'><one>1</one><two>A</two><two>B</two></p>";
			HtmlTagTester tag = HtmlTagTester.TestInstance(test, "parent", test);

			tag.Child("two");
		}
	}
}
