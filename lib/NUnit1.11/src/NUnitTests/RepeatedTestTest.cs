namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;
	using NUnit.Extensions;

	/// <summary>
	/// Testing the RepeatedTest support.
	/// </summary>
	public class RepeatedTestTest: TestCase 
	{
		private TestSuite fSuite;
		/// <summary>
		/// 
		/// </summary>
		public class SuccessTest: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			public SuccessTest(String name) : base(name) {}
			/// <summary>
			/// 
			/// </summary>
			public void Success() {}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RepeatedTestTest(string name) : base(name) 
		{
			fSuite= new TestSuite();
			fSuite.AddTest(new SuccessTest("success"));
			fSuite.AddTest(new SuccessTest("success"));
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedOnce() 
		{
			ITest test= new RepeatedTest(fSuite, 1);
			Assertion.AssertEquals(2, test.CountTestCases);
			TestResult result= new TestResult();
			test.Run(result);
			Assertion.AssertEquals(2, result.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedMoreThanOnce() 
		{
			ITest test= new RepeatedTest(fSuite, 3);
			Assertion.AssertEquals(6, test.CountTestCases);
			TestResult result= new TestResult();
			test.Run(result);
			Assertion.AssertEquals(6, result.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedZero() 
		{
			ITest test= new RepeatedTest(fSuite, 0);
			Assertion.AssertEquals(0, test.CountTestCases);
			TestResult result= new TestResult();
			test.Run(result);
			Assertion.AssertEquals(0, result.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedNegative() 
		{
			try 
			{
				ITest test= new RepeatedTest(fSuite, -1);
			} 
			catch (ArgumentOutOfRangeException) 
			{
				return;
			}
			Assertion.Fail("Should throw an ArgumentOutOfRangeException");
		}
	}
}