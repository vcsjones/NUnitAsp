namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;
	/// <summary>
	/// 
	/// </summary>
	public class ExpectExceptionTest : TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ExpectExceptionTest(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		[ExpectException(typeof(Exception))]
		public void TestSingle()
		{
			throw new Exception("single exception");
		}
		/// <summary>
		/// 
		/// </summary>
		[ExpectException(typeof(ArgumentException))]
		[ExpectException(typeof(SystemException))] 
		public void TestList()
		{
			throw new ArgumentException("List of exceptions");
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestInvalid()
		{
			TestSuite suite = new TestSuite(typeof(ExpectExceptionTest.InvalidExceptionTests));
			TestResult result= new TestResult();
			suite.Run(result);
			Assertion.AssertEquals(2, result.RunCount);
			Assertion.AssertEquals(2, result.FailureCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public class InvalidExceptionTests : TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			public InvalidExceptionTests(string name) : base(name) {}
			/// <summary>
			/// 
			/// </summary>
			[ExpectException(typeof(ArgumentException))]
			public void Test() 
			{
				;
			}
			/// <summary>
			/// 
			/// </summary>
			[ExpectException(typeof(ArgumentException))]
			public void TestWrongException() 
			{
				throw new IndexOutOfRangeException("wrong exception");
			}
		}
	}
}
