#region Copyright (c) 2002, 2003 by Brian Knowles and Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003 by Brian Knowles and Jim Shore
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
#endregion

#region Instructions
/********************************************************************************************************************
 * 
 * This file allows NUnitAsp to be used with NUnit.  To use, copy this file 
 * into your test project.  For additional information, see the NUnitAsp
 * documentation in your download package, or visit
 * http://nunitasp.sourceforge.net.
 * 
 *******************************************************************************************************************/
#endregion

using System;
using NUnit.Framework;
using System.Xml;
using NUnit.Extensions.Asp.AspTester;
using System.Globalization;

namespace NUnit.Extensions.Asp 
{ 
	/// <summary>
	/// Base class for NUnitAsp test fixtures.  Extend this class to use NUnitAsp.
	/// </summary>
	[TestFixture]
	public abstract class WebFormTestCase : WebAssertion
	{
		private bool setupCalled = false;

		/// <summary>
		/// Do not call.  For use by NUnit only.
		/// </summary>
		[SetUp]
		public void MasterSetUp() 
		{
			setupCalled = true;
			HttpClient.Default = new HttpClient();
			SetUp();
		}

		/// <summary>
		/// Executed before each test method is run.  Override in subclasses to do subclass
		/// set up.  NOTE: The [SetUp] attribute cannot be used in subclasses because it is already
		/// in use.
		/// </summary>
		protected virtual void SetUp()
		{
		}

		/// <summary>
		/// Do not call.  For use by NUnit only.
		/// </summary>
		[TearDown]
		public void MasterTearDown()
		{
			TearDown();
		}

		/// <summary>
		/// Executed after each test method is run.  Override in subclasses to do subclass
		/// clean up.  NOTE: [TearDown] attribute cannot be used in subclasses because it is
		/// already in use.
		/// </summary>
		protected virtual void TearDown()
		{
		}

		/// <summary>
		/// The web form currently loaded by the browser.
		/// </summary>
		protected WebForm CurrentWebForm
		{
			get 
			{
				AssertSetUp();
				return new WebForm(HttpClient.Default);
			}
		}

		/// <summary>
		/// The web browser.
		/// </summary>
		protected HttpClient Browser 
		{
			get 
			{
				AssertSetUp();
				return HttpClient.Default;
			}
		}

		private void AssertSetUp()
		{
			if (!setupCalled) 
			{
				Fail("A required setup method in WebFormTestCase was not called.  This is probably because you used the [SetUp] attribute in a subclass of WebFormTestCase.  That is not supported.  Override the SetUp() method instead.");
			}
		}
	}


	/// <summary>
	/// The data type of a column (use with AssertSortOrder)
	/// </summary>
	public enum DataType {String, DateTime, Int};

	/// <summary>
	/// Assertions specific to NUnitAsp's web testers.
	/// </summary>
	public class WebAssertion : Assertion
	{
		/// <summary>
		/// Asserts that a specific control is on the current web page, with the "Visible"
		/// parameter set to "true."  This method does not assert that the control is actually
		/// visible to the user.
		/// </summary>
		public static void AssertVisibility(ControlTester tester, bool expectedVisibility)
		{
			string not = expectedVisibility ? "" : " not";
			string message = String.Format("{0} control should{1} be visible (HTML ID: {2}; ASP location: {3})", tester.AspId, not, tester.HtmlId, tester.Description);
			Assert(message, tester.Visible == expectedVisibility);
		}

		/// <summary>
		/// Asserts that two "rows" of strings are identical.
		/// </summary>
		public static void AssertEquals(string[] expected, string[] actual)
		{
			AssertEquals("", expected, actual);
		}

		/// <summary>
		/// Asserts that two "rows" of strings are identical.
		/// </summary>
		public static void AssertEquals(string message, string[] expected, string[] actual)
		{
			if (expected == null && actual == null) return;
			if (actual == null || expected == null) Fail(message, expected, actual);
			if (expected.Length != actual.Length) Fail(message, expected, actual);
			if (!ArraysEqual(expected, actual)) Fail(message, expected, actual);
		}

		private static bool ArraysEqual(string[] expected, string[] actual)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				if (expected[i] != actual[i]) return false;
			}
			return true;
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical.
		/// </summary>
		[CLSCompliant(false)]
		public static void AssertEquals(string[][] expected, string[][] actual)
		{
			AssertEquals("", expected, actual);
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical.
		/// </summary>
		[CLSCompliant(false)]
		public static void AssertEquals(string message, string[][] expected, string[][] actual)
		{
			if (expected == null && actual == null) return;
			if (actual == null || expected == null) Fail(message, expected, actual);
			if (expected.Length != actual.Length) Fail(message, expected, actual);

			for (int i = 0; i < expected.Length; i++)
			{
				if (!ArraysEqual(expected[i], actual[i])) Fail(message, expected, actual);
			}
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical, but permits ordering
		/// differences.  Individual rows in the tables must match, but the order of the
		/// rows may differ.
		/// </summary>
		[CLSCompliant(false)]
		public static void AssertEqualsIgnoreOrder(string message, string[][] expected, string[][] actual)
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

		private static void Fail(string message, string[] expected, string[] actual)
		{
			message += "\r\nexpected: " + RenderArray(expected);
			message += "\r\n but was: " + RenderArray(actual);
			Fail(message);
		}

		private static string RenderArray(string[] array)
		{
			if (array == null) return "<null>";
			if (array.Length == 0) return "{}";

			return "{\"" + string.Join("\", \"", array) + "\"}";
		}

		private static void Fail(string message, string[][] expected, string[][] actual)
		{
			message += "\r\nexpected: " + RenderDoubleArray(expected);
			message += "\r\n but was: " + RenderDoubleArray(actual);
			Fail(message);
		}

		private static string RenderDoubleArray(string[][] doubleArray)
		{
			if (doubleArray == null) return "<null>";
			if (doubleArray.Length == 0) return "{}";

			string result = "\r\n   {";
			foreach (string[] array in doubleArray)
			{
				result += "\r\n      " + RenderArray(array);
			}
			return result + "\r\n   }";
		}

		private static string Flatten(string[] a)
		{
			if (a == null) return "null";

			string result = "{";
			foreach (string element in a)
			{
				result += "<" + element + ">";
			}
			return result + "}";
		}

		private static string Flatten(string[][] a)
		{
			if (a == null) return "null";

			string result = "{";
			foreach (string[] element in a)
			{
				result += "\r\n   " + Flatten(element);
			}
			return result + "\r\n}";
		}

		/// <summary>
		/// Asserts that the "rows" in a "table" of strings are sorted.
		/// </summary>
		/// <param name="message">A noun to display if the assertion fails.</param>
		/// <param name="data">The table to check.</param>
		/// <param name="column">The column that must be sorted.</param>
		/// <param name="isAscending">'true' if the table should be sorted from low to high; 'false' if the table should be sorted from high to low.</param>
		/// <param name="type">The type of data in the column that's sorted.</param>
		[CLSCompliant(false)]
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