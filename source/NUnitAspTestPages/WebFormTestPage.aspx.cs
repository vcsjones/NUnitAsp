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

namespace NUnitAspTestPages
{
	public class WebFormTestPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label submitted;
		protected System.Web.UI.HtmlControls.HtmlInputHidden one_tag;
		protected System.Web.UI.HtmlControls.HtmlInputHidden two_tag;
		protected System.Web.UI.WebControls.LinkButton one;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			string one = Request.Form["tag_one"];
			string two = Request.Form["tag_two"];
			if (two == null) submitted.Text = "form 1";
			else if (one == null) submitted.Text = "form 2";
			else submitted.Text = "both submitted??";

			if (one == null && two == null) submitted.Text = "none";
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
