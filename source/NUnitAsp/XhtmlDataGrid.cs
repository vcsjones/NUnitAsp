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
using NUnit.Framework;

namespace NUnit.Extensions.Asp
{

	public class XhtmlDataGrid : XhtmlElement
	{

		private XmlNodeList rows;

		internal XhtmlDataGrid(Browser browser, XmlElement element, string aspId, string containerDescription)
			: base(browser, element, aspId, containerDescription)
		{
			rows = Element.GetElementsByTagName("tr");
		}

		protected override string ElementType 
		{
			get 
			{
				return "data grid";
			}
		}

		public XhtmlLinkButton LinkButton(int rowNumberWithHeaderStartingAtZero, string buttonId) 
		{
			string id = string.Format("{0}__ctl{1}_{2}", HtmlId, rowNumberWithHeaderStartingAtZero + 1, buttonId);
			string message = string.Format("Element '{0}' (aka '{1}') should be on {2}", buttonId, id, Description);
			XmlElement linkButton = Element.OwnerDocument.GetElementById(id);
			Assertion.Assert(message, linkButton != null);
			return new XhtmlLinkButton(Browser, linkButton, id, Description);
		}

		public void AssertRowCount(int expectedRowsIncludingHeader) 
		{
			AssertEquals(expectedRowsIncludingHeader, rows.Count);
		}

		public void AssertRowEquals(int rowNumberWithHeaderStartingAtZero, params string[] expectedCells) 
		{
			XmlElement row = (XmlElement)rows[rowNumberWithHeaderStartingAtZero];
			XmlNodeList cells = row.GetElementsByTagName("td");
			AssertEquals("column count of row " + rowNumberWithHeaderStartingAtZero, expectedCells.Length, cells.Count);
			for (int i = 0; i < cells.Count; i++) 
			{
				XmlElement cell = (XmlElement)cells[i];
				string expectedCell = expectedCells[i];
				Assertion.Assert("expected cells shouldn't have any leading or trailing whitespace", expectedCell.Trim() == expectedCell);
				AssertEquals("column " + i + ", row " + rowNumberWithHeaderStartingAtZero, expectedCell, cell.InnerText.Trim());
			}
		}

	}
}
