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

	public class AspDataGridTest : WebFormTestCase
	{

		public AspDataGridTest(string name) : base(name)
		{
		}

		protected override void SetUp() 
		{
			base.SetUp();
			Browser.GetPage("http://localhost/NUnitAsp/NUnitAspTestPages/AspDataGridTestPage.aspx");
		}

		public void TestRowCount() 
		{
			AssertEquals("# of rows", 2, Grid1.RowCount);
			AssertEquals("# of rows", 1, Grid2.RowCount);
		}

		public void TestRowCells()
		{
			AssertEquals("row 1", new string[] {"Link", "Cell 1, 1", "Cell 1, 2", "1"}, Grid1.GetRow(0).Cells);
			AssertEquals("row 2", new string[] {"Link", "Cell 2, 1", "Cell 2, 2", "2"}, Grid1.GetRow(1).Cells);
			AssertEquals("row 3", new string[] {"Link", "Cell 3, 1", "Cell 3, 2", "3"}, Grid2.GetRow(0).Cells);
		}

		public void TestNestedControls()
		{
			new AspLinkButton("link1", Grid1.GetRow(1)).Click();
			AssertEquals("1,2", ClickResult.Text);
			new AspLinkButton("link2", Grid2.GetRow(0)).Click();
			AssertEquals("2,3", ClickResult.Text);
		}

		private AspDataGrid Grid1
		{
			get
			{
				return new AspDataGrid("dataGrid1", CurrentWebForm);
			}
		}

		private AspDataGrid Grid2
		{
			get
			{
				return new AspDataGrid("dataGrid2", CurrentWebForm);
			}
		}

		private AspLabel ClickResult
		{
			get
			{
				return new AspLabel("clickResult", CurrentWebForm);
			}
		}

	}
}
