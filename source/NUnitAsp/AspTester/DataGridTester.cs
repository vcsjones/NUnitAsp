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

namespace NUnit.Extensions.Asp.AspTester
{
	public class DataGridTester : ControlTester
	{
		public DataGridTester(string aspId, Control container) : base(aspId, container)
		{
		}

		public int RowCount 
		{
			get
			{
				return Element.GetElementsByTagName("tr").Count - 1;
			}
		}

		public string[][] TrimmedCells
		{
			get
			{
				string[][] result = new string[RowCount][];
				for (int i = 0; i < RowCount; i++)
				{
					result[i] = GetRow(i).TrimmedCells;
				}
				return result;
			}
		}

		public Row GetRow(int rowNumber)
		{
			return new Row(rowNumber, this);
		}

		public Row GetRowByCellValue(int columnNumber, string trimmedValue)
		{
			string[][] cells = TrimmedCells;
			for (int i = 0; i < cells.GetLength(0); i++)
			{
				if (cells[i][columnNumber] == trimmedValue) return GetRow(i);
			}
			Assertion.Fail(string.Format("Expected to find a row with '{0}' in column {1} of {2}", trimmedValue, columnNumber, HtmlIdAndDescription));
			throw new ApplicationException("This line cannot execute.  Fail() throws an exception.");
		}

		private XmlElement GetRowElement(int rowNumber)
		{
			XmlNodeList rows = Element.GetElementsByTagName("tr");
			return (XmlElement)rows[rowNumber + 1];
		}

		internal override string GetChildElementHtmlId(string aspId)
		{
			int rowNumber = int.Parse(aspId);
			return HtmlId + "__ctl" + (rowNumber + 2);
		}

		public class Row : ControlTester
		{
			private int rowNumber;
			private DataGridTester container;

			public Row(int rowNumber, DataGridTester container) : base(rowNumber.ToString(), container)
			{
				this.rowNumber = rowNumber;
				this.container = container;
			}

			internal override string GetChildElementHtmlId(string inAspId)
			{
				return HtmlId + "_" + inAspId;
			}

			internal override XmlElement Element
			{
				get
				{
					return container.GetRowElement(rowNumber);
				}
			}

			public string[] TrimmedCells
			{
				get
				{
					XmlNodeList cells = Element.GetElementsByTagName("td");
					string[] cellText = new string[cells.Count];
					for (int i = 0; i < cells.Count; i++) 
					{
						XmlElement cell = (XmlElement)cells[i];
						cellText[i] = cell.InnerText.Trim();
					}
					return cellText;
				}
			}
		}
	}
}
