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
using NUnit.Extensions.Asp.AspTester;
using System.Globalization;

namespace NUnit.Extensions.Asp 
{
	public enum DataType {String, DateTime, Int};
 
	/// <summary>
	/// An ASP.NET test case.  Extend this clss from your test cases to use NUnitAsp
	/// in your tests.
	/// </summary>
	public abstract class WebFormTestCase : TestCase 
	{
		private HttpClient browser;
		private WebForm form;

		public WebFormTestCase(string name) : base(name) 
		{
		}

		/// <summary>
		/// Executed before each test method is run.  If you override this method, BE
		/// SURE to call <code>base.SetUp()</code>.  Otherwise your tests won't work.
		/// </summary>
		protected override void SetUp() 
		{
			base.SetUp();
			browser = new HttpClient();
			form = new WebForm(browser);
   		}

		/// <summary>
		/// The web form currently loaded by the browser.
		/// </summary>
		protected WebForm CurrentWebForm
		{
			get 
			{
				return form;
			}
		}

		/// <summary>
		/// The web browser.
		/// </summary>
		protected HttpClient Browser 
		{
			get 
			{
				return browser;
			}
		}

		/// <summary>
		/// Asserts that a specific tester is on the current web page, with the "Visible"
		/// parameter set to "true."  This method does not assert that the tester is actually
		/// visible to the user.
		/// </summary>
		protected static void AssertVisibility(ControlTester tester, bool expectedVisibility)
		{
			string not = expectedVisibility ? "" : " not";
			string message = String.Format("{0} control should{1} be visible (HTML ID: {2}; ASP location: {3})", tester.AspId, not, tester.HtmlId, tester.Description);
			Assert(message, tester.Visible == expectedVisibility);
		}

		/// <summary>
		/// Asserts that two "rows" of strings are identical.
		/// </summary>
		protected static void AssertEquals(string[] expected, string[] actual)
		{
			AssertEquals(Flatten(expected), Flatten(actual));
		}

		/// <summary>
		/// Asserts that two "rows" of strings are identical.
		/// </summary>
		protected static void AssertEquals(string message, string[] expected, string[] actual)
		{
			Assertion.AssertEquals(message, Flatten(expected), Flatten(actual));
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical.
		/// </summary>
		protected static void AssertEquals(string[][] expected, string[][] actual)
		{
			AssertEquals(Flatten(expected), Flatten(actual));
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical.
		/// </summary>
		protected static void AssertEquals(string message, string[][] expected, string[][] actual)
		{
			AssertEquals(Flatten(expected), Flatten(actual));
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical, but permits ordering
		/// differences.  Individual rows in the tables must match, but the order of the
		/// rows may differ.
		/// </summary>
		protected static void AssertEqualsIgnoreOrder(string message, string[][] expected, string[][] actual)
		{
			if (expected.Length != actual.Length) Fail(message, expected, actual);

			foreach (string[] row in actual)
			{
				AssertTableContainsRow(message, expected, actual, row);
			}
		}

		private static void AssertTableContainsRow(string message, string[][] expected, string[][] actual, string[] actualRow)
		{
			foreach (string[] expectedRow in expected)
			{
				if (Flatten(expectedRow) == Flatten(actualRow)) return;
			}
			Fail(message, expected, actual);
		}

		private static void Fail(string message, string[][] expected, string[][] actual)
		{
			AssertEquals(message, expected, actual);
		}
		
		private static string Flatten(string[] a)
		{
			string result = "{";
			foreach (string element in a)
			{
				result += "<" + element + ">";
			}
			return result + "}";
		}

		private static string Flatten(string[][] a)
		{
			string result = "{";
			foreach (string[] element in a)
			{
				result += "\n   " + Flatten(element);
			}
			return result + "\n}";
		}

		/// <summary>
		/// Asserts that the "rows" in a "table" of strings are sorted.
		/// </summary>
		/// <param name="message">A noun to display if the assertion fails.</param>
		/// <param name="data">The table to check.</param>
		/// <param name="column">The column that must be sorted.</param>
		/// <param name="isAscending">'true' if the table should be sorted from low to high; 'false' if the table should be sorted from high to low.</param>
		/// <param name="type">The type of data in the column that's sorted.</param>
		public static void AssertSortOrder(string message, string[][] data, int column, bool isAscending, DataType type)
		{
			string lastCell = null;
			foreach (string[] row in data)
			{
				string cell = row[column];
				if (lastCell == null) 
				{
					lastCell = cell;
					continue;
				}

				bool sorted;
				string orderName;
				int comparison = Compare(cell, lastCell, type);
				if (isAscending)
				{
					sorted = comparison >= 0;
					orderName = "ascending";
				}
				else
				{
					sorted = comparison <= 0;
					orderName = "descending";
				}
				if (!sorted) Fail(message + " should be sorted " + orderName + ".  Was: " + Flatten(data));
				lastCell = cell;
			}
		}

		private static int Compare(string a, string b, DataType type)
		{
			if (a == "" && b == "") return 0;
			if (a == "") return -1;
			if (b == "") return 1;

			switch (type)
			{
				case DataType.String:
					return a.CompareTo(b);
				case DataType.Int:
					if (a == "" && b == "") return 0;
					int aInt = int.Parse(a);
					int bInt = int.Parse(b);
					return aInt.CompareTo(bInt);
				case DataType.DateTime:
					IFormatProvider formatter = CultureInfo.InvariantCulture.DateTimeFormat;
					DateTime aDate = DateTime.Parse(a, formatter);
					DateTime bDate = DateTime.Parse(b, formatter);
					return aDate.CompareTo(bDate);
				default:
					throw new ApplicationException("Unknown data type comparison: " + type);
			}
		}
	}
}