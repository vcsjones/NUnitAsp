namespace NUnit.Samples 
{
	using System;
	using NUnit.Framework;
	using NUnit.Tests;

	/// <summary>TestSuite that runs all the sample Tests.</summary>
	public class AllTests 
	{
		public static ITest Suite 
		{
			get 
			{
				TestSuite suite= new TestSuite(typeof(AllTests).FullName);
				suite.AddTest(ArrayListTest.Suite);
				suite.AddTest(NUnit.Tests.AllTests.Suite);
				suite.AddTest(new TestSuite(typeof(NUnit.Samples.Money.MoneyTest)));
				suite.AddTest(NUnit.Samples.SimpleCSharpTest.Suite);
				//suite.AddTest(NUnit.Samples.SimpleVBTest.Suite);
				return suite;
			}
		}
	}
}
