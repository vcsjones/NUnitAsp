namespace NUnit.Tests 
{

	using System;

	using NUnit.Framework;

	/// <summary>Test class used in SuiteTest.</summary>
	public class NoTestCases: TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public NoTestCases(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		public void NoTestCase() 
		{
		}
	}
}
