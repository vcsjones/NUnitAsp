namespace NUnit.Tests 
{
	using System;
	using NUnit.Extensions;
	using NUnit.Framework;

	/// <summary>
	/// A test case testing the extensions to the Testing framework.
	/// </summary>
	public class ExtensionTest: TestCase 
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ExtensionTest(String name) : base(name) {}
		#endregion

		#region Test Methods
		/// <summary>
		/// 
		/// </summary>
		public void TestRunningErrorInTestSetup() 
		{
			TestSetup wrapper= new TestSetup(new RunningSetupFailure());
			TestResult result= new TestResult();
			wrapper.Run(result);
			Assertion.Assert(!result.WasSuccessful);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRunningErrorsInTestSetup() 
		{
			TestSuite suite = new TestSuite();
			TestResult result = new TestResult();
			
			suite.AddTest(new RunningSetupFailure());
			suite.AddTest(new RunningSetupError());
            
			TestSetup wrapper = new TestSetup(suite);
            wrapper.Run(result);
            
			Assertion.AssertEquals("FailureCount.", 1, result.FailureCount);
			Assertion.AssertEquals("ErrorCount.", 1, result.ErrorCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestSetupErrorDontTearDown() 
		{
			WasRun test= new WasRun("");
                
			TornDown wrapper= new TornDownTest(test);

			TestResult result= new TestResult();
			wrapper.Run(result);

			Assertion.Assert(!wrapper.fTornDown);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestSetupErrorInTestSetup() 
		{
			WasRun test= new WasRun("");
			TestSetup wrapper= new SetupFailTest(test);

			TestResult result= new TestResult();
			wrapper.Run(result);

			Assertion.Assert(!test.fWasRun);
			Assertion.Assert(!result.WasSuccessful);
		}
		#endregion

		#region Nested classes
		private class TornDown: TestSetup
		{
			/// <summary>
			/// 
			/// </summary>
			protected internal bool fTornDown= false;
			/// <summary>
			/// 
			/// </summary>
			/// <param name="test"></param>
			protected TornDown(ITest test) : base(test) {}
			/// <summary>
			/// 
			/// </summary>
			protected override void TearDown() 
			{
				fTornDown= true;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public class RunningSetupFailure: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public RunningSetupFailure() : base("RunningSetupFailure") {}
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
		class RunningSetupError: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public RunningSetupError(): base("RunningSetupError") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() 
			{
				throw new SystemException();
			}
		}
		private class TornDownTest: TornDown 
		{
			public TornDownTest(ITest test) : base(test) {}

			protected override void SetUp() 
			{
				Assertion.Fail();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected class SetupFailTest: TestSetup 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="test"></param>
			public SetupFailTest(ITest test) : base(test) {}
			/// <summary>
			/// 
			/// </summary>
			protected override void SetUp() 
			{
				Assertion.Fail();
			}
		}
		#endregion
	}
}