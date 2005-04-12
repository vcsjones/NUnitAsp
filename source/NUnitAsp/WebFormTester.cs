#region Copyright (c) 2002-2005, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002-2005, Brian Knowles, Jim Shore
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
using System.Text.RegularExpressions;

namespace NUnit.Extensions.Asp
{
	/// <summary>
	/// <p>A tester for an ASP.NET form.  Most of the methods in this class aren't meant to
	/// be called by third parties.</p>
	/// 
	/// <p>The API for this class will change in future releases.</p>
	/// </summary>
	public class WebFormTester : Tester
	{
		private HttpClient browser;
		private string aspId = null;

		/// <summary>
		/// Create the tester and link it to an ASP.NET web form.
		/// </summary>
		/// <param name="browser">The browser used to load this page.</param>
		public WebFormTester(HttpClient browser)
		{
			this.browser = browser;
		}

		public WebFormTester(string aspId, HttpClient browser)
		{
			this.aspId = aspId;
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
		/// The tester for the form containing the thing being tested.
		/// </summary>
		protected internal override WebFormTester Form
		{
			get
			{
				return this;
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
		/// Submit this form to the server.
		/// </summary>
		public void Submit()
		{
			WebAssert.Visible(this);
			browser.SubmitForm(this);
		}

		/// <summary>
		/// Emulates ASP.NET's post-back script.  To simply submit the form,
		/// use <see cref="Submit"/> instead.  If you have access to the JavaScript call
		/// string, use <see cref="OptionalPostBack"/> or the other form of
		/// <see cref="PostBack(string)"/>.
		/// </summary>
		/// <param name="eventTarget">The "event target" parameter for the post-back script.</param>
		/// <param name="eventArgument">The "event argument" parameter for the post-back script.</param>
		public void PostBack(string eventTarget, string eventArgument)
		{
			Variables.ReplaceAll("__EVENTTARGET", eventTarget);
			Variables.ReplaceAll("__EVENTARGUMENT", eventArgument);
			Submit();
		}

		/// <summary>
		/// Like <see cref="PostBack"/>, except that it does nothing if 
		/// candidatePostBackScript doesn't contain a post-back script.
		/// </summary>
		public void OptionalPostBack(string candidatePostBackScript)
		{
			if (IsPostBack(candidatePostBackScript))
			{
				PostBack(candidatePostBackScript);
			}
		}

		/// <summary>
		/// Checks a string to see if it contains a post-back script.
		/// Typically you should just use <see cref="OptionalPostBack"/> instead.
		/// </summary>
		public bool IsPostBack(string candidatePostBackScript)
		{
			return (candidatePostBackScript != null) && (candidatePostBackScript.IndexOf("__doPostBack") != -1);
		}

		/// <summary>
		/// Emulates ASP.NET's post-back script (often seen as a Javascript
		/// "__doPostBack" call).  To simply submit the form, use 
		/// <see cref="Submit"/> instead.  If you don't have access to the
		/// JavaScript call, use <see cref="PostBack(string, string)"/> instead.
		/// This method throws an exception if the postBackScript string isn't
		/// actually a post-back script (for example, if it's an empty string).
		/// Use <see cref="OptionalPostBack"/> if the string isn't expected to
		/// contain a post-back script all of the time.
		/// </summary>
		/// <example>
		/// ASP.NET link buttons are rendered as &lt;a&gt; hyperlinks with a
		/// post-back script call in the "href" attribute.  The following code takes
		/// the call from the attribute and calls PostBack(), causing NUnitAsp to
		/// analyze the post-back script call, set the appropriate environment
		/// variables, and submit the form.
		/// 
		/// <code>
		/// string href = Tag.Attribute("href");
		/// Form.PostBack(href);
		/// </code>
		/// </example>
		public void PostBack(string postBackScript)
		{
			string postBackPattern = @"__doPostBack\('(?<target>.*?)','(?<argument>.*?)'\)";

			Match match = Regex.Match(postBackScript, postBackPattern, RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				throw new ParseException("'" + postBackScript + "' doesn't match expected pattern for postback");
			}

			string target = match.Groups["target"].Captures[0].Value;
			string argument = match.Groups["argument"].Captures[0].Value;
			Form.PostBack(target.Replace('$', ':'), argument);
		}

		/// <summary>
		/// The HTML tag this tester corresponds to.
		/// </summary>
		protected override HtmlTag Tag
		{
			get
			{
				if (aspId == null) return FindTagByForm();
				else return FindTagById();
			}
		}

		private HtmlTag FindTagByForm()
		{
			XmlNodeList formNodes = browser.CurrentPage.Document.GetElementsByTagName("form");
			WebAssert.True(formNodes.Count == 1, "The current page has more than one form.  To test it, construct a WebFormTester and use it as the 'container' parameter for your other testers.");
			XmlElement formElement = (XmlElement)formNodes[0];

			XmlAttribute id = formElement.Attributes["id"];
			WebAssert.NotNull(id, "couldn't find web form's 'id' attribute");

			return new HtmlTag(browser, id.Value, this);
		}

		private HtmlTag FindTagById()
		{
			return new HtmlTag(browser, aspId, this);
		}

		/// <summary>
		/// The "action" attribute of this form; will be an empty string 
		/// if there isn't one.
		/// </summary>
		protected internal string Action
		{
			get
			{
				string action = Tag.OptionalAttribute("action");
				if (action == null) return "";
				return action;
			}
		}

		/// <summary>
		/// The "method" attribute of this form.
		/// </summary>
		protected internal string Method
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
		public override string AspId
		{
			get
			{
				if (aspId != null) return aspId;
				else return Tag.Attribute("id");
			}
		}

		
		/// <summary>
		/// The HTML ID of the form being tested.  It corresponds to the
		/// ID of the HTML tag rendered by the server.  It's useful for looking at 
		/// raw HTML while debugging.
		/// </summary>
		public override string HtmlId
		{
			get
			{
				return AspId;
			}
		}

		/// <summary>
		/// The HTML form variables in this form.
		/// </summary>
		public FormVariables Variables
		{
			get
			{
				return Browser.CurrentPage.VariablesFor(this.HtmlId);
			}
		}
	}
}
