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

		public XhtmlWebUserControl UserControl(string id) 
		{
			return new XhtmlWebUserControl(this, id, HtmlId(id), Description);
		}

		public XhtmlLabel Label(string id) 
		{
			return new XhtmlLabel(browser, Element(id), id, Description);
		}

		public XhtmlTextBox TextBox(string id) 
		{
			return new XhtmlTextBox(browser, Element(id), id, Description);
		}

		public XhtmlLinkButton LinkButton(string id) 
		{
			return new XhtmlLinkButton(browser, Element(id), id, Description);
		}

		public XhtmlDataGrid DataGrid(string id)
		{
			return new XhtmlDataGrid(browser, Element(id), id, HtmlId(id), Description);
		}

		public void AssertIdEquals(string expected) 
		{
			Assertion.AssertEquals("page id", expected, Id);
		}

		public void AssertElementVisibility(string id, bool expectedVisibility) 
		{
			string not = expectedVisibility ? " " : " not ";
			string message = string.Format("Element '{0}' (aka '{1}') should{2}be on {3}", id, HtmlId(id), not, Description);
			Assertion.Assert(message, expectedVisibility == HasElement(id));
		}

		protected virtual string HtmlId(string id) 
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
			return (page.GetElementById(HtmlId(id)) != null);
		}

		private XmlElement Element(string id) 
		{
			AssertElementVisibility(id, true);
			return page.GetElementById(HtmlId(id));
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




		internal void ClickLink(string id) {
			XmlElement link = page.GetElementById(id);
			browser.GetPage(link.GetAttribute("href"));
		}

		public void ClickLinkButton(string id, string formId) {
			XmlElement linkButton = page.GetElementById(id);
			if (linkButton == null) throw new ApplicationException("Link '" + id + "' not found");
			string postBackCall = linkButton.GetAttribute("href");
			string postBackPattern = @"javascript:__doPostBack\('(?<target>.*?)',''\)";

			Match match = Regex.Match(postBackCall, postBackPattern, RegexOptions.IgnoreCase);
			if (!match.Success) throw new XmlException("Link '" + id + "' doesn't look like a link button", null);
			string target = match.Groups["target"].Captures[0].Value;

			browser.EnterInputValue("__EVENTTARGET", target);
			browser.EnterInputValue("__EVENTARGUMENT", "");
			browser.SubmitForm(formId);
		}

		internal void SelectDropDownListItem(string dropDownId, string textValue) {
			XmlNode selectNode = page.GetElementById(dropDownId);
			string inputValue = null;
			bool found = false;

			foreach (XmlNode selectOption in selectNode.ChildNodes) {
				if (selectOption.InnerText == textValue) {
					found = true;
					XmlAttribute valueAttribute = selectOption.Attributes["value"];
					if (valueAttribute == null) {
						// Use the option text for the value
						browser.EnterInputValue(dropDownId, textValue);
					}
					else {
						inputValue = valueAttribute.Value;
					}
				}
			}

			if (!found) {
				throw new XmlException(string.Format("DropDownList '{0}' does not have ListItem '{1}'", dropDownId, textValue), null);
			}

			browser.EnterInputValue(dropDownId, inputValue);
		}

		internal void ClickButton(string buttonId, string formId) {
			browser.EnterInputValue(buttonId, GetButtonValue(buttonId));
			browser.SubmitForm(formId);
		}

		internal void ClickButtonInDataGrid(string dataGridId, int rowNumberIncludingHeader, int controlColumnIndex, string formId) {
			string buttonName = string.Format("{0}:ctrl{1}:ctrl{2}", dataGridId, rowNumberIncludingHeader, controlColumnIndex);
			ClickButtonByName(buttonName, formId);
		}

		public void ClickLinkButtonInDataGrid(string dataGridId, int rowNumberIncludingHeader, string buttonId, string formId) {
			string linkName = string.Format("{0}__ctl{1}_{2}", dataGridId, rowNumberIncludingHeader + 1, buttonId);
			ClickLinkButton(linkName, formId);
		}

		internal void ClickButtonByName(string buttonName, string formId) {
			browser.EnterInputValue(buttonName, GetButtonValue(buttonName, formId));
			browser.SubmitForm(formId);
		}

		internal string GetButtonValue(string buttonId) {
			XmlElement button = page.GetElementById(buttonId);
			if (button == null) {
				throw new XmlException(String.Format("Button '{0}' is not on the page", buttonId), null);
			}
			return button.GetAttribute("value");
		}

		internal string GetButtonValue(string buttonName, string formId) {
			XmlElement form = page.GetElementById(formId);
			string expression = String.Format("//input[@type='submit'][@name='{0}']", buttonName);
			XmlNode button = form.SelectSingleNode(expression);
			if (button == null) {
				throw new XmlException(String.Format("Button '{0}' for form '{1}' is not on the page..." + page.InnerXml, buttonName, formId), null);
			}
			string returnValue = String.Empty;
			foreach (XmlAttribute attribute in button.Attributes) {
				if (attribute.Name.Equals("value")) {
					returnValue = attribute.Value;
				}
			}
			return returnValue;
		}

		internal string GetFormMethod(string formId) {
			return GetAttribute("form", formId, "method");
		}

		internal string GetFormAction(string formId) {
			return GetAttribute("form", formId, "action");
		}

		private string GetAttribute(string element, string id, string attribute) {
			XmlElement xmlElement = page.GetElementById(id);
			if (xmlElement == null) {
				throw new XmlException(String.Format("Element '{0}' with id '{1}' is not on the page", element, id), null);
			}
			return xmlElement.GetAttribute(attribute);
		}

		public override string ToString() 
		{
			return page.InnerXml;
		}

	}

}
