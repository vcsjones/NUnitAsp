using System;
using System.Xml;
using NUnit.Framework;

namespace NUnit.Extensions.Asp
{

	public class DataGrid : XhtmlElement
	{

		private XmlNodeList rows;
		private string htmlId;

		internal DataGrid(Browser browser, XmlElement element, string id, string htmlId, string containerDescription)
			: base(browser, element, id, containerDescription)
		{
			this.htmlId = htmlId;
			rows = Element.GetElementsByTagName("tr");
		}

		public LinkButton LinkButton(int rowNumberWithHeaderStartingAtZero, string buttonId) 
		{
			string id = string.Format("{0}__ctl{1}_{2}", htmlId, rowNumberWithHeaderStartingAtZero + 1, buttonId);
			string message = string.Format("Element '{0}' (aka '{1}') should be on {2}", buttonId, id, Description);
			XmlElement linkButton = Element.OwnerDocument.GetElementById(id);
			Assertion.Assert(message, linkButton != null);
			return new LinkButton(Browser, linkButton, id, Description);
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
