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
'******************************************************************************************************************/

using System;
using NUnit.Framework;
using System.Xml;

namespace NUnit.Extensions.Asp 
{
	public abstract class WebFormTestCase : TestCase 
	{

		private Browser _browser;

		public WebFormTestCase(string name) : base(name) 
		{
		}

		protected override void SetUp() 
		{
			base.SetUp();
			_browser = new Browser();
   		}

		protected override void TearDown() 
		{
			_browser.Dispose();
			base.TearDown();
		}

		protected XhtmlWebForm Page 
		{
			get 
			{
				return Browser.CurrentPage;
			}
		}

		protected AspWebForm CurrentWebForm
		{
			get 
			{
				return new AspWebForm(Browser);
			}
		}

		protected Browser Browser 
		{
			get 
			{
				return _browser;
			}
			set 
			{
				_browser = value;
			}
		}

		protected static void AssertEquals(string[] a, string[] b)
		{
			AssertEquals("", a, b);
		}

		protected static void AssertEquals(string message, string[] a, string[] b)
		{
			string aFlat = flatten(a);
			string bFlat = flatten(b);
			Assertion.AssertEquals(message, aFlat, bFlat);
		}

		private static string flatten(string[] a)
		{
			string result = "{";
			foreach (string element in a)
			{
				result += "<" + element + ">";
			}
			return result + "}";
		}

	}
}