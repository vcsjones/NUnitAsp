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

using System;
using NUnit.Framework;
using System.Xml;

namespace NUnit.Extensions.Asp
{

	public abstract class XhtmlElement
	{

		private Browser browser;
		private XmlElement element;
		private string id;
		private string containerDescription;

		internal XhtmlElement(Browser browser, XmlElement element, string id, string containerDescription)
		{
			this.browser = browser;
			this.element = element;
			this.id = id;
			this.containerDescription = containerDescription;
		}
	
		protected void AssertEquals(object expected, object actual) 
		{
			Assertion.AssertEquals(Description, expected, actual);
		}

		protected void AssertEquals(string subElementDescription, object expected, object actual)
		{
			Assertion.AssertEquals(subElementDescription + " in " + Description, expected, actual);
		}

		protected string GetAttributeValue(string name) 
		{
			return element.Attributes[name].Value;
		}

		protected Browser Browser 
		{
			get 
			{
				return browser;
			}
		}

		protected XmlElement Element
		{
			get 
			{
				return element;
			}
		}

		protected string Description 
		{
			get 
			{
				string elementType = this.GetType().Name;
				return string.Format("{0} '{1}' in {2}", elementType, id, containerDescription);
			}
		}

	}

}
