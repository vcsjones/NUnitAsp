namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;

	/// <summary>
	/// Test an implementor of NUnit.Framework.ITest other 
	/// than TestCase or TestSuite
	/// </summary>
	public class TestImplementorTest:  TestCase 
	{
#if(false)
    public class SuccessTest: TestCase {
      public SuccessTest(String name) : base(name) {}
      public void Success() {}
    }
#endif
		/// <summary>
		/// 
		/// </summary>
		public class DoubleTestCase: ITest 
		{
			private TestCase fTestCase;
			/// <summary>
			/// 
			/// </summary>
			/// <param name="testCase"></param>
			public DoubleTestCase(TestCase testCase) 
			{
				fTestCase= testCase;
			}
			/// <summary>
			/// 
			/// </summary>
			public int CountTestCases 
			{
				get { return 2; }
			}
			/// <summary>
			/// 
			/// </summary>
	
			public class ProtectedClass: IProtectable 
			{
				TestCase fTestCase;
				/// <summary>
				/// 
				/// </summary>
				/// <param name="testCase"></param>
				public ProtectedClass(TestCase testCase) 
				{
					fTestCase = testCase;
				}
				/// <summary>
				/// 
				/// </summary>
				public void Protect() 
				{
					fTestCase.RunBare();
					fTestCase.RunBare();
				}
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="result"></param>
			public void Run(TestResult result) 
			{
				result.StartTest(this);
				IProtectable p= new ProtectedClass(fTestCase);
				result.RunProtected(this, p);
				result.EndTest(this);
			}

		}
	
		private DoubleTestCase fTest;
		/// <summary>
		/// 
		/// </summary>
		public class NoOpTest : TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			public NoOpTest(string name) : base(name) {}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() {}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public TestImplementorTest(string name) : base(name) 
		{
			TestCase testCase= new NoOpTest("noop");
			fTest= new DoubleTestCase(testCase);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestSuccessfulRun() 
		{
			TestResult result= new TestResult();
			fTest.Run(result);
			Assertion.AssertEquals(fTest.CountTestCases, result.RunCount);
			Assertion.AssertEquals(0, result.ErrorCount);
			Assertion.AssertEquals(0, result.FailureCount);
		}
	}
}
