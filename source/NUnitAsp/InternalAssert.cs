using System;

namespace NUnit.Extensions.Asp
{
	internal class InternalAssert
	{
		internal static void Fail(string message)
		{
			throw new InternalAssertionException(message);
		}

		internal static void True(bool condition, string message)
		{
			if (!condition) Fail(message);
		}

		internal static void Equal(object expected, object actual, string message)
		{
			True(expected.Equals(actual), message + " expected '" + expected + "' but was '" + actual + "'");
		}

		internal static void NotNull(object o, string message)
		{
			True(o != null, message);
		}
	}

	/// <summary>
	/// You tried to do something that NUnitAsp doesn't allow.  Look at the
	/// text of the exception for more information.
	/// </summary>
	public class InternalAssertionException : ApplicationException
	{
		public InternalAssertionException(string message) : base(message)
		{
		}
	}
}
