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

namespace NUnit.Extensions.Asp.Test
{

	public class AspDataGridTest : NUnitAspTestCase
	{
		private AspDataGrid grid1;
		private AspDataGrid grid2;
		private AspLabel clickResult;

		public AspDataGridTest(string name) : base(name)
		{
		}

		protected override void SetUp() 
		{
			base.SetUp();
			grid1 = new AspDataGrid("dataGrid1", CurrentWebForm);
			grid2 = new AspDataGrid("dataGrid2", CurrentWebForm);
			clickResult = new AspLabel("clickResult", CurrentWebForm);

			Browser.GetPage("http://localhost/NUnitAsp/NUnitAspTestPages/AspDataGridTestPage.aspx");
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
				new string[] {"Link", "Cell 1, 1", "Cell 1, 2", "1"}, 
				new string[] {"Link", "Cell 2, 1", "Cell 2, 2", "2"}, 
			};
			AssertEquals("cells", expected, grid1.Cells);				
		}

		public void TestRowCells()
		{
			AssertEquals("row 1", new string[] {"Link", "Cell 1, 1", "Cell 1, 2", "1"}, grid1.GetRow(0).Cells);
			AssertEquals("row 2", new string[] {"Link", "Cell 2, 1", "Cell 2, 2", "2"}, grid1.GetRow(1).Cells);
			AssertEquals("row 3", new string[] {"Link", "Cell 3, 1", "Cell 3, 2", "3"}, grid2.GetRow(0).Cells);
		}

		public void TestNestedControls()
		{
			new AspLinkButton("link1", grid1.GetRow(1)).Click();
			AssertEquals("1,2", clickResult.Text);
			new AspLinkButton("link2", grid2.GetRow(0)).Click();
			AssertEquals("2,3", clickResult.Text);
		}

	}
}
