#region Copyright (c) 2005 James Shore
/********************************************************************************************************************
'
' Copyright (c) 2005, James Shore
' Created by Ben Monro.  Copyright assigned to Brian Knowles and Jim Shore on SourceForge "Patches"
' tracker, item #1184020, 15 April 2005.
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

namespace NUnitAspTestPages.AspTester
{
	public class RepeaterTestPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Repeater Repeater1;
		protected System.Web.UI.WebControls.Repeater Repeater3;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Repeater Repeater2;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			ArrayList list1 = new ArrayList();

			SomeThing thing1 = new SomeThing("Hello");
			thing1.InnerList.Add("Hello");
			thing1.InnerList.Add("World");

			SomeThing thing2 = new SomeThing("World");
			thing2.InnerList.Add("blah");
			thing2.InnerList.Add("blah");
			thing2.InnerList.Add("blah");
			thing2.InnerList.Add("blah");

			list1.Add(thing1);
			list1.Add(thing2);

			ArrayList list2 = new ArrayList();

			list2.Add("wtf?");

			Repeater1.DataSource = list1;
			Repeater2.DataSource = list2;

			Repeater1.DataBind();
			Repeater2.DataBind();

		}


		public void btnInner_Click(object sender, EventArgs args)
		{
			Button clickedButton = (Button)sender;

			Label1.Text = clickedButton.Attributes["ThingName"];
		}

		public class SomeThing
		{
			private readonly string _thing;
			private ArrayList innerList = new ArrayList();

			public string Thing
			{
				get
				{
					return _thing;
				}
			}

			public ArrayList InnerList
			{
				get
				{
					return innerList;
				}
				set
				{
					innerList = value;
				}
			}

			public SomeThing(string thing)
			{
				_thing = thing;
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
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button1_Click(object sender, System.EventArgs e)
		{
			SomeThing t1 = new SomeThing("Thing 1");
			SomeThing t2 = new SomeThing("Thing 2");

			ArrayList list = new ArrayList();

			list.Add(t1);
			list.Add(t2);

			Repeater3.DataSource = list;
			Repeater3.DataBind();
		}
	}
}
