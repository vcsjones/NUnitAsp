#region Copyright (c) 2002, Brian Knowles, Jim Little
/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Little
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
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test
{
	public class PerformanceTest : NUnitAspTestCase
	{
		private const int expectedTestsPerSecond = 2;
		private const long speedOfMinimumComputer = 997576;		// determined empirically; do not change

		[Test]
		public void TestPerformance() 
		{
			TimeSpan actualTime = BestOutOfThree();
			actualTime = AdjustForSpeedOfComputer(actualTime);
			TimeSpan expectedTime = new TimeSpan(0, 0, 0, 0, 1000 / expectedTestsPerSecond);
			string failureMessage = String.Format("performance must be at least {0} but was {1} (adjusted for speed of computer)", expectedTime, actualTime);
			Assert(failureMessage, actualTime <= expectedTime);
		}

		private TimeSpan AdjustForSpeedOfComputer(TimeSpan time)
		{
			long speed = CalculateComputerSpeedSuchThatLargerIsBetter();

			double speedRatio = speed / speedOfMinimumComputer;
			long adjustedTime = (long)(speedRatio * time.Ticks);
			return new TimeSpan(adjustedTime);
		}

		private long CalculateComputerSpeedSuchThatLargerIsBetter()
		{
			DateTime start = DateTime.Now;
			TimeSpan runTime = new TimeSpan(0, 0, 0, 0, 200);
			long speed = 0;
			int timeWaster = 153;
			do
			{
				speed++;
				if (timeWaster / 2 * 2 == timeWaster) timeWaster /= 2;
				else timeWaster = timeWaster * 3 + 1;
			} while ((DateTime.Now - start) < runTime);
			return speed;
		}

		private TimeSpan BestOutOfThree()
		{
			TimeSpan bestPerformance = TimeSpan.MaxValue;
			for (int i = 0; i < 3; i++) 
			{
				TimeSpan currentPerformance = TimedTest();
				if (currentPerformance < bestPerformance) bestPerformance = currentPerformance;
			}
			return bestPerformance;
		}

		private TimeSpan TimedTest()
		{
			TimeSpan accumulatedServerTime = Browser.ElapsedServerTime;
			DateTime start = DateTime.Now;
			RunTest();
			TimeSpan elapsedTime = DateTime.Now - start;
			TimeSpan clientTime = elapsedTime - (Browser.ElapsedServerTime - accumulatedServerTime);
			return clientTime;
		}

		private void RunTest()
		{
			TextBoxTester textBox1 = new TextBoxTester("textBox1", CurrentWebForm);
			TextBoxTester textBox2 = new TextBoxTester("textBox2", CurrentWebForm);
			TextBoxTester textBox3 = new TextBoxTester("textBox3", CurrentWebForm);
			TextBoxTester textBox4 = new TextBoxTester("textBox4", CurrentWebForm);
			TextBoxTester textBox5 = new TextBoxTester("textBox5", CurrentWebForm);
			ButtonTester button = new ButtonTester("button", CurrentWebForm);

			Browser.GetPage(BaseUrl + "PerformanceTestPage.aspx");
			AssertEquals("textBox1", "textBox1", textBox1.Text);
			AssertEquals("textBox2", "textBox2", textBox2.Text);
			AssertEquals("textBox3", "textBox3", textBox3.Text);
			AssertEquals("textBox4", "textBox4", textBox4.Text);
			AssertEquals("textBox5", "textBox5", textBox5.Text);

			textBox1.Text = "not 1";
			textBox2.Text = "not 2";
			textBox3.Text = "not 3";
			textBox4.Text = "not 4";
			textBox5.Text = "not 5";

			button.Click();
			AssertEquals("textBox1", "not 1", textBox1.Text);
			AssertEquals("textBox2", "not 2", textBox2.Text);
			AssertEquals("textBox3", "not 3", textBox3.Text);
			AssertEquals("textBox4", "not 4", textBox4.Text);
			AssertEquals("textBox5", "not 5", textBox5.Text);
		}
	}
}
