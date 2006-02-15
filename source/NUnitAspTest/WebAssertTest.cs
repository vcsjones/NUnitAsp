#region Copyright (c) 2002-2005, James Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005, James Shore
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'******************************************************************************************************************/
#endregion

using System;
using NUnit.Framework;
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test
{
	[TestFixture]
	public class WebAssertTest : NUnitAspTestCase
	{
		private const string TestPageUrl = BaseUrl + "WebAssertTestPage.aspx";

		[Test]
		public void TestVisibleAndNotVisible()
		{
			GetTestPage();
			WebAssert.Visible(new LabelTester("visible"));
			WebAssert.NotVisible(new LabelTester("invisible"));
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException), "error message: invisible (LabelTester 'invisible' in web form 'WebAssertTestPage' in " + TestPageUrl + ")")]
		public void TestVisibilityMessage()
		{
			GetTestPage();
			WebAssert.Visible(new LabelTester("invisible"), "error message");
		}

		[Test]
		public void TestAreEqual_WhenArrays()
		{
			AssertEqualsFails(new string[] {"1"}, new string[] {});
			WebAssert.AreEqual(new string[] {}, new string[] {});
			AssertEqualsFails(new string[] {"1"}, new string[] {"2"});

			AssertEqualsFails(new string[] {}, null);
			AssertEqualsFails(null, new string[] {});
			WebAssert.AreEqual((string[])null, (string[])null);

			AssertArrayRenders("\r\nexpected: {\"1\", \"2\", \"3\"}\r\n but was: {}", new string[] {"1", "2", "3"}, new string[] {});
			AssertArrayRenders("\r\nexpected: {}\r\n but was: <null>", new string[] {}, null);
		}

		[Test]
		public void TestAreEqual_WhenDoubleArrays()
		{
			AssertEqualsFails(new string[][] {new string[] {"1"}}, new string[][] {});
			WebAssert.AreEqual(new string[][] {}, new string[][] {});
			AssertEqualsFails(new string[][] {new string[] {"1"}}, new string[][] {new string[] {"2"}});
			AssertEqualsFails(new string[][] {new String[] {"1", "2"}}, new string[][] {new string[] {"1"}});
			
			AssertEqualsFails(new string[][] {}, null);
			AssertEqualsFails(null, new string[][] {});
			WebAssert.AreEqual((string[][])null, (string[][])null);

			string expected = "\r\nexpected: \r\n   {\r\n      {\"1\"}\r\n   }\r\n but was: \r\n   {\r\n      {\"1\"}\r\n      {\"2\"}\r\n   }";
			string[][] one = new string[][] {
																				new string[] {"1"}
																			};
			string[][] two = new string[][] {
																				new string[] {"1"},
																				new string[] {"2"}
																			};
			AssertArrayRenders(expected, one, two);							
			AssertArrayRenders("\r\nexpected: {}\r\n but was: <null>", new string[][] {},  null);
		}

		[Test]
		public void TestSorted_WhenSorted()
		{
			string[][] testData = new string[][]
			{
				new string[] {"1"},
				new string[] {"2"},
				new string[] {"3"},
				new string[] {"4"}
			};
			WebAssert.Sorted(testData, 0, true, DataType.String);
		}

		[Test]
		public void TestSorted_WhenSortedAndStartingWithEmptyString()
		{
			string[][] testData = new string[][]
			{
				new string[] {""},
				new string[] {"2"},
				new string[] {"3"},
				new string[] {"4"}
			};
			WebAssert.Sorted(testData, 0, true, DataType.String);
		}
		
		[Test]
		public void TestSorted_WhenSortedAndManyRepeatingValues()
		{
			string[][] testData = new string[][]
			{
				new string[] {""},
				new string[] {"2"},
				new string[] {"2"},
				new string[] {"2"},
				new string[] {"3"},
				new string[] {"4"}
			};
			WebAssert.Sorted(testData, 0, true, DataType.String);
		}

		[Test]
		public void TestSorted_WhenSortedDescending()
		{
			string[][] testData = new string[][]
			{
				new string[] {"4"},
				new string[] {"3"},
				new string[] {"2"},
				new string[] {"1"},
			};
			WebAssert.Sorted(testData, 0, false, DataType.String);
		}

		[Test]
		public void TestSorted_WhenSortedDescendingAndEndingWithEmptyString()
		{
			string[][] testData = new string[][]
			{
				new string[] {"4"},
				new string[] {"3"},
				new string[] {"2"},
				new string[] {""},
			};
			WebAssert.Sorted(testData, 0, false, DataType.String);
		}

		[Test]
		public void TestSorted_WhenSortedDescendingAndManyRepeatValues()
		{
			string[][] testData = new string[][]
			{
				new string[] {"4"},
				new string[] {"3"},
				new string[] {"3"},
				new string[] {"3"},
				new string[] {"2"},
				new string[] {"1"},
			};
			WebAssert.Sorted(testData, 0, false, DataType.String);
		}

		[Test]
		public void TestSorted_WhenSortingOnLastColumn()
		{
			string[][] testData = new string[][]
			{
				new string[] {"1", "4"},
				new string[] {"3", "3"},
				new string[] {"2", "2"},
				new string[] {"4", "1"},
			};
			WebAssert.Sorted(testData, 1, false, DataType.String);
		}

		[Test]
		public void TestSorted_WhenNotSorted()
		{
			string[][] testData = new string[][]
			{
				new string[] {"2"},
				new string[] {"1"},
				new string[] {"3"},
				new string[] {"4"}
			};
			AssertSortOrderFails(testData, DataType.String);
		}

		[Test]
		public void TestSorted_WhenNoData()
		{
			WebAssert.Sorted(new string[][] {}, 0, true, DataType.String, "ascending");
			WebAssert.Sorted(new string[][] {}, 0, false, DataType.String, "descending");
		}

		[Test]
		public void TestSorted_WhenNumber()
		{
			string[][] testData = new string[][]
			{
				new string[] {"9"},
				new string[] {"10"},
			};
			WebAssert.Sorted(testData, 0, true, DataType.Int);
		}

		[Test]
		public void TestSorted_WhenBlankNumber()
		{
			string[][] testData = new string[][]
			{
				new string[] {""},
				new string[] {"9"},
				new string[] {"10"},
			};
			WebAssert.Sorted(testData, 0, true, DataType.Int);
		}

		[Test]
		public void TestSorted_WhenBlankNumberAtEnd()
		{
			string[][] testData = new string[][]
			{
				new string[] {"9"},
				new string[] {"10"},
				new string[] {""},
			};
			AssertSortOrderFails(testData, DataType.Int);
		}

		[Test]
		public void TestSorted_WhenDate()
		{
			string[][] testData = new string[][]
			{
				new string[] {"7/4/2002"},
				new string[] {"7/16/2002"},
			};
			WebAssert.Sorted(testData, 0, true, DataType.DateTime);
		}

		[Test]
		public void TestSorted_WhenBlankDate()
		{
			string[][] testData = new string[][]
			{
				new string[] {""},
				new string[] {"7/4/2002"},
				new string[] {"7/16/2002"},
			};
			WebAssert.Sorted(testData, 0, true, DataType.DateTime);
		}

		[Test]
		public void TestSorted_WhenBlankDateAtEnd()
		{
			string[][] testData = new string[][]
			{
				new string[] {"7/4/2002"},
				new string[] {"7/16/2002"},
				new string[] {""},
			};
			AssertSortOrderFails(testData, DataType.DateTime);
		}

		[Test]
		public void TestTableContainsRow()
		{
			string[][] testData = new string[][]
			{
				new string[] {"a", "b"},
			};
			WebAssert.TableContainsRow(testData, "a", "b");
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestTableContainsRow_WhenItDoesnt()
		{
			string[][] testData = new string[][] {};
			WebAssert.TableContainsRow(testData, "a", "b");
		}
        
		[Test]
		public void TestCurrentUrlEndsWith_WhenPasses()
		{
			GetTestPage();
			WebAssert.CurrentUrlEndsWith("/WebAssertTestPage.aspx");
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestCurrentUrlEndsWith_WhenFails()
		{
			GetTestPage();
			WebAssert.CurrentUrlEndsWith("/foo");
		}
        
		private void AssertSortOrderFails(string[][] testData, DataType dataType)
		{
			try
			{
				WebAssert.Sorted(testData, 0, true, dataType);
			}
			catch (WebAssertionException)
			{
				return;
			}
			NUnit.Framework.Assert.Fail("Expected assertion");
		}

		private void AssertEqualsFails(string[] expected, string[] actual)
		{
			try
			{
				WebAssert.AreEqual(expected, actual);
			}
			catch (WebAssertionException)
			{
				return;
			}
			NUnit.Framework.Assert.Fail("Expected assertion");
		}

		private void AssertEqualsFails(string[][] expected, string[][] actual)
		{
			try
			{
				WebAssert.AreEqual(expected, actual);
			}
			catch (WebAssertionException)
			{
				return;
			}
			NUnit.Framework.Assert.Fail("Expected assertion");
		}

		private void AssertArrayRenders(string expected, string[] one, string[] two)
		{
			try
			{
				WebAssert.AreEqual(one, two);
				NUnit.Framework.Assert.Fail("Expected assertion");
			}
			catch (WebAssertionException e)
			{
				NUnit.Framework.Assert.AreEqual(expected, e.Message);
			}
		}

		private void AssertArrayRenders(string expected, string[][] one, string[][] two)
		{
			try
			{
				WebAssert.AreEqual(one, two);
				NUnit.Framework.Assert.Fail("Expected assertion");
			}
			catch (WebAssertionException e)
			{
				NUnit.Framework.Assert.AreEqual(expected, e.Message);
			}
		}

		private void GetTestPage()
		{
			Browser.GetPage(TestPageUrl);
		}
	}
}
