#region Copyright (c) 2002, 2003 Brian Knowles, Jim Little
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
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NUnit.Extensions.Asp.Test.AspTester
{
	public class DataGridTestPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid dataGrid1;
		protected System.Web.UI.WebControls.DataGrid dataGrid2;
		protected System.Web.UI.WebControls.Label headerResult;
		protected System.Web.UI.WebControls.Label clickResult;

		private void Page_Load(object sender, System.EventArgs e)
		{
			RowData row1;
			RowData row2;

			row1 = new RowData(1);
			row2 = new RowData(2);
			dataGrid1.DataSource = new RowData[] {row1, row2};
			dataGrid1.DataBind();

			row1 = new RowData(3);
			dataGrid2.DataSource = new RowData[] {row1};
			dataGrid2.DataBind();
		}

		protected void dataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "click")
			{
				clickResult.Text = "1," + e.CommandArgument;
			}
		}

		protected void dataGrid1_Sort(object sender, DataGridSortCommandEventArgs args)
		{
			headerResult.Text = args.SortExpression;
		}

		protected void link2_Clicked(object sender, EventArgs args)
		{
			clickResult.Text = "2," + ((LinkButton)sender).CommandArgument;
		}

		private class RowData
		{
			private int rowNum;

			public RowData(int rowNum) 
			{
				this.rowNum = rowNum;
			}

			public string RowNumber
			{
				get
				{
					return rowNum.ToString();
				}
			}

			public string Column1
			{
				get 
				{
					return "Cell " + rowNum + ", 1";
				}
			}

			public string Column2
			{
				get
				{
					return "Cell " + rowNum + ", 2";
				}
			}

			public string SpaceColumn
			{
				get
				{
					return "Space: ";
				}
			}

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
