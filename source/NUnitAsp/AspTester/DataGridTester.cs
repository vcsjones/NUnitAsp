#region Copyright (c) 2002-2004 Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2004 Brian Knowles, Jim Shore
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
using System.Xml;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.AspTester
{
	/// <summary>
	/// Tester for System.Web.UI.WebControls.DataGrid
	/// </summary>
	public class DataGridTester : AspControlTester
	{
		/// <summary>
		/// <p>Create a tester for a top-level control.  Use this constructor
		/// for testing most controls.  Testers created with this constructor
		/// will test pages loaded by the <see cref="HttpClient.Default"/>
		/// HttpClient.</p>
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public DataGridTester(string aspId) : base(aspId)
		{
		}

		/// <summary>
		/// Create a tester for a nested control.  Use this constructor when 
		/// the control you are testing is nested within another control,
		/// such as a DataGrid or UserControl.  You should also use this
		/// constructor when you're not using the 
		/// <see cref="HttpClient.Default"/> HttpClient.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.)</param>
		/// 
		/// <example>
		/// This example demonstrates how to test a label that's inside
		/// of a user control:
		/// 
		/// <code>
		/// UserControlTester user1 = new UserControlTester("user1");
		/// LabelTester label = new LabelTester("label", user1);</code>
		/// </example>
		/// 
		/// <example>This example demonstrates how to use an HttpClient
		/// other than <see cref="HttpClient.Default"/>:
		/// 
		/// <code>
		/// HttpClient myHttpClient = new HttpClient();
		/// WebForm currentWebForm = new WebForm(myHttpClient);
		/// LabelTester myTester = new LabelTester("id", currentWebForm);</code>
		/// </example>
		public DataGridTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// The number of rows in the data grid, not counting the header.
		/// </summary>
		public int RowCount 
		{
			get
			{
				return Tag.Children("tr").Length - 1;
			}
		}

		/// <summary>
		/// An array of string arrays containing the contents of the data grid, 
		/// not counting the header.  The outer array represents rows and the inner arrays
		/// represents cells within the rows.  Whitespace has been trimmed from the front and
		/// back of the cells.
		/// </summary>
		[CLSCompliant(false)]
		public string[][] TrimmedCells
		{
			get
			{
				string[][] result = new string[RowCount][];
				for (int i = 0; i < RowCount; i++)
				{
					result[i] = GetRow(i).TrimmedCells;
				}
				return result;
			}
		}

		/// <summary>
		/// The data grid's header row.  The first row is always assumed to be the header row.
		/// </summary>
		public Row GetHeaderRow()
		{
			return new Row(0, this);
		}

		/// <summary>
		/// Returns a row from the data grid.  Row number zero is the first row <b>after</b>
		/// the header row.
		/// </summary>
		public Row GetRow(int rowNumber)
		{
			return new Row(rowNumber + 1, this);
		}

		/// <summary>
		/// Returns a row containing a specific cell.
		/// </summary>
		/// <param name="columnNumber">The column containing the cell to look for (zero-based).</param>
		/// <param name="trimmedValue">The cell to look for.</param>
		public Row GetRowByCellValue(int columnNumber, string trimmedValue)
		{
			string[][] cells = TrimmedCells;
			for (int i = 0; i < cells.GetLength(0); i++)
			{
				if (cells[i][columnNumber] == trimmedValue) return GetRow(i);
			}
			WebAssert.Fail(string.Format("Expected to find a row with '{0}' in column {1} of {2}", trimmedValue, columnNumber, HtmlIdAndDescription));
			throw new ApplicationException("This line cannot execute.  Fail() throws an exception.");
		}

		/// <summary>
		/// Click a column heading link that was generated with the "allowSorting='true'" attribute.
		/// </summary>
		/// <param name="columnNumberZeroBased">The column to sort (zero-based)</param>
		public void Sort(int columnNumberZeroBased)
		{
			Row header = GetHeaderRow();
			HtmlTag cell = header.GetCellElement(columnNumberZeroBased);
			HtmlTag[] links = cell.Children("a");
			WebAssert.True(links.Length != 0, "Attempted to sort non-sortable grid (" + HtmlIdAndDescription + ")");
			WebAssert.True(links.Length == 1, "Expected sort link to have exactly one anchor tag");

			Form.PostBack(links[0].Attribute("href"));
		}

		private HtmlTag GetRowTag(int rowNumber)
		{
			return Tag.Children("tr")[rowNumber];
		}

		protected internal override string GetChildElementHtmlId(string aspId)
		{
			try 
			{
				int rowNumber = int.Parse(aspId);
				return HtmlId + "__ctl" + (rowNumber + 1);
			}
			catch (FormatException) 
			{
				throw new ContainerMustBeRowException(aspId, this);
			}
		}
		
		/// <summary>
		/// Tests a row within a data grid.
		/// </summary>
		public class Row : AspControlTester
		{
			private int rowNumber;
			private DataGridTester container;

			/// <summary>
			/// Create the tester and link it to a row in a specific data grid.
			/// </summary>
			/// <param name="rowNumberWhereZeroIsHeader">The row to test.</param>
			/// <param name="container">The data grid that contains the row.</param>
			public Row(int rowNumberWhereZeroIsHeader, DataGridTester container) : base(rowNumberWhereZeroIsHeader.ToString(), container)
			{
				this.rowNumber = rowNumberWhereZeroIsHeader;
				this.container = container;
			}

			protected internal override string GetChildElementHtmlId(string inAspId)
			{
				return HtmlId + "_" + inAspId;
			}

			protected override HtmlTag Tag
			{
				get
				{
					return container.GetRowTag(rowNumber);
				}
			}

			/// <summary>
			/// The cells in the row.  Whitespace has been trimmed from the front and back
			/// of the cells.
			/// </summary>
			public string[] TrimmedCells
			{
				get
				{
					HtmlTag[] cells = Tag.Children("td");
					string[] cellText = new string[cells.Length];
					for (int i = 0; i < cells.Length; i++) 
					{
						cellText[i] = cells[i].BodyNoTags.Trim();
					}
					return cellText;
				}
			}

			internal HtmlTag GetCellElement(int columnNumberZeroBased)
			{
				HtmlTag[] cells = Tag.Children("td");
				WebAssert.True(columnNumberZeroBased >= 0 && columnNumberZeroBased < cells.Length, "There is no column #" + columnNumberZeroBased + " in " + HtmlIdAndDescription);
				return cells[columnNumberZeroBased];
			}
		}
	}

	/// <summary>
	/// Exception: The container of the control being tested was a DataGridTester, but
	/// it should be a Row.  Change "new MyTester("foo", datagrid)" to 
	/// "new MyTester("foo", datagrid.getRow(rowNum))".
	/// </summary>
	public class ContainerMustBeRowException : ApplicationException 
	{
		internal ContainerMustBeRowException(string aspId, DataGridTester dataGrid) 
			: base(GetMessage(aspId, dataGrid))
		{
		}

		private static string GetMessage(string aspId, DataGridTester dataGrid) 
		{
			return String.Format(
				"Tester '{0}' has DataGridTester '{1}' as its container. That isn't allowed. "
				+ "It should be a DataGridTester.Row. When constructing {0}, pass '{1}.getRow(#)' "
				+ "as the container argument.",
				aspId, dataGrid.AspId);
		}
	}
}
