namespace NUnit.GUI 
{
	using System;
	using System.Diagnostics;
	using System.Drawing;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Reflection;

	/// <summary>
	///    Summary description for AboutBox.
	/// </summary>
	public class AboutBox : System.Windows.Forms.Form 
	{

		/// <summary>
		///    Required by the Win Forms designer
		/// </summary>
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button OK;

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label Version;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;

		public AboutBox() 
		{
			// Required for Win Form Designer support
			InitializeComponent();
			Version.Text = "Version " + NUnit.Runner.Version.id();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}


		/// <summary>
		///    Required method for Designer support - do not modify
		///    the contents of this method with an editor
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutBox));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(47, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(524, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "NUnit - XUnit testing framework for .Net";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(47, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(524, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Copyright © Philip Craig 2000 - 2002";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.World);
            this.label3.Location = new System.Drawing.Point(47, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "http://nunit.sourceforge.net/";
            this.label3.Click += new System.EventHandler(this.httpClick);
            // 
            // Version
            // 
            this.Version.Location = new System.Drawing.Point(51, 136);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(314, 23);
            this.Version.TabIndex = 2;
            this.Version.Text = "Version unknown";
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(424, 136);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(58, 29);
            this.OK.TabIndex = 5;
            this.OK.Text = "OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(47, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(524, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "Thanks to Jim Newkirk, Ethan Smith, Kent Beck and Erich Gamma";
            // 
            // AboutBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(495, 181);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.label3,
                                                                          this.OK,
                                                                          this.label4,
                                                                          this.Version,
                                                                          this.label2,
                                                                          this.label1});
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutBox";
            this.Text = "About NUnit";
            this.Load += new System.EventHandler(this.AboutBox_Load);
            this.ResumeLayout(false);

        }
		protected void httpClick(object sender, System.EventArgs e)
		{
			Process.Start("http://nunit.sourceforge.net/");
		}
		protected void OK_Click(object sender, System.EventArgs e)
		{
			Close();
		}

        private void AboutBox_Load(object sender, System.EventArgs e) {
        
        }

        private void label2_Click(object sender, System.EventArgs e) {
        
        }

	}
}