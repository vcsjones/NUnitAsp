#region Copyright (c) 2002-2004, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2004, Brian Knowles, Jim Shore
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
using System.Xml;
using NUnit.Framework;

namespace NUnit.Extensions.Asp
{

	/// <summary>
	/// A tester for an ASP.NET form.  Most of the methods in this class aren't meant to
	/// be called by third parties.
	/// 
	/// The API for this class will change in future releases.  
	/// </summary>
	public class WebForm : Tester
	{
		private HttpClient browser;

		/// <summary>
		/// Create the tester and link it to an ASP.NET web form.
		/// </summary>
		/// <param name="browser">The browser used to load this page.</param>
		public WebForm(HttpClient browser)
		{
			this.browser = browser;
		}

		/// <summary>
		/// The browser instance used to load the page containing the thing being tested.
		/// </summary>
		protected internal override HttpClient Browser
		{
			get
			{
				return browser;
			}
		}

		/// <summary>
		/// Returns the HTML ID of a child control.  Useful when implementing
		/// testers for container controls that do HTML ID mangling.  This method
		/// is very likely to change in a future release.
		/// </summary>
		protected internal override string GetChildElementHtmlId(string aspId)
		{
			return aspId;
		}

		/// <summary>
		/// Post this page to the server.  (That is, the page that contains the form being tested.)
		/// </summary>
		protected internal override void Submit()
		{
			browser.SubmitForm(this);
		}

		private HtmlTag Tag
		{
			get
			{
				XmlNodeList formNodes = browser.CurrentPage.GetElementsByTagName("form");
				Assertion.AssertEquals("page form elements", 1, formNodes.Count);
				XmlElement formElement = (XmlElement)formNodes[0];

				XmlAttribute id = formElement.Attributes["id"];
				Assertion.AssertNotNull("couldn't find web form's 'id' attribute", id);

				return new HtmlTag(browser, id.Value, this);
			}
		}

		internal string Action
		{
			get
			{
				return Tag.Attribute("action");
			}
		}

		internal string Method
		{
			get
			{
				return Tag.Attribute("method");
			}
		}

		/// <summary>
		/// A human-readable description of the location of the control.
		/// </summary>
		public override string Description
		{
			get
			{
				return "web form '" + AspId + "'";
			}
		}

		/// <summary>
		/// The ASP.NET ID of the form being tested.  It corresponds to the
		/// ID in the ASP.NET source code.
		/// </summary>
		public string AspId
		{
			get
			{
				return Tag.Attribute("id");
			}
		}
	}
}
