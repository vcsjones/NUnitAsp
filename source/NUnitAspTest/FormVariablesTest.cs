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
		public void TestReplaceAll()
		{
			variables.ReplaceAll("var2", "val2B");
			Assert.AreEqual("var=val&var=valB&var2=val2B", variables.ToString());
		}
	}
}
