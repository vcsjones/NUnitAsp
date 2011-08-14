#region Copyright (c) 2002, 2003, 2005 Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, 2005 Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp.Test
{
	public abstract class NUnitAspTestCase : WebFormTestCase
	{
		protected const string BasePath = "/NUnitAsp";
		protected const string BaseUrl = "http://localhost" + BasePath + "/";
		private DateTime startTime;
		private static TimeSpan totalElapsedTime = TimeSpan.Zero;
		private static TimeSpan totalServerTime = TimeSpan.Zero;
		private static readonly TimeSpan TEN_HOURS = new TimeSpan(10, 0, 0);

		protected override void SetUp()
		{
			base.SetUp();
			startTime = DateTime.Now;
		}

		protected override void TearDown()
		{
//			GatherAndTestCumulativePerformanceMetrics();
			base.TearDown();
		}

		private void GatherAndTestCumulativePerformanceMetrics()
		{
			TimeSpan testTime = DateTime.Now - startTime;
			if (testTime < TEN_HOURS) totalElapsedTime += testTime;   // avoid glitch with very fast tests
			totalServerTime += Browser.ElapsedServerTime;
			TimeSpan totalNUnitAspTime = totalElapsedTime - totalServerTime;
			double overheadPercentage = (double)totalNUnitAspTime.Ticks / (double)totalElapsedTime.Ticks;
			overheadPercentage = ((int)(overheadPercentage * 10000)) / 100.00;  // round to two decimal places

			Console.WriteLine();
			Console.WriteLine("    Elapsed: " + totalElapsedTime);
			Console.WriteLine(" -   Server: " + totalServerTime);
			Console.WriteLine(" = NUnitAsp: " + totalNUnitAspTime + " (" + overheadPercentage + "%)");

			NUnit.Framework.Assert.IsTrue(overheadPercentage < 25, "Expected NUnitAsp overhead to be less than 10%; was " + overheadPercentage + "%");
		}

		protected void AssertRedirected()
		{
			AssertEquals("RedirectionTarget", CurrentWebForm.AspId);
		}

	  
		protected void AssertDescription(string expected, Tester tester)
		{
			string description = tester.Description;
			if (description.StartsWith(expected)) return;

			Fail("Expected description to start with \nexpected: <" + expected + ">...\n but was: <" + description + ">");
		}
	}
}
