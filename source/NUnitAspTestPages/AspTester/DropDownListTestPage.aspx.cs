/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Shore
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NUnitAspTestPages.AspTester
{
	public class DropDownListTestPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.LinkButton submit;
		protected System.Web.UI.WebControls.DropDownList list;
		protected System.Web.UI.WebControls.DropDownList emptyList;
		protected System.Web.UI.WebControls.LinkButton clearSelection;
		protected System.Web.UI.WebControls.CheckBox auto;
		protected System.Web.UI.WebControls.Label indexChanged;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			indexChanged.Text = "No";
			if (!IsPostBack)
			{
				list.Items.Add(new ListItem("one", "1"));
				list.Items.Add(new ListItem("two", "2"));
				list.Items.Add(new ListItem("three", "3"));
				list.SelectedIndex = 1;
			}
			list.AutoPostBack = auto.Checked;
			list.SelectedIndexChanged += new EventHandler(index_Changed);
		}

		protected void clearSelection_Click(object sender, EventArgs args)
		{
			list.ClearSelection();
		}

		protected void add_Click(object sender, EventArgs args)
		{
			list.Items.Add(DateTime.Now.ToString());
		}

		protected void index_Changed(object sender, EventArgs args)
		{
			indexChanged.Text = "Yes";
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
