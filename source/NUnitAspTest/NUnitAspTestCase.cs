using System;

namespace NUnit.Extensions.Asp.Test
{

	public class NUnitAspTestCase : WebFormTestCase
	{
		protected const string BaseUrl = "http://localhost/NUnitAsp/NUnitAspTestPages/";

		public NUnitAspTestCase(string name) : base(name)
		{
		}
	}
}
