#region Copyright (c) 2005 James Shore
/********************************************************************************************************************
'
' Copyright (c) 2005 James Shore
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
'*******************************************************************************************************************/
#endregion

using System;
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test
{
	[TestFixture]
	public class FormVariablesTest
	{
		private FormVariables variables;

		[SetUp]
		public void SetUp()
		{
			variables = new FormVariables();
			AddStuff();
		}

		private void AddStuff()
		{
			variables.Add("var", "val");
			variables.Add("var2", "val2");
			variables.Add("var", "valB");
		}

		[Test]
		public void TestAdd()
		{
			Assert.AreEqual("var=val&var2=val2&var=valB", variables.ToString());
		}

		[Test]
		public void TestRemove()
		{
			variables.Remove("var2", "val2");
			Assert.AreEqual("var=val&var=valB", variables.ToString());

			variables.Remove("var", "valB");
			Assert.AreEqual("var=val", variables.ToString());
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestRemove_WhenNotPresent()
		{
			variables.Remove("var2", "noSuchValue");
		}

		[Test]
		public void TestRemoveAll()
		{
			variables.RemoveAll("var");
			Assert.AreEqual("var2=val2", variables.ToString());
		}

		[Test]
		public void TestReplace()
		{
			variables.Replace("var2", "val2", "val2B");
			Assert.AreEqual("var=val&var=valB&var2=val2B", variables.ToString());
			variables.Replace("var", "val", "valC");
			Assert.AreEqual("var=valB&var2=val2B&var=valC", variables.ToString());
		}

		[Test]
		public void TestReplaceAll()
		{
			variables.ReplaceAll("var2", "val2B");
			Assert.AreEqual("var=val&var=valB&var2=val2B", variables.ToString());
			variables.ReplaceAll("var", "valC");
			Assert.AreEqual("var2=val2B&var=valC", variables.ToString());
			variables.ReplaceAll("new", "newVal");
			Assert.AreEqual("var2=val2B&var=valC&new=newVal", variables.ToString());
		}

		[Test]
		public void TestContains()
		{
			Assert.IsTrue(variables.Contains("var", "val"));
			Assert.IsFalse(variables.Contains("var", "val2"));
			Assert.IsTrue(variables.Contains("var", "valB"));
		}

		[Test]
		public void TestContainsAny()
		{
			Assert.IsTrue(variables.ContainsAny("var"));
			Assert.IsTrue(variables.ContainsAny("var2"));
			Assert.IsFalse(variables.ContainsAny("not there"));
		}

		[Test]
		public void TestAllValuesOf()
		{
			WebAssert.AreEqual(new string[] {"val", "valB"}, variables.AllValuesOf("var"));
			WebAssert.AreEqual(new string[] {"val2"}, variables.AllValuesOf("var2"));
			WebAssert.AreEqual(new string[] {}, variables.AllValuesOf("not there"));
		}

		[Test]
		public void TestValueOf()
		{
			Assert.AreEqual("val2", variables.ValueOf("var2"));
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestValueOf_WhenMoreThanOne()
		{
			variables.ValueOf("var");
		}

		[Test]
		[ExpectedException(typeof(WebAssertionException))]
		public void TestValueOf_WhenNone()
		{
			variables.ValueOf("not there");
		}
	}
}
