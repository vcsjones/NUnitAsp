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
'******************************************************************************************************************/

using System;
using NUnit.Framework;
using System.Xml;

namespace NUnit.Extensions.Asp 
{
	public abstract class WebFormTestCase : TestCase 
	{

		private Browser _browser;

		public WebFormTestCase(string name) : base(name) 
		{
		}

		protected override void SetUp() 
		{
			_browser = new Browser();
   		}

		protected override void TearDown() 
		{
			_browser.Dispose();
		}

		protected XhtmlWebForm Page 
		{
			get 
			{
				return Browser.CurrentPage;
			}
		}



//		protected void AssertElementExists(string id, bool expected) 
//		{
//			string not = expected ? " " : " not ";
//			AssertElementExists("Element '" + id + "' should" + not + "be on the page", id, expected);
//		}
//
//		protected void AssertElementExists(string message, string id, bool expected) 
//		{
//			bool exists = (_browser.Page.GetElementById(id) != null);
//			Assert(message, expected == exists);
//		}		
//
//		protected void AssertQueryStringHasValue(string name, string aValue) 
//		{
//			AssertEquals("Query String '" + name + "'", aValue, _browser.QueryString[name]);
//		}
//
//		protected void AssertMultiLineTextBoxValue(string id, string aValue) 
//		{
//			AssertElementExists(String.Format("Textarea '{0}' is not on the page", id), id, "textarea");
//			XmlElement textArea = _browser.Page.GetElementById(id);
//			string message = String.Format("Textarea '{0}' does not have the correct text", id);
//			AssertEquals(message, aValue, textArea.InnerText);
//		}
//
//		protected void AssertTextBoxValue(string id, string aValue) 
//		{
//			XmlElement textBox = _browser.Page.GetElementById(id);
//			AssertNotNull(String.Format("TextBox '{0}' is not on the page", id), textBox);
//			string message = String.Format("TextBox '{0}' does not have the correct value,", id);
//			AssertEquals(message, aValue, textBox.GetAttribute("value"));
//		}
//
//		protected void AssertLabelText(string id, string text) 
//		{
//			XmlElement label = _browser.Page.GetElementById(id);
//			AssertNotNull(String.Format("Label '{0}' is not on the page", id), label);
//			string message = String.Format("Label '{0}'", id);
//			AssertNotNull(message + " should not be null", label.InnerText);
//			AssertEquals(message, text, label.InnerText.Trim());
//		}
//
//		protected void AssertDataGridRowCount(string dataGridId, int expectedCountIncludingHeader) 
//		{
//			XmlElement table = GetDataGridTable(dataGridId);
//			XmlNodeList rows = table.GetElementsByTagName("tr");
//			AssertEquals("table '" + dataGridId + "' row count", expectedCountIncludingHeader, rows.Count);
//		}
//
//		protected void AssertDataGridRow(string dataGridId, int rowNumberIncludingHeader, string[] expectedCells) 
//		{
//			XmlElement table = GetDataGridTable(dataGridId);
//			XmlElement row = (XmlElement)table.GetElementsByTagName("tr")[rowNumberIncludingHeader];
//			XmlNodeList cells = row.GetElementsByTagName("td");
//			AssertEquals("table '" + dataGridId + "' row " + rowNumberIncludingHeader + " column count", expectedCells.Length, cells.Count);
//			for (int i = 0; i < cells.Count; i++) 
//			{
//				XmlElement cell = (XmlElement)cells[i];
//				AssertEquals("column " + i + ", row " + rowNumberIncludingHeader + " in table '" + dataGridId + "'", expectedCells[i], cell.InnerText.Trim());
//			}
//		}
//
//		private XmlElement GetDataGridTable(string dataGridId) 
//		{
//			AssertDataGridExists(dataGridId);
//			XmlElement table = _browser.Page.GetElementById(dataGridId);
//			AssertEquals("element " + dataGridId, "table", table.Name);
//			return table;
//		}
//
//		protected void AssertDataGridCellValue(string dataGridId, string text) 
//		{
//			XmlElement table = GetDataGridTable(dataGridId);
//			XmlNodeList cells = table.GetElementsByTagName("td");
//			string message = String.Format("DataGrid '{0}' should have a cell with the text '{1}'", dataGridId, text);
//			AssertValueInCells(message, cells, text);
//		}
//
//		protected void AssertDataGridColumnHeader(string dataGridId, string text) 
//		{
//			AssertDataGridExists(dataGridId);
//			XmlElement table = GetDataGridTable(dataGridId);
//			string message = String.Format("DataGrid '{0}' should have a header with the text '{1}'", dataGridId, text);
//			AssertValueInCells(message, table.FirstChild.ChildNodes, text);
//		}
//
//		protected void AssertDataGridCellHasButton(string dataGridId, int rowNumberIncludingHeader, int controlColumnIndex) 
//		{
//			AssertDataGridExists(dataGridId);
//			XmlElement table = _browser.Page.GetElementById(dataGridId);
//			XmlNodeList rows = table.GetElementsByTagName("tr");
//			XmlNode row = rows[rowNumberIncludingHeader];
//			string expression = String.Format("//input[@type='submit'][@name='{0}:ctrl{1}:ctrl{2}']", dataGridId, rowNumberIncludingHeader - 1, controlColumnIndex);
//			string message = String.Format("DataGrid '{0}' does not have a button in row {1}, control column {2}", dataGridId, rowNumberIncludingHeader - 1, controlColumnIndex + 1);
//			AssertExpressionExists(message, expression, row);
//		}
//
//		protected void AssertValueInCells(string message, XmlNodeList cells, string aValue) 
//		{
//			bool found = false;
//			foreach (XmlNode cell in cells) 
//			{
//				found = aValue.Equals(cell.InnerText.Trim());
//				if (found) 
//				{
//					break;
//				}
//			}
//			Assert(message, found);
//		}
//
//		protected void AssertDataGridExists(string id) 
//		{
//			string message = String.Format("DataGrid '{0}' is not on the page", id);
//			AssertElementExists(message, id, "table");
//		}
//
//		protected void AssertDataGridDoesNotExist(string id) 
//		{
//			string message = String.Format("DataGrid '{0}' should not be on the page", id);
//			AssertElementDoesNotExist(message, id);
//		}
//
//		protected void AssertPageName(string bodyId) 
//		{
//			XmlNodeList xmlNodes = _browser.Page.GetElementsByTagName("body");
//			// Pick the first one off since there should be just one.
//			XmlNode bodyNode = xmlNodes.Item(0);
//			AssertNotNull("There should be a body element on the page (and it must be lowercase).", bodyNode);
//			AssertNotNull("There should be an 'id' attribute on the page's body element.", bodyNode.Attributes["id"]);
//			AssertEquals("Page Name (<body id='name'>)" + _browser.PageText, bodyId, bodyNode.Attributes["id"].Value);
//		}
//
//		protected void AssertFormExists(string id) 
//		{
//			string message = String.Format("Form '{0}' is no on the page", id);
//			AssertElementExists(message, id);
//		}
//
//		protected void AssertHyperLinkExists(string id) 
//		{
//			string message = String.Format("HyperLink '{0}' is not on the page", id);
//			AssertElementExists(message, id, "a");
//		}
//
//		protected void AssertLabelExists(string id) 
//		{
//			string message = String.Format("Label '{0}' is not on the page", id);
//			AssertElementExists(message, id);
//		}
//
//		protected void AssertLabelDoesNotExist(string id) 
//		{
//			string message = String.Format("Label '{0}' should not be on the page", id);
//			AssertElementDoesNotExist(message, id);
//		}
//
//		protected void AssertElementExists(string id, bool expected) 
//		{
//			string not = expected ? " " : " not ";
//			AssertElementExists("Element '" + id + "' should" + not + "be on the page", id, expected);
//		}
//
//		protected void AssertElementExists(string message, string id, bool expected) 
//		{
//			bool exists = (_browser.Page.GetElementById(id) != null);
//			Assert(message, expected == exists);
//		}
//
//		protected void AssertElementExists(string id) 
//		{
//			AssertElementExists(id, true);
//		}
//
//		protected void AssertElementDoesNotExist(string id) 
//		{
//			AssertElementExists(id, false);
//		}
//
//		protected void AssertElementExists(string message, string id) 
//		{
//			AssertElementExists(message, id, true);
//		}
//
//		protected void AssertElementExists(string message, string id, string elementName) 
//		{
//			XmlElement element = _browser.Page.GetElementById(id);
//			AssertNotNull(message, element);
//			AssertEquals(message, elementName, element.Name);
//		}
//
//		protected void AssertElementDoesNotExist(string message, string id) 
//		{
//			AssertElementExists(message, id, false);
//		}
//
//		protected void AssertDropDownHasListItem(string dropDownId, string optionText) 
//		{
//			AssertDropDownListExists(dropDownId);
//			string expression = String.Format("//select[@id='{0}'][option='{1}']", dropDownId, optionText);
//			string message = String.Format("The DropDownList '{0}' does not have a ListItem with the text '{1}'", dropDownId, optionText);
//			AssertExpressionExists(message, expression);
//		}
//
//		protected void AssertValidationMessageExists(string id) 
//		{
//			string message = String.Format("Validation message for '{0}' is not on the page", id);
//			AssertElementExists(message, id);
//		}
//
//		protected void AssertValidationMessageDoesNotExist(string id) 
//		{
//			string message = String.Format("Validation message for '{0}' should not be on the page", id);
//			AssertElementDoesNotExist(message, id);
//		}
//
//		protected void AssertDropDownListSelection(string dropDownID, string listItemValue) 
//		{
//			AssertDropDownListExists(dropDownID);
//			string expression = String.Format("//select[@id='{0}']/option[@value='{1}'][@selected='selected']", dropDownID, listItemValue);
//			string message = String.Format("DropDownList '{0}' should have option '{1}' selected", dropDownID, listItemValue);
//			AssertExpressionExists(message, expression);
//		}
//
//		protected void AssertDropDownListSelectionByText(string dropDownId, string listItemText) 
//		{
//			AssertDropDownListExists(dropDownId);
//			string expression = String.Format("//select[@id='{0}']/option[@selected='selected']", dropDownId);
//			XmlNode selectedOption = _browser.Page.SelectSingleNode(expression);
//			AssertNotNull(String.Format("DropDownList '{0}' should have an option selected", dropDownId), selectedOption);
//			AssertEquals(String.Format("DropDownList '{0}' selected option", dropDownId), listItemText, selectedOption.InnerText);
//		}
//
//		protected void AssertSubmitButtonExists(string id) 
//		{
//			string message = String.Format("Button '{0}' is not on the page", id);
//			AssertElementExists(message, id); 
//		}
//
//		protected void AssertSubmitButtonDoesNotExist(string id) 
//		{
//			string message = String.Format("Button '{0}' should not be on the page", id);
//			AssertElementDoesNotExist(message, id);
//		}
//
//		protected void AssertTextBoxExists(string id) 
//		{
//			string message = String.Format("TextBox '{0}' is not on the page", id);
//			AssertElementExists(message, id);
//		}
//
//		protected void AssertMultiLineTextBoxExists(string id, int rows) 
//		{
//			string message = String.Format("Multi-Line TextBox '{0}' is not on the page", id);
//			AssertElementExists(message, id);
//		}
//
//		protected void AssertDropDownListExists(string id) 
//		{
//			string message = "DropDownList '" + id + "' is not on the page";
//			AssertElementExists(message, id);
//		}
//
//		protected void AssertElementHasAttributeValue(string element, string attribute, string aValue) 
//		{
//			string message = String.Format("Element '{0}' with attribute '{1}' with value '{2}' is not on the page", element, attribute, aValue);
//			AssertElementHasAttributeValue(element, attribute, aValue, message);
//		}
//
//		protected void AssertElementHasAttributeValue(string element, string attribute, string aValue, string message) 
//		{
//			string expression = "//" + element + "[@" + attribute + "='" + aValue + "']";
//			AssertExpressionExists(message, expression);
//		}
//
//		protected void AssertExpressionExists(string expression) 
//		{
//			AssertExpressionExists("Element not found for expression: " + expression, expression);
//		}
//
//		protected void AssertExpressionExists(string message, string expression) 
//		{
//			AssertExpressionExists(message, expression, _browser.Page);
//		}
//
//		protected void AssertExpressionExists(string message, string expression, XmlNode node) 
//		{
//			AssertNotNull(message, node);
//			XmlNode result = node.SelectSingleNode(expression);
//			AssertNotNull(message, result);
//		}
//
//		protected void AssertCookieExists(string cookieName) 
//		{
//			AssertNotNull(cookieName);
//			Assert("Cookie '" + cookieName + "' not found in browser", _browser.HasCookie(cookieName));
//		}

		protected Browser Browser 
		{
			get 
			{
				return _browser;
			}
			set 
			{
				_browser = value;
			}
		}

	}
}