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

	public class HttpBrowserTestPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.LinkButton redirect;
		protected System.Web.UI.WebControls.LinkButton dropCookie;
		protected Label cookie;

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetCookieLabel();
		}

		protected void redirect_Click(object sender, EventArgs args)
		{
			Redirect();
		}

		private void Redirect()
		{
			Response.Redirect("RedirectionTarget.aspx");
		}

		protected void dropCookie_Click(object sender, EventArgs args)
		{
			SetCookie();
			SetCookieLabel();
		}

		private void SetCookie()
		{
			Response.SetCookie(new HttpCookie("TestCookie", "TestCookieValue"));
		}
	
		protected void dropCookieAndRedirect_Click(object sender, EventArgs args)
		{
			SetCookie();
			Redirect();
		}

		private void SetCookieLabel()
		{
			HttpCookie requestCookie = Request.Cookies["TestCookie"];
			if (requestCookie == null) cookie.Text = "Not Set";
			else cookie.Text = requestCookie.Value;
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
