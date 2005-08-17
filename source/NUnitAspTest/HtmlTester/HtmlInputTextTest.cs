using System;
using NUnit.Framework;
using NUnit.Extensions.Asp.HtmlTester;

namespace NUnit.Extensions.Asp.Test.HtmlTester
{
	[TestFixture]
	public class HtmlInputTextTest : NUnitAspTestCase
	{
		[Test]
		public void TestVisibilityOnlyBecauseThisTagHasNoBehaviorWorthMentioning()
		{
			Browser.GetPage(BaseUrl + "/HtmlTester/HtmlInputTextTestPage.aspx");
			WebAssert.Visible(new HtmlInputTextTester("text"));
		}
	}
}
