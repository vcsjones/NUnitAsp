namespace NUnit.Tests 
{
  
	using System;
  
	using NUnit.Extensions;
	using NUnit.Framework;
	/// <summary>
	/// 
	/// </summary>
	public class ExceptionTestCaseTest: NUnit.Framework.TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ExceptionTestCaseTest(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		protected class ThrowExceptionTestCase: ExceptionTestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			/// <param name="exception"></param>
			public ThrowExceptionTestCase(String name, Type exception)
				: base(name, exception) {}
			/// <summary>
			/// 
			/// </summary>
			public void Test() 
			{
				throw new IndexOutOfRangeException();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected class ThrowRuntimeExceptionTestCase: ExceptionTestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			/// <param name="exception"></param>
			public ThrowRuntimeExceptionTestCase(String name, Type exception)
				: base(name, exception) {}
			/// <summary>
			/// 
			/// </summary>
			public void Test() 
			{
				throw new SystemException();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected class ThrowNoExceptionTestCase: ExceptionTestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			/// <param name="exception"></param>
			public ThrowNoExceptionTestCase(String name, Type exception)
				: base(name, exception) {}
			/// <summary>
			/// 
			/// </summary>
			public void Test() 
			{
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestExceptionSubclass() 
		{
			ExceptionTestCase test=
				new ThrowExceptionTestCase("Test", typeof(Exception));
			TestResult result= test.Run();
			Assertion.AssertEquals(1, result.RunCount);
			Assertion.Assert(result.WasSuccessful);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestExceptionTest() 
		{
			ExceptionTestCase test=
				new ThrowExceptionTestCase("Test", typeof(IndexOutOfRangeException));
			TestResult result= test.Run();
			Assertion.AssertEquals(1, result.RunCount);
			Assertion.Assert(result.WasSuccessful);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestFailure() 
		{
			ExceptionTestCase test=
				new ThrowRuntimeExceptionTestCase("Test",
				typeof(IndexOutOfRangeException));
			TestResult result= test.Run();
			Assertion.AssertEquals(1, result.RunCount);
			Assertion.AssertEquals(1, result.ErrorCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestNoException() 
		{
			ExceptionTestCase test=
				new ThrowNoExceptionTestCase("Test", typeof(Exception));
			TestResult result= test.Run();
			Assertion.AssertEquals(1, result.RunCount);
			Assertion.AssertEquals(1, result.FailureCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestWrongException() 
		{
			ExceptionTestCase test=
				new ThrowRuntimeExceptionTestCase("Test",
				typeof(IndexOutOfRangeException));
			TestResult result= test.Run();
			Assertion.AssertEquals(1, result.RunCount);
			Assertion.AssertEquals(1, result.ErrorCount);
		}
	}
}