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
using System.Xml;
using NUnit.Framework;

namespace NUnit.Extensions.Asp
{

	public class AspWebForm : Control
	{
		Browser browser;

		public AspWebForm(Browser browser)
		{
			this.browser = browser;
		}

		internal override bool HasElement(string aspId)
		{
			return (GetElementInternal(aspId) != null);
		}

		internal override XmlElement GetElement(string aspId)
		{
			XmlElement element = GetElementInternal(aspId);
			if (element == null) throw new ElementNotVisibleException("Couldn't find " + aspId + " on " + Description);
			return element;
		}

		private XmlElement GetElementInternal(string aspId)
		{
			return browser.CurrentDocument.GetElementById(aspId);
		}

		internal override void EnterInputValue(string name, string value)
		{
			browser.EnterInputValue(name, value);
		}

		internal override void Submit()
		{
			browser.SubmitForm();
		}

		public override string Description
		{
			get
			{
				return "web form '" + HtmlId + "'";
			}
		}

		private string HtmlId
		{
			get
			{
				XmlNodeList bodyList = browser.CurrentDocument.GetElementsByTagName("form");
				Assertion.AssertEquals("page form elements", 1, bodyList.Count);
				XmlAttribute id = bodyList[0].Attributes["id"];
				if (id == null) throw new AttributeMissingException("id", Description);
				return id.Value;
			}
		}

		private class ElementNotVisibleException : ApplicationException
		{
			internal ElementNotVisibleException(string message) : base(message)
			{
			}
		}
	}
}
