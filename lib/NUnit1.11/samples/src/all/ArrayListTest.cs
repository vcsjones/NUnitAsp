namespace NUnit.Samples 
{
	using System;
	using System.Collections;
	using NUnit.Framework;

	/// <summary>A sample test case, testing
	/// <see cref="System.Collections.ArrayList"/></summary>.
	public class ArrayListTest: TestCase 
	{
		protected ArrayList fEmpty;
		protected ArrayList fFull;

		public ArrayListTest(String name): base(name) {}

		protected override void SetUp()
		{
			fEmpty= new ArrayList();
			fFull= new ArrayList();
			fFull.Add(1);
			fFull.Add(2);
			fFull.Add(3);
		}

		public static ITest Suite 
		{
			get { return new TestSuite(typeof(ArrayListTest)); }
		}

		public void TestCapacity() 
		{
			int size= fFull.Count; 
			for (int i= 0; i < 100; i++)
				fFull.Add(i);
			Assertion.Assert(fFull.Count == 100+size);
		}

		public void TestClone() 
		{
			ArrayList clone= (ArrayList)fFull.Clone(); 
			Assertion.Assert(clone.Count == fFull.Count);
			Assertion.Assert(clone.Contains(1));
		}
		public void TestContains() 
		{
			Assertion.Assert(fFull.Contains(1));  
			Assertion.Assert(!fEmpty.Contains(1));
		}
		public void TestElementAt() 
		{
			int i= (int)fFull[0];
			Assertion.Assert(i == 1);
            
			try 
			{ 
				int j = (int)fFull[fFull.Count];
			}
			catch(ArgumentOutOfRangeException) 
			{
				return;
			}
			Assertion.Fail("Should raise an ArgumentOutOfRangeException");
		}
		public void TestRemoveAll() 
		{
			fFull.Clear();
			fEmpty.Clear();
			Assertion.Assert(fFull.Count == 0);
			Assertion.Assert(fEmpty.Count == 0); 
		}
		public void TestRemoveElement() 
		{
			fFull.Remove(3);
			Assertion.Assert(!fFull.Contains(3)); 
		}
	}
}