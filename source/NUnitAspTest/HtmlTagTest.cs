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
		private HtmlTag tag;

		[SetUp]
		public void SetUp()
		{
			string test = "<html><myTag id='tag' attr='one'>The body</myTag></html>";
			tag = new HtmlTag(test, "tag", test);
		}

		[Test]
		public void TestVisibility()
		{
			string test = "<html><myTag id='tagOne' /></html>";
			HtmlTag tagOne = new HtmlTag(test, "tagOne", test);
			HtmlTag tagNone = new HtmlTag(test, "tagNone", test);
			Assert.IsTrue(tagOne.Visible);
			Assert.IsFalse(tagNone.Visible);
		}

		[Test]
		public void TestXPath()
		{
			string test = "<html><myTag foo='bar' /></html>";
			HtmlTag tag = new HtmlTag(test, "//*[@foo='bar']");
			Assert.IsTrue(tag.Visible);
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
			Assert.AreEqual("The body", tag.Body);
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
			HtmlTag tag = new HtmlTag(test, "parent", test);

			Assert.IsTrue(tag.HasChildren("c"));
			Assert.IsFalse(tag.HasChildren("x"));

			Assert.AreEqual("0", tag.Children("c")[0].Body);
			Assert.AreEqual("1", tag.Children("c")[1].Body);
			Assert.AreEqual("2", tag.Children("c")[2].Body);
			Assert.AreEqual("3<c>nested</c>", tag.Children("c")[3].Body);
			Assert.AreEqual("4", tag.Children("c")[4].Body);
		}

		[Test]
		public void TestChild()
		{
			string test = "<p id='parent'><one>1</one><two>A</two><two>B</two></p>";
			HtmlTag tag = new HtmlTag(test, "parent", test);

			Assert.AreEqual("1", tag.Child("one").Body);
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestChild_WhenNoChildren()
		{
			string test = "<p id='parent'><one>1</one><two>A</two><two>B</two></p>";
			HtmlTag tag = new HtmlTag(test, "parent", test);

			tag.Child("none");
		}
		
		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestChild_WhenTwoManyChildren()
		{
			string test = "<p id='parent'><one>1</one><two>A</two><two>B</two></p>";
			HtmlTag tag = new HtmlTag(test, "parent", test);

			tag.Child("two");
		}
	}
}
