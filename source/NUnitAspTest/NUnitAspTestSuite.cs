using System;
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test
{

	public class NUnitAspTestSuite : TestSuite
	{

		public NUnitAspTestSuite() : base() 
		{
			AddTestSuite(typeof(BrowserTest));
			AddTestSuite(typeof(XhtmlDataGridTest));
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
