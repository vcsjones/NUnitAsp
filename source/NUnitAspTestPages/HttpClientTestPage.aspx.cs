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
using NUnit.Extensions.Asp.Test;

namespace NUnitAspTestPages
{

	public class HttpBrowserTestPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.LinkButton redirect;
		protected System.Web.UI.WebControls.LinkButton dropCookie;
		protected System.Web.UI.WebControls.LinkButton dropCookieAndRedirect;
		protected System.Web.UI.WebControls.LinkButton dropCookieWithExpiry;
		protected Label cookie;
		protected Label postBackStatus;
		protected System.Web.UI.WebControls.LinkButton postBack;
		protected Label testParm;
		protected Label userAgent;
		protected Label userLanguages;

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetCookieLabel();
			postBackStatus.Text = "Not Clicked";
			testParm.Text = Request["testparm"];
			userAgent.Text = Request.UserAgent;
			userLanguages.Text = UserLanguages();
		}

		protected void redirect_Click(object sender, EventArgs args)
		{
			Redirect();
		}

		protected void dropCookie_Click(object sender, EventArgs args)
		{
			SetCookie();
		}
	
		protected void dropCookieAndRedirect_Click(object sender, EventArgs args)
		{
			SetCookie();
			Redirect();
		}

		protected void postBack_Click(object sender, EventArgs args)
		{
			postBackStatus.Text = "Clicked";
		}

		protected void dropCookieWithExpiry_Click(object sender, System.EventArgs e)
		{
			SetCookieExpires();
		}

		private void Redirect()
		{
			Response.Redirect("RedirectionTarget.aspx#fragment");
		}

		private void SetCookie()
		{
			Response.SetCookie(new HttpCookie(HttpClientTest.TEST_COOKIE_NAME, HttpClientTest.TEST_COOKIE_VALUE));
		}

		private void SetCookieExpires()
		{
			HttpCookie cookie = new HttpCookie(HttpClientTest.TEST_COOKIE_NAME, HttpClientTest.TEST_COOKIE_VALUE);
			cookie.Expires = new DateTime(2024, 11, 20, 14, 0, 0, 0);
			Response.SetCookie(cookie);
		}

		private void SetCookieLabel()
		{
			HttpCookie requestCookie = Request.Cookies[HttpClientTest.TEST_COOKIE_NAME];
			if (requestCookie == null) cookie.Text = "Not Set";
			else cookie.Text = requestCookie.Value;
		}

		private string UserLanguages()
		{
			if (Request.UserLanguages == null) return "Not Set";

			string languages = "";
			foreach (string language in Request.UserLanguages)
			{
				languages += "[" + language + "]";
			}
			return languages;
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
