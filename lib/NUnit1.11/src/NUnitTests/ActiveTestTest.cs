namespace NUnit.Tests 
{

	using System;
	using NUnit.Extensions;
	using NUnit.Framework;

	/// <summary>Testing the ActiveTest support.</summary>
	public class ActiveTestTest: TestCase 
	{

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
		public ActiveTestTest(String name) : base(name) {}

		ActiveTestSuite CreateActiveTestSuite() 
		{
			ActiveTestSuite suite = new ActiveTestSuite();
			for (int i=0; i < 100; i++)
				suite.AddTest(new SuccessTest("Success"));
			return suite;
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestActiveRepeatedTest() 
		{
			ITest test = new RepeatedTest(CreateActiveTestSuite(), 5);
			TestResult result = new TestResult();
			test.Run(result);
			Assertion.AssertEquals(500, result.RunCount);
			Assertion.AssertEquals(0, result.FailureCount);
			Assertion.AssertEquals(0, result.ErrorCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestActiveRepeatedTest0() 
		{
			ITest test = new RepeatedTest(CreateActiveTestSuite(), 0);
			TestResult result = new TestResult();
			test.Run(result);
			Assertion.AssertEquals(0, result.RunCount);
			Assertion.AssertEquals(0, result.FailureCount);
			Assertion.AssertEquals(0, result.ErrorCount);
		}
		/// <summary>
		///		
		/// </summary>
		public void TestActiveRepeatedTest1() 
		{
			ITest test = new RepeatedTest(CreateActiveTestSuite(), 1);
			TestResult result = new TestResult();
			test.Run(result);
			Assertion.AssertEquals(100, result.RunCount);
			Assertion.AssertEquals(0, result.FailureCount);
			Assertion.AssertEquals(0, result.ErrorCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestActiveTest() 
		{
			ITest test = CreateActiveTestSuite();
			TestResult result = new TestResult();
			test.Run(result);
			Assertion.AssertEquals(100, result.RunCount);
			Assertion.AssertEquals(0, result.FailureCount);
			Assertion.AssertEquals(0, result.ErrorCount);
		}
	}
}
