namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;

	/// <summary>A fixture for Testing the "auto" test suite feature.</summary>
	public class SuiteTest : TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		protected TestResult fResult;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public SuiteTest(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		protected override void SetUp() 
		{
			fResult = new TestResult();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ITest Suite() 
		{
        
			TestSuite suite= new TestSuite("Suite Tests");
			// build the suite manually
			suite.AddTest(new SuiteTest("TestNoTestCaseClass"));
			suite.AddTest(new SuiteTest("TestNoTestCases"));
			suite.AddTest(new SuiteTest("TestOneTestCase"));
			suite.AddTest(new SuiteTest("TestNotPublicTestCase"));
			suite.AddTest(new SuiteTest("TestNotVoidTestCase"));
			suite.AddTest(new SuiteTest("TestNotExistingTestCase"));
			suite.AddTest(new SuiteTest("TestInheritedTests"));
			suite.AddTest(new SuiteTest("TestShadowedTests"));
			suite.AddTest(new SuiteTest("TestAddTestSuite"));
            
			return suite;
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAddTestSuite() 
		{
			TestSuite suite = new TestSuite();
			suite.AddTestSuite(typeof(OneTestCase));
			suite.Run(fResult);
			Assertion.AssertEquals(1, fResult.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
    
		public void TestInheritedTests() 
		{
			TestSuite suite= new TestSuite(typeof(InheritedTestCase));
			suite.Run(fResult);
			Assertion.Assert(fResult.WasSuccessful);
			Assertion.AssertEquals(2, fResult.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestNoTestCaseClass() 
		{
			ITest t = new TestSuite(typeof(NoTestCaseClass));
			t.Run(fResult);
			Assertion.AssertEquals(1, fResult.RunCount);  // warning test
			Assertion.Assert(! fResult.WasSuccessful);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestNoTestCases() 
		{
			ITest t= new TestSuite(typeof(NoTestCases));
			t.Run(fResult);
			Assertion.Assert(fResult.RunCount == 1);  // warning test
			Assertion.Assert(fResult.FailureCount == 1);
			Assertion.Assert(! fResult.WasSuccessful);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestNotExistingTestCase() 
		{
			ITest t= new SuiteTest("NotExistingMethod");
			t.Run(fResult);
			Assertion.Assert(fResult.RunCount == 1);  
			Assertion.Assert(fResult.FailureCount == 1);
			Assertion.Assert(fResult.ErrorCount == 0);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestNotPublicTestCase() 
		{
			TestSuite suite= new TestSuite(typeof(NotPublicTestCase));
			// 1 public test case + 1 warning for the non-public test case
			Assertion.AssertEquals(2, suite.CountTestCases);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestNotVoidTestCase() 
		{
			TestSuite suite= new TestSuite(typeof(NotVoidTestCase));
			Assertion.Assert(suite.CountTestCases == 1);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestOneTestCase() 
		{
			ITest t= new TestSuite(typeof(OneTestCase));
			t.Run(fResult);
			Assertion.Assert(fResult.RunCount == 1);  
			Assertion.Assert(fResult.FailureCount == 0);
			Assertion.Assert(fResult.ErrorCount == 0);
			Assertion.Assert(fResult.WasSuccessful);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestShadowedTests() 
		{
			TestSuite suite = new TestSuite(typeof(OverrideTestCase));
			suite.Run(fResult);
			Assertion.AssertEquals(1, fResult.RunCount);
		}
	}
}