namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;
	/// <summary>
	/// 
	/// </summary>
	public class AssertionTest: TestCase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public AssertionTest(String name) : base(name) 
		{
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertEquals() 
		{
			Object o= new Object();
			Assertion.AssertEquals(o, o);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertEqualsNaNFails() 
		{
			try 
			{
				Assertion.AssertEquals(1.234, Double.NaN, 0.0);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}    
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertEqualsNull() 
		{
			Assertion.AssertEquals(null, null);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertNanEqualsFails() 
		{
			try 
			{
				Assertion.AssertEquals(Double.NaN, 1.234, 0.0);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}     
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertNanEqualsNaNFails() 
		{
			try 
			{
				Assertion.AssertEquals(Double.NaN, Double.NaN, 0.0);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}     
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertNegInfinityEqualsInfinity() 
		{
			Assertion.AssertEquals(Double.NegativeInfinity, Double.NegativeInfinity, 0.0);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertPosInfinityEqualsInfinity() 
		{
			Assertion.AssertEquals(Double.PositiveInfinity, Double.PositiveInfinity, 0.0);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertPosInfinityNotEquals() 
		{
			try 
			{
				Assertion.AssertEquals(Double.PositiveInfinity, 1.23, 0.0);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertPosInfinityNotEqualsNegInfinity() 
		{
			try 
			{
				Assertion.AssertEquals(Double.PositiveInfinity, Double.NegativeInfinity, 0.0);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}

		/// <summary>
		/// 
		/// </summary>
		public void TestAssertSinglePosInfinityNotEqualsNegInfinity() 
		{
			try 
			{
				Assertion.AssertEquals(float.PositiveInfinity, float.NegativeInfinity, (float)0.0);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertSingle() 
		{
			Assertion.AssertEquals((float)1.0, (float)1.0, (float)0.0);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertByte() 
		{
			Assertion.AssertEquals((byte)1, (byte)1);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertString() 
		{
			string s1 = "test";
			string s2 = new System.Text.StringBuilder(s1).ToString();
			Assertion.AssertEquals(s1,s2);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertShort() 
		{
			Assertion.AssertEquals((short)1,(short)1);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertNull() 
		{
			Assertion.AssertNull(null);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertNullNotEqualsNull() 
		{
			try 
			{
				Assertion.AssertEquals(null, new Object());
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertSame() 
		{
			Object o= new Object();
			Assertion.AssertSame(o, o);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestAssertSameFails() 
		{
			try 
			{
				object one = 1;
				object two = 1;
				Assertion.AssertSame(one, two);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestFail() 
		{
			try 
			{
				Assertion.Fail();
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			throw new AssertionFailedError("fail"); // You can't call fail() here
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestFailAssertNotNull() 
		{
			try 
			{
				Assertion.AssertNotNull(null);
			} 
			catch (AssertionFailedError) 
			{
				return;
			}
			Assertion.Fail();
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestSucceedAssertNotNull() 
		{
			Assertion.AssertNotNull(new Object());
		}
	}
}
