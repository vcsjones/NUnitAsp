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

namespace NUnit.Extensions.Asp 
{
	public abstract class WebFormTestCase : TestCase 
	{
		private HttpClient browser;
		private WebForm form;

		public WebFormTestCase(string name) : base(name) 
		{
		}

		protected override void SetUp() 
		{
			base.SetUp();
			browser = new HttpClient();
			form = new WebForm(browser);
   		}

		protected WebForm CurrentWebForm
		{
			get 
			{
				return form;
			}
		}

		protected HttpClient Browser 
		{
			get 
			{
				return browser;
			}
		}

		protected static void AssertVisibility(ControlTester tester, bool expectedVisibility)
		{
			string not = expectedVisibility ? "" : " not";
			string message = String.Format("{0} control should{1} be visible (HTML ID: {2}; ASP location: {3})", tester.AspId, not, tester.HtmlId, tester.Description);
			Assert(message, tester.Visible == expectedVisibility);
		}

		protected static void AssertEquals(string[] expected, string[] actual)
		{
			AssertEquals(Flatten(expected), Flatten(actual));
		}

		protected static void AssertEquals(string message, string[] expected, string[] actual)
		{
			Assertion.AssertEquals(message, Flatten(expected), Flatten(actual));
		}

		protected static void AssertEquals(string[][] expected, string[][] actual)
		{
			AssertEquals(Flatten(expected), Flatten(actual));
		}

		protected static void AssertEquals(string message, string[][] expected, string[][] actual)
		{
			AssertEquals(Flatten(expected), Flatten(actual));
		}

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
		/// Null strings in data[][] are not allowed.
		/// </summary>
		public static void AssertSortOrder(string message, string[][] data, int column, bool isAscending)
		{
			string lastCell = null;
			foreach (string[] row in data)
			{
				string cell = row[column];
				int comparison = cell.CompareTo(lastCell);

				bool sorted;
				string orderName;
				if (isAscending)
				{
					sorted = (lastCell == null) || (comparison >= 0);
					orderName = "ascending";
				}
				else
				{
					sorted = (lastCell == null) || (comparison <= 0);
					orderName = "descending";
				}
				if (!sorted) Fail(message + " should be sorted " + orderName + ".  Was: " + Flatten(data));
				lastCell = cell;
			}
		}
	}
}