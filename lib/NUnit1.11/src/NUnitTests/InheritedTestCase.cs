namespace NUnit.Tests 
{

	using System;

	/// <summary>Test class used in SuiteTest.</summary>
	public class InheritedTestCase: OneTestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public InheritedTestCase(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		public void Test2() 
		{
		}
	}
}
