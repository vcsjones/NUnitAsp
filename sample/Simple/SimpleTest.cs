using System;
using NUnit.Extensions.Asp;

namespace Simple
{
	public class SimpleTest : WebFormTestCase
	{
		public SimpleTest(string name) : base(name)
		{
		}

		public void TestFirstPage()
		{
			Browser.GetPage("http://localhost/NUnitAsp/SamplePages/FirstPage.aspx");
		}
	}
}
