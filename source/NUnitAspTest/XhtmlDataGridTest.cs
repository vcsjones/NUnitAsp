using System;
using NUnit.Extensions.Asp;

namespace NUnit.Extensions.Asp.Test
{

	public class XhtmlDataGridTest : WebFormTestCase
	{

		public XhtmlDataGridTest(string name) : base(name)
		{
		}

		public void TestGridContents() 
		{
			Browser.GetPage("http://localhost/NUnitAsp/NUnitAspTestPages/TestBed.aspx");
			XhtmlDataGrid grid = Page.GetWebUserControl("DataGridTestBed1").GetDataGrid("DataGrid1");
			grid.AssertRowCount(3);
			grid.AssertRowEquals(0, "", "Column1", "Column2");
			grid.AssertRowEquals(1, "View", "Cell 1, 1", "Cell 1, 2");
			grid.AssertRowEquals(2, "View", "Cell 2, 1", "Cell 2, 2");
		}

	}
}
