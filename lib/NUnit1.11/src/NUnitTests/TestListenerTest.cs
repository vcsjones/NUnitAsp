namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;

	/// <summary>Test class used in SuiteTest.</summary>
	public class TestListenerTest: TestCase, ITestListener 
	{
		private TestResult fResult;
		private int fStartCount;
		private int fEndCount;
		private int fFailureCount;
		private int fErrorCount;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public TestListenerTest(String name): base(name) {}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="test"></param>
		/// <param name="t"></param>
		public void AddError(ITest test, Exception t) 
		{
			fErrorCount++;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="test"></param>
		/// <param name="t"></param>
		public void AddFailure(ITest test, AssertionFailedError t) 
		{
			fFailureCount++;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="test"></param>
		public void EndTest(ITest test) 
		{
			fEndCount++;
		}
		/// <summary>
		/// 
		/// </summary>
		protected override void SetUp() 
		{
			fResult= new TestResult();
			fResult.AddListener(this);
            
			fStartCount= 0;
			fEndCount= 0;
			fFailureCount= 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="test"></param>
		public void StartTest(ITest test) 
		{
			fStartCount++;
		}
		/// <summary>
		/// 
		/// </summary>
		public class ErrorTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public ErrorTestCase() : base("noop") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() 
			{
				throw new SystemException();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestError() 
		{
			TestCase test= new ErrorTestCase();
			test.Run(fResult);
			Assertion.AssertEquals(1, fErrorCount);
			Assertion.AssertEquals(1, fEndCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public class FailTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public FailTestCase() : base("noop") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() 
			{
				Assertion.Fail();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestFailure() 
		{
			TestCase test= new FailTestCase();
			test.Run(fResult);
			Assertion.AssertEquals(1, fFailureCount);
			Assertion.AssertEquals(1, fEndCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public class NoOpTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public NoOpTestCase() : base("noop") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() 
			{
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestStartStop() 
		{
			TestCase test= new NoOpTestCase();
			test.Run(fResult);
			Assertion.AssertEquals(1, fStartCount);
			Assertion.AssertEquals(1, fEndCount);
		}
	}
}