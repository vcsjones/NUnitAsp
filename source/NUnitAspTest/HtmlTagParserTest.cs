/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Little
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
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test
{
	public class HtmlTagParserTest : NUnitAspTestCase
	{
		private void RunTest(string tag)
		{
			RunTest(tag, tag);
		}

		private void RunTest(string sourceHtml, string expectedTag)
		{
			HtmlTagParser parser = new HtmlTagParser(sourceHtml);
			AssertEquals("tag", expectedTag, parser.GetTagById("testId"));
		}

		public void TestSingleTagAndNoQuotes()
		{
			RunTest("<tag id=testId />");
		}

		public void TestSingleTagAndSingleQuotes()
		{
			RunTest("<tag id='testId' />");
		}

		public void TestSingleTagAndDoubleQuotes()
		{
			RunTest("<tag id=\"testId\" />");
		}

		public void TestSingleTagAndMultipleAttributes()
		{
			RunTest("<tag foo=bar id=testId bar=foo />");
		}

		public void TestSingleTagWithRepeatedTag()
		{
			RunTest("<tag id=testId /><tag id=notTestId />", "<tag id=testId />");
		}

		public void TestTagPairAndNoQuotes()
		{
			RunTest("<tag id=testId></tag>");
		}

		public void TestTagPairAndSingleQuotes()
		{
			RunTest("<tag id='testId'></tag>");
		}

		public void TestTagPairAndDoubleQuotes()
		{
			RunTest("<tag id=\"testId\"></tag>");
		}

		public void TestTagPairAndMultipleLines()
		{
			RunTest("<tag\r\nid=testId\r\n/>");
		}

		public void TestTagPairWithEmbeddedTag()
		{
			RunTest("<tag id=testId><embeddedTag></embeddedTag></tag>");
		}

		public void TestTagPairWithRepeatedTag()
		{
			RunTest("<tag id=testId></tag><tag id=notTestId></tag>", "<tag id=testId></tag>");
		}

		public void TestWrongId()
		{
			RunTest("<tag id=notTestId />", null);
		}

		public void TestNoId()
		{
			RunTest("<tag />", null);
		}

		public void TestSameIdTwice()
		{
			try
			{
				RunTest("<tag1 id=testId /><tag2 id=testId />");
				Fail("Expected exception");
			}
			catch(HtmlTagParser.UniqueIdException)
			{
				Assert("correct behavior", true);
			}
		}

		public void TestSameIdPrefixTwice()
		{
			RunTest("<tag1 id=testId/><tag2 id=testId1/>", "<tag1 id=testId/>");
			RunTest("<tag1 id=testId /><tag2 id=testId1 />", "<tag1 id=testId />");
			RunTest("<tag1 id='testId'/><tag2 id='testId1'/>", "<tag1 id='testId'/>");
			RunTest("<tag1 id=\"testId\"/><tag2 id=\"testId1\"/>", "<tag1 id=\"testId\"/>");
			RunTest("<tag1 id=testId></tag1><tag2 id=testId1></tag2>", "<tag1 id=testId></tag1>");
		}
	}
}
