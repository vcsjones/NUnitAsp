namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;
	using NUnit.Runner;

	/// <summary>TestSuite that runs all the sample Tests.</summary>
	public class AllTests 
	{
		/// <summary>
		/// 
		/// </summary>
		public static ITest Suite 
		{
			get 
			{
				TestSuite suite = new TestSuite(typeof(AllTests).FullName);
				suite.AddTestSuite(typeof(ExtensionTest));
				suite.AddTestSuite(typeof(TestCaseTest));
				// Tests suite building, so can't use automatic suite extraction
				suite.AddTest(SuiteTest.Suite());
				suite.AddTestSuite(typeof(ExceptionTestCaseTest));
				suite.AddTestSuite(typeof(ExpectExceptionTest));
				//suite.AddTestSuite(typeof(NUnit.Tests.Failure)); // for unloading test only
				suite.AddTestSuite(typeof(TestListenerTest));
				suite.AddTestSuite(typeof(ActiveTestTest));
				suite.AddTestSuite(typeof(AssertionTest));
				// suite.AddTestSuite(typeof(StackFilterTest));
				// suite.AddTestSuite(typeof(SorterTest));
				suite.AddTestSuite(typeof(RepeatedTestTest));
				suite.AddTestSuite(typeof(TestImplementorTest));
				// suite.AddTestSuite(typeof(TextRunnerTest));
				return suite;
			}
		}
	}
}
