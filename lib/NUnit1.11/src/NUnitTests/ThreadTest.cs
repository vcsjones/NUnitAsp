namespace NUnit.Tests 
{
	using System;
	using System.Threading;
	using NUnit.Framework;

	/// <summary>A test case Testing the running of Tests across multiple
	/// threads.</summary><remarks>
	/// Demonstrates that running a test case across different threads
	/// doesn't work yet.</remarks>
	public class ThreadTest: TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ThreadTest(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ITest Suite() 
		{
			return new TestSuite(typeof(ThreadTest));
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRemote() 
		{            
			Thread t= new Thread(new ThreadStart(VerifyResults));
			t.Start();
			try 
			{
				t.Join();
			}
			catch(ThreadInterruptedException) 
			{
				Assertion.Fail("interrupted test");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public class RunInThreadTestCase: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			public RunInThreadTestCase() : base("runInThread") {}
			/// <summary>
			/// 
			/// </summary>
			protected override void RunTest() 
			{
				Thread t= new Thread(new ThreadStart(Assertion.Fail));
				t.Start();
				try 
				{
					t.Join();
				}
				catch(ThreadInterruptedException ) 
				{
					Assertion.Fail("interrupted test");
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRunInThread() 
		{

			Thread t = new Thread(new ThreadStart(VerifyResults));
			TestCase runInThread = new RunInThreadTestCase();

			TestResult result= runInThread.Run();
			Assertion.AssertEquals("RunCount",1,result.RunCount);
			Assertion.AssertEquals("FailureCount",1,result.FailureCount);
			Assertion.AssertEquals("ErrorCount",0,result.ErrorCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void VerifyResults() 
		{
			Assertion.Fail("verify failed");
		}
	}
}