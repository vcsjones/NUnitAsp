namespace NUnit.Tests 
{

	using System;

	using NUnit.Framework;

	/// <summary>Test class used in SuiteTest.</summary>
	public class NotPublicTestCase: TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public NotPublicTestCase(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		protected void TestNotPublic() 
		{
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestPublic() 
		{
		}
	}
}
