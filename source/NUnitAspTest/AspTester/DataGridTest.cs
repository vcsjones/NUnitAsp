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
using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test.AspTester
{
	public class DataGridTest : NUnitAspTestCase
	{
		private DataGridTester grid1;
		private DataGridTester grid2;
		private LabelTester clickResult;

		public DataGridTest(string name) : base(name)
		{
		}

		protected override void SetUp() 
		{
			base.SetUp();
			grid1 = new DataGridTester("dataGrid1", CurrentWebForm);
			grid2 = new DataGridTester("dataGrid2", CurrentWebForm);
			clickResult = new LabelTester("clickResult", CurrentWebForm);

			Browser.GetPage(BaseUrl + "AspTester/DataGridTestPage.aspx");
		}

		public void TestRowCount() 
		{
			AssertEquals("# of rows", 2, grid1.RowCount);
			AssertEquals("# of rows", 1, grid2.RowCount);
		}

		public void TestCells()
		{
			string[][] expected = new string[][]
			{
				new string[] {"Link", "Cell 1, 1", "Cell 1, 2", "Space:", "1"}, 
				new string[] {"Link", "Cell 2, 1", "Cell 2, 2", "Space:", "2"}, 
			};
			AssertEquals("cells", expected, grid1.TrimmedCells);				
		}

		public void TestRowCells()
		{
			AssertEquals("row 1", new string[] {"Link", "Cell 1, 1", "Cell 1, 2", "Space:", "1"}, grid1.GetRow(0).TrimmedCells);
			AssertEquals("row 2", new string[] {"Link", "Cell 2, 1", "Cell 2, 2", "Space:", "2"}, grid1.GetRow(1).TrimmedCells);
			AssertEquals("row 3", new string[] {"Link", "Cell 3, 1", "Cell 3, 2", "Space:", "3"}, grid2.GetRow(0).TrimmedCells);
		}

		public void TestNestedControls()
		{
			new LinkButtonTester("link1", grid1.GetRow(1)).Click();
			AssertEquals("1,2", clickResult.Text);
			new LinkButtonTester("link2", grid2.GetRow(0)).Click();
			AssertEquals("2,3", clickResult.Text);
		}

		public void TestHeaderRow()
		{
			string[] expected = new string[] {"", "Column1", "Column2", "SpaceColumn", "RowNumber"};
			AssertEquals("header", expected, grid1.GetHeaderRow().TrimmedCells);
		}
	}
}
