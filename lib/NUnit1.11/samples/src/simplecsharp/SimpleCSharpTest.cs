namespace NUnit.Samples 
{
	using System;
	using NUnit.Framework;

	/// <summary>Some simple Tests.</summary>
	public class SimpleCSharpTest: TestCase
	{
		/// <summary>
		/// 
		/// </summary>
		protected int fValue1;
		/// <summary>
		/// 
		/// </summary>
		protected int fValue2;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public SimpleCSharpTest(String name) : base(name) {}
		/// <summary>
		/// 
		/// </summary>
		protected override void SetUp() 
		{
			fValue1= 2;
			fValue2= 3;
		}
		/// <summary>
		/// 
		/// </summary>
		public static ITest Suite 
		{
			// the dynamic way
			get {return new TestSuite(typeof (SimpleCSharpTest));}

			/*
			 * the type safe way
			 *
			 protected class AddSimpleTest: SimpleTest {
			   public void AddSimpleTest(String name) : base(name) {}
			   protected override void RunTest() { TestAdd(); }
			 }
			 protected override void DivideSimpleTest: SimpleTest {
			   public void DivideSimpleTest(String name) : base(name) {}
			   protected override void RunTest() { TestAdd(); }
			 }
			 ...
			 TestSuite suite= new TestSuite();
			 suite.AddTest( new AddSimpleTest("Add");
			 suite.AddTest( new DivideSimpleTest("TestDivideByZero");
			 return suite;
			*/
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAdd() 
		{
			double result= fValue1 + fValue2;
			// forced failure result == 5
			Assertion.AssertEquals("Expected Failure.",6,result);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestDivideByZero() 
		{
			int zero= 0;
			int result= 8/zero;
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestEquals() 
		{
			Assertion.AssertEquals("Integer.",12, 12);
			Assertion.AssertEquals("Long.",12L, 12L);
			Assertion.AssertEquals("Char.",'a', 'a');
			Assertion.AssertEquals("Integer Object Cast.",(object)12, (object)12);
            
			Assertion.AssertEquals("Expected Failure (Integer).", 12, 13);
			Assertion.AssertEquals("Expected Failure (Double).", 12.0, 11.99, 0.0);
		}
	}
}