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

using System;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace NUnit.Extensions.Asp {

	public class XhtmlWebForm {
		private Browser browser;
		private XmlDocument page;

		internal XhtmlWebForm(Browser browser, string pageBody) {
			this.browser = browser;
			page = new XmlDocument();
			page.LoadXml(pageBody);
		}

		protected XhtmlWebForm(XhtmlWebForm page) 
		{
			this.browser = page.browser;
			this.page = page.page;
		}

		public XhtmlWebUserControl GetWebUserControl(string aspId) 
		{
			return new XhtmlWebUserControl(this, aspId, CreateHtmlId(aspId), Description);
		}

		public XhtmlLabel GetLabel(string aspId) 
		{
			return new XhtmlLabel(browser, GetElement(aspId), aspId, Description);
		}

		public XhtmlCheckBox GetCheckBox(string aspId)
		{
			return new XhtmlCheckBox(browser, GetElement(aspId), aspId, Description);
		}

		public XhtmlTextBox GetTextBox(string aspId) 
		{
			return new XhtmlTextBox(browser, GetElement(aspId), aspId, Description);
		}

		public XhtmlLinkButton GetLinkButton(string aspId) 
		{
			return new XhtmlLinkButton(browser, GetElement(aspId), aspId, Description);
		}

		public XhtmlDataGrid GetDataGrid(string aspId)
		{
			return new XhtmlDataGrid(browser, GetElement(aspId), aspId, Description);
		}

		internal XhtmlForm GetForm()
		{
			return new XhtmlForm(browser, GetElementByName("form"), Description);
		}

		public void AssertIdEquals(string expected) 
		{
			Assertion.AssertEquals("page id", expected, Id);
		}

		public void AssertElementVisibility(string aspId, bool expectedVisibility) 
		{
			string not = expectedVisibility ? " " : " not ";
			string message = string.Format("Element '{0}'{1}expected ('{2}' on {3})", CreateHtmlId(aspId), not, aspId, Description);
			Assertion.Assert(message, expectedVisibility == HasElement(aspId));
		}

		protected virtual string CreateHtmlId(string aspId) 
		{
			return aspId;
		}

		protected virtual string Description 
		{
			get 
			{
				return "page '" + Id + "'";
			}
		}

		private bool HasElement(string aspId) 
		{
			return (page.GetElementById(CreateHtmlId(aspId)) != null);
		}

		private XmlElement GetElement(string aspId) 
		{
			AssertElementVisibility(aspId, true);
			return page.GetElementById(CreateHtmlId(aspId));
		}

		private XmlElement GetElementByName(string name)
		{
			XmlNodeList elements = page.GetElementsByTagName(name);
			Assertion.AssertEquals("# of " + name + "elements", 1, elements.Count);
			return (XmlElement)elements[0];
		}

		private string Id 
		{
			get 
			{
				XmlAttribute id = GetElementByName("body").Attributes["id"];
				Assertion.AssertNotNull("There should be an 'id' attribute on the page's body element.", id);
				return id.Value;
			}
		}

		public override string ToString() 
		{
			return page.InnerXml;
		}

	}

}
