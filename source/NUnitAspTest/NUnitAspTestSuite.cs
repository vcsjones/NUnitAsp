using System;
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test
{

	public class NUnitAspTestSuite : TestSuite
	{

		public NUnitAspTestSuite() : base() 
		{
			AddTestSuite(typeof(AspDataGridTest));
			AddTestSuite(typeof(AspLabelTest));
			AddTestSuite(typeof(HttpBrowserTest));
		}

		public static ITest Suite 
		{
			get 
			{
				return new NUnitAspTestSuite();
			}
		}

	}

}
