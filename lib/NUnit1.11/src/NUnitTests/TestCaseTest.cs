namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;

	/// <summary>
	/// A test case testing the testing framework.
	/// </summary>
	public class TestCaseTest: TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		public class TornDown: TestCase 
		{
			internal bool fTornDown= false;
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			protected internal TornDown(String name) : base(name)
			{
			}
			/// <summary>
			/// 
			/// </summary>
			protected override void TearDown() 
			{
				fTornDown= true;
			}
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
		/// <param name="name"></param>
		public TestCaseTest(String name) : base(name){}
		/// <summary>
		/// 
		/// </summary>
		public void TestCaseToString() 
		{
			// This test wins the award for twisted snake tail eating while
			// writing self Tests. And you thought those weird anonymous
			// inner classes were bad...
			Assertion.AssertEquals("TestCaseToString(NUnit.Tests.TestCaseTest)", ToString());
		}
		/// <summary>
		/// 
		/// </summary>
		public class ErrorTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public ErrorTestCase(): base("Error") {}
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
			TestCase error= new ErrorTestCase();
			VerifyError(error);
		}
		/// <summary>
		/// 
		/// </summary>
		public class FailureTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public FailureTestCase() : base ("Failure") {}
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
			TestCase failure= new FailureTestCase();
			VerifyFailure(failure);
		}
		/// <summary>
		/// 
		/// </summary>
		public class FailsTornDown: TornDown 
		{
			/// <summary>
			/// 
			/// </summary>
			public FailsTornDown() : base("fails") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void TearDown() 
			{
				base.TearDown();
				throw new SystemException();
			}
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
		public void TestRunAndTearDownFails() 
		{
			TornDown fails= new FailsTornDown();
			VerifyError(fails);
			Assertion.Assert(fails.fTornDown);
		}
		/// <summary>
		/// 
		/// </summary>
		public class SetupFailsTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public SetupFailsTestCase() : base("success") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void SetUp() 
			{
				throw new SystemException();
			}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() {}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestSetupFails() 
		{
			TestCase fails= new SetupFailsTestCase();
			VerifyError(fails);
		}
		/// <summary>
		/// 
		/// </summary>
		public class SuccessTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public SuccessTestCase() : base ("sucess") { }
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() {}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestSuccess() 
		{
			TestCase success= new SuccessTestCase();
			VerifySuccess(success);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestTearDownAfterError() 
		{
			TornDown fails= new TornDown("fails");
			VerifyError(fails);
			Assertion.Assert(fails.fTornDown);
		}
		/// <summary>
		/// 
		/// </summary>
		protected class TearDownFailsTestCase : TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public TearDownFailsTestCase() : base ("success") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void TearDown() 
			{
				throw new SystemException();
			}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() { }
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestTearDownFails() 
		{
			TestCase fails= new TearDownFailsTestCase();
			VerifyError(fails);
		}
		/// <summary>
		/// 
		/// </summary>
		protected class TearDownSetupFailsTestCase: TornDown 
		{
			/// <summary>
			/// 
			/// </summary>
			public TearDownSetupFailsTestCase() : base ("fails") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void SetUp() 
			{
				throw new SystemException();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestTearDownSetupFails() 
		{
			TornDown fails= new TearDownSetupFailsTestCase();
			VerifyError(fails);
			Assertion.Assert(!fails.fTornDown);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestWasRun() 
		{
			WasRun test= new WasRun("");
			test.Run();
			Assertion.Assert(test.fWasRun);
		}
		private void VerifyError(TestCase test) 
		{
			TestResult result= test.Run();
			Assertion.Assert(result.RunCount == 1);
			Assertion.Assert(result.FailureCount == 0);
			Assertion.Assert(result.ErrorCount == 1);
		}
		private void VerifyFailure(TestCase test) 
		{
			TestResult result= test.Run();
			Assertion.Assert(result.RunCount == 1);
			Assertion.Assert(result.FailureCount == 1);
			Assertion.Assert(result.ErrorCount == 0);
		}
		private void VerifySuccess(TestCase test) 
		{
			TestResult result= test.Run();
			Assertion.Assert(result.RunCount == 1);
			Assertion.Assert(result.FailureCount == 0);
			Assertion.Assert(result.ErrorCount == 0);
		}
	}
}
