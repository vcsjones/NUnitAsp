namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;
  
	/// <summary>
	/// A test case testing the extensions to the Testing framework.
	/// </summary>
	public class Failure: TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public Failure(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		public void Test() 
		{
			Assertion.Fail("Intentional Failure");
		}
	}
}
