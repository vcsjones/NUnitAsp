namespace NUnit.GUI
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Reflection;
	using System.Security;
	using System.Windows.Forms;
	using NUnit.Framework;
	using NUnit.Runner;
  
	public class ShowDetail : System.Windows.Forms.Form
	{
		private void InitializeComponent ()
		{

        }
  
		private Button closeButton = new Button();
		private RichTextBox stackTrace = new RichTextBox();

		public ShowDetail(Exception ex) 
		{
			InitializeComponent(ex.ToString());
		}

		private void InitializeComponent(String trace)
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ShowDetail));
            // 
            // ShowDetail
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowDetail";

        }    

		private void InitializeForm()
		{
			Text = "Stack Trace";
			AutoScaleBaseSize = new Size(5, 13);
			ClientSize = new Size(666, 248);
		}

		private void InitializeStackTrace(String trace)
		{
			stackTrace.Location = new System.Drawing.Point(8,8);
			stackTrace.Size = new System.Drawing.Size(650,192);
			stackTrace.ReadOnly = true;
			stackTrace.Text = trace;
			stackTrace.WordWrap = false;
			stackTrace.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
				|System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			stackTrace.Font = new System.Drawing.Font("Courier New", 12f, 
				System.Drawing.FontStyle.Regular, 
				System.Drawing.GraphicsUnit.World);
			Controls.Add(stackTrace);
		}

		private void InitializeCloseButton()
		{
			closeButton.Location = new Point(598, 208);
			closeButton.Size = new Size(60, 24);
			closeButton.TabIndex = 1;
			closeButton.Text = "Close";
			closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			closeButton.Click += new System.EventHandler(CloseButtonClicked);

			// add to the form
			AcceptButton = closeButton;
			Controls.Add(closeButton);
		}

		private void CloseButtonClicked(object sender, System.EventArgs e)
		{
			Close();	
		}
	}
}
