namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;

	/// <summary>Test class used in SuiteTest.</summary>
	public class OneTestCase: TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public OneTestCase(String name): base(name){}
		
		/// <summary>
		/// 
		/// </summary>
		public void NoTestCase() {}
		
		/// <summary>
		/// 
		/// </summary>
		public virtual void TestCase() {}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		public void TestCase(int arg) {}
	}
}