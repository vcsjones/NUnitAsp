using System;
using System.Globalization;

namespace NUnit.Extensions.Asp
{
	/// <summary>
	/// The data type of a column (use with AssertSortOrder)
	/// </summary>
	public enum DataType {String, DateTime, Int};
	
	
	/// <summary>
	/// Assertions specific to NUnitAsp's web testers.
	/// </summary>
	public class WebAssert
	{
		internal static void Fail(string message)
		{
			throw new WebAssertionException(message);
		}

		internal static void True(bool condition, string message)
		{
			if (!condition) Fail(message);
		}

		internal static void AreEqual(object expected, object actual, string message)
		{
			True(expected.Equals(actual), message + " expected '" + expected + "' but was '" + actual + "'");
		}

		internal static void NotNull(object o, string message)
		{
			True(o != null, message);
		}

		/// <summary>
		/// Asserts that a specific control is on the current web page, with the "Visible"
		/// parameter set to "true."  This method does not assert that the control is actually
		/// visible to the user.
		/// </summary>
		/// <param name="tester">The tester for the control to check.</param>
		public static void Visible(Tester tester)
		{
			Visibility(tester, true);
		}

		/// <summary>
		/// Asserts that a specific control is not on the current web page, or if it is,
		/// its "Visible" parameter is set to "false."  This method does not distinguish
		/// between non-existant controls and non-visible controls: use with caution.
		/// </summary>
		/// <param name="tester">The tester for the control to check.</param>
		public static void NotVisible(Tester tester)
		{
			Visibility(tester, false);
		}

		private static void Visibility(Tester tester, bool expectedVisibility)
		{
			string not = expectedVisibility ? " not" : "";
			string message = String.Format("Unexpectedly{0} visible: {1}", not, tester.HtmlIdAndDescription);
			True(tester.Visible == expectedVisibility, message);
		}

		/// <summary>
		/// Asserts that two "rows" of strings are identical.
		/// </summary>
		public static void AreEqual(string[] expected, string[] actual)
		{
			AreEqual(expected, actual, "");
		}

		/// <summary>
		/// Asserts that two "rows" of strings are identical.
		/// </summary>
		public static void AreEqual(string[] expected, string[] actual, string message)
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
		public static void AreEqual(string[][] expected, string[][] actual)
		{
			AreEqual(expected, actual, "");
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical.
		/// </summary>
		[CLSCompliant(false)]
		public static void AreEqual(string[][] expected, string[][] actual, string message)
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
		public static void AreEqualIgnoringOrder(string[][] expected, string[][] actual)
		{
			AreEqualIgnoringOrder(expected, actual, "");
		}

		/// <summary>
		/// Asserts that two "tables" of strings are identical, but permits ordering
		/// differences.  Individual rows in the tables must match, but the order of the
		/// rows may differ.
		/// </summary>
		[CLSCompliant(false)]
		public static void AreEqualIgnoringOrder(string[][] expected, string[][] actual, string message)
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

			string joiner = "";
			string result = "{";
			foreach (string element in a)
			{
				result += joiner + "\"" + element + "\"";
				joiner = ", ";
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
		/// <param name="data">The table to check.</param>
		/// <param name="column">The column that must be sorted.</param>
		/// <param name="isAscending">'true' if the table should be sorted from low to high; 'false' if the table should be sorted from high to low.</param>
		/// <param name="type">The type of data in the column that's sorted.</param>
		[CLSCompliant(false)]
		public static void Sorted(string[][] data, int column, bool isAscending, DataType type)
		{
			Sorted(data, column, isAscending, type, "table");
		}

		/// <summary>
		/// Asserts that the "rows" in a "table" of strings are sorted.
		/// </summary>
		/// <param name="data">The table to check.</param>
		/// <param name="column">The column that must be sorted.</param>
		/// <param name="isAscending">'true' if the table should be sorted from low to high; 'false' if the table should be sorted from high to low.</param>
		/// <param name="type">The type of data in the column that's sorted.</param>
		/// <param name="message">A noun to display if the assertion fails.</param>
		[CLSCompliant(false)]
		public static void Sorted(string[][] data, int column, bool isAscending, DataType type, string message)
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

		/// <summary>
		/// Asserts that a "table" of strings contains a particular "row."
		/// </summary>
		/// <param name="table">The table to check.</param>
		/// <param name="expectedRow">The row that the table should contain.</param>
		[CLSCompliant(false)]
		public static void TableContainsRow(string[][] table, params string[] expectedRow)
		{
			foreach (string[] row in table)
			{
				if (ArraysEqual(expectedRow, row)) return;
			}
			Fail(String.Format(
				"\nExpected table to contain row:\n   {0}\nactual table was:{1}",
				RenderArray(expectedRow),
				RenderDoubleArray(table)
			));
		}
	}


	/// <summary>
	/// An assumption was violated, either in your tests or in NUnitAsp
	/// itself.  When you get this exception, look at the exception message
	/// for more information.
	/// </summary>
	public class WebAssertionException : ApplicationException
	{
		public WebAssertionException(string message) : base(message)
		{
		}
	}
}
