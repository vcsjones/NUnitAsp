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

		internal XhtmlWebForm(Browser browser, byte[] pageBody) {
			this.browser = browser;
			UTF8Encoding decoder = new UTF8Encoding();
			string text = decoder.GetString(pageBody);
			page = new XmlDocument();
			page.LoadXml(text);
		}

		protected XhtmlWebForm(XhtmlWebForm page) 
		{
			this.browser = page.browser;
			this.page = page.page;
		}

		public XhtmlWebUserControl GetUserControl(string id) 
		{
			return new XhtmlWebUserControl(this, id, GetHtmlId(id), Description);
		}

		public XhtmlLabel GetLabel(string id) 
		{
			return new XhtmlLabel(browser, GetElement(id), id, Description);
		}

		public XhtmlTextBox GetTextBox(string id) 
		{
			return new XhtmlTextBox(browser, GetElement(id), id, Description);
		}

		public XhtmlLinkButton GetLinkButton(string id) 
		{
			return new XhtmlLinkButton(browser, GetElement(id), id, Description);
		}

		public XhtmlDataGrid GetDataGrid(string id)
		{
			return new XhtmlDataGrid(browser, GetElement(id), id, GetHtmlId(id), Description);
		}

		internal XhtmlForm GetForm(string id)
		{
			return new XhtmlForm(browser, GetElement(id), id, Description);
		}

		public void AssertIdEquals(string expected) 
		{
			Assertion.AssertEquals("page id", expected, Id);
		}

		public void AssertElementVisibility(string id, bool expectedVisibility) 
		{
			string not = expectedVisibility ? " " : " not ";
			string message = string.Format("Element '{0}' (aka '{1}') should{2}be on {3}", id, GetHtmlId(id), not, Description);
			Assertion.Assert(message, expectedVisibility == HasElement(id));
		}

		protected virtual string GetHtmlId(string id) 
		{
			return id;
		}

		protected virtual string Description 
		{
			get 
			{
				return "page '" + Id + "'";
			}
		}

		private bool HasElement(string id) 
		{
			return (page.GetElementById(GetHtmlId(id)) != null);
		}

		private XmlElement GetElement(string id) 
		{
			AssertElementVisibility(id, true);
			return page.GetElementById(GetHtmlId(id));
		}

		protected string Id {
			get {
				XmlNodeList xmlNodes = page.GetElementsByTagName("body");
				Assertion.AssertEquals("number of lowercase <body> elements", 1, xmlNodes.Count);

				XmlNode body = xmlNodes[0];
				XmlAttribute id = body.Attributes["id"];
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
