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
	public class ValidatorTestPage : System.Web.UI.Page
	{
		protected CustomValidator cuvValidateNum;
		protected TextBox txtNum1;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.CompareValidator cvPassword;
		protected System.Web.UI.WebControls.TextBox txtPassword2;
		protected System.Web.UI.WebControls.TextBox txtAge;
		protected System.Web.UI.WebControls.RangeValidator rvAge;
		protected System.Web.UI.WebControls.TextBox txtPhone;
		protected System.Web.UI.WebControls.RegularExpressionValidator revPhone;
		protected System.Web.UI.WebControls.TextBox txtNum;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected System.Web.UI.WebControls.TextBox clientSideTextBox;
		protected System.Web.UI.WebControls.RequiredFieldValidator staticClientSideValidator;
		protected System.Web.UI.WebControls.TextBox staticClientSideTextBox;
		protected System.Web.UI.WebControls.TextBox dynamicClientSideTextBox;
		protected System.Web.UI.WebControls.RequiredFieldValidator dynamicClientSideValidator;
		protected System.Web.UI.WebControls.TextBox noneClientSideTextBox;
		protected System.Web.UI.WebControls.RequiredFieldValidator noneClientSideValidator;
		protected TextBox txtNum2;

		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected void cuvValidateNum_ServerValidate(object source, ServerValidateEventArgs args)
		{
			try
			{
				int num = int.Parse(args.Value);
				args.IsValid = ((num%5) == 0);
			}
			catch
			{
				args.IsValid = false;
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
			this.cuvValidateNum.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.cuvValidateNum_ServerValidate);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
