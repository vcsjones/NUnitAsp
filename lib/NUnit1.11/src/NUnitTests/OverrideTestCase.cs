namespace NUnit.Tests 
{   
	using System;

	/// <summary>Test class used in SuiteTest.</summary>
	public class OverrideTestCase: OneTestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public OverrideTestCase(String name) : base(name){}

		/// <summary>
		/// 
		/// </summary>
		public override void TestCase()
		{
		}
	}
}