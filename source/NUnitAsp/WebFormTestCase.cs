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
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp 
{
	public abstract class WebFormTestCase : TestCase 
	{
		private HttpClient browser;
		private WebForm form;

		public WebFormTestCase(string name) : base(name) 
		{
		}

		protected override void SetUp() 
		{
			base.SetUp();
			browser = new HttpClient();
			form = new WebForm(browser);
   		}

		protected WebForm CurrentWebForm
		{
			get 
			{
				return form;
			}
		}

		protected HttpClient Browser 
		{
			get 
			{
				return browser;
			}
		}

		protected static void AssertVisibility(ControlTester tester, bool expectedVisibility)
		{
			string not = expectedVisibility ? "" : " not";
			string message = String.Format("{0} should{1} be visible ({0} is {2})", tester.AspId, not, tester.Description);
			Assert(message, tester.Visible == expectedVisibility);
		}

		protected static void AssertEquals(string[] expected, string[] actual)
		{
			AssertEquals(flatten(expected), flatten(actual));
		}

		protected static void AssertEquals(string message, string[] expected, string[] actual)
		{
			Assertion.AssertEquals(message, flatten(expected), flatten(actual));
		}

		protected static void AssertEquals(string[][] expected, string[][] actual)
		{
			AssertEquals(flatten(expected), flatten(actual));
		}

		protected static void AssertEquals(string message, string[][] expected, string[][] actual)
		{
			AssertEquals(flatten(expected), flatten(actual));
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

		private static string flatten(string[][] a)
		{
			string result = "{";
			foreach (string[] element in a)
			{
				result += "\n   " + flatten(element);
			}
			return result + "\n}";
		}

	}
}