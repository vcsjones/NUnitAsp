#region Copyright (c) 2003 Brian Knowles, Jim Little
/********************************************************************************************************************
'
' Copyright (c) 2003, Brian Knowles, Jim Little
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUnitAspTestPages.AspTester
{
	public class UserControlTestPage : Page
	{
		protected PlaceHolder placeHolder;
		protected Label clickResult;

		protected override void CreateChildControls()
		{
			Control container = new MockUserControl();
			container.ID = "userControl";

			placeHolder.Controls.Add(container);

			container.Controls.Add(BuildLabel());
			container.Controls.Add(BuildSpacer());

			container.Controls.Add(BuildButton());
			container.Controls.Add(BuildSpacer());

			container.Controls.Add(BuildLinkButton());
		}

		private void button_Click(object sender, EventArgs e)
		{
			clickResult.Text = "Clicked";
		}


		private Label BuildLabel()
		{
			Label label = new Label();
			label.ID = "label";
			label.Text = "Label";
			return label;
		}

		private Button BuildButton()
		{
			Button button = new Button();
			button.ID = "button";
			button.Text = "Button";
			button.Click += new EventHandler(button_Click);
			return button;
		}

		private LinkButton BuildLinkButton()
		{
			LinkButton button = new LinkButton();
			button.ID = "linkButton";
			button.Text = "Link Button";
			button.Click += new EventHandler(button_Click);
			return button;
		}

		private LiteralControl BuildSpacer()
		{
			return new LiteralControl(" ");
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
		}
		#endregion

		private class MockUserControl : Control, INamingContainer
		{
		}
	}
}
