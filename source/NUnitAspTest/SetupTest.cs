using System;
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test
{
	[TestFixture]
	public class SetupTest : NUnitAspTestCase
	{
		[SetUp]
		public void MySetUp()
		{
		}

		[TearDown]
		public void MyTearDown()
		{
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void TestSetupAttributeErrorMessage()
		{
			Browser.GetPage(BaseUrl);
		}
	}
}
