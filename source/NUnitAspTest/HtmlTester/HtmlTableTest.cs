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
using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.AspTester;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class HtmlTableTest : NUnitAspTestCase
	{
		private HtmlTableTester table;
		private HtmlTableTester emptyTable;
		private HtmlTableTester malformedTable;
		private HtmlTableTester outerNestedTable;
		private HtmlTableTester headerTags;

		protected override void SetUp()
		{
			table = new HtmlTableTester("table");
			emptyTable = new HtmlTableTester("emptyTable");
			malformedTable = new HtmlTableTester("malformed");
			outerNestedTable = new HtmlTableTester("outerNestedTable");
			headerTags = new HtmlTableTester("headerTags");

			Browser.GetPage(BaseUrl + "/HtmlTester/HtmlTableTestPage.aspx");
		}

		[Test]
		public void Basics()
		{
			string[][] expected = new string[][]
			{
				new string[] {"UL", "UR"},
				new string[] {"ML", "MR"},
				new string[] {"BL", "BR"},
				new string[] {},
				new string[] {"<b>Has markup</b>"}
			};
			WebAssert.AreEqual(expected, table.Cells, "test table");
			Assert.AreEqual("Has markup", table.RenderedCells[4][0], "rendered cell");
		}

		[Test]
		public void EmptyTable()
		{
			WebAssert.AreEqual(new string[][] {}, emptyTable.Cells, "empty table");
		}

		[Test]
		public void MalformedTables()
		{
			string[][] expected = new string[][] 
			{
				new string[] {}
			};
			WebAssert.AreEqual(expected, malformedTable.Cells, "malformed table");
		}

		[Test]
		public void NestedTables()
		{
			string[][] expectedNonRendered = new string[][]
			{
				new string[] {"<table id=\"innerNestedTable\"><tr><td>Inner Left</td><td>Inner Right</td></tr></table>", "Outer"}
			};
			WebAssert.AreEqual(expectedNonRendered, outerNestedTable.Cells, "outer table, not rendered");

			string[][] expectedRendered = new string[][]
			{
				new string[] {"Inner LeftInner Right", "Outer"}
			};
			WebAssert.AreEqual(expectedRendered, outerNestedTable.RenderedCells, "outer table, rendered");
		}

		[Test]
		public void TableHeaderTags()
		{
			string[][] expected = new string[][]
			{
				new string[] {"header 1", "data 1", "header 2", "data 2"}
			};
			WebAssert.AreEqual(expected, headerTags.Cells, "header tags table");
			WebAssert.AreEqual(expected, headerTags.RenderedCells, "header tags table (rendered)");
		}

		[Test]
		public void Rows()
		{
			Assert.AreEqual(5, table.Rows.Length);
		}
	}
}
