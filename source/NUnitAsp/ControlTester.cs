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
using NUnit.Framework;
using System.Xml;
using System.Text.RegularExpressions;

namespace NUnit.Extensions.Asp
{
	/// <summary>
	/// Base class for all tag-based controls.  Most people should
	/// extend <see cref="AspTester.AspControlTester" /> or <see cref="HtmlTester.HtmlControlTester"/>.
	/// 
	/// The API for this class will change in future releases.
	/// </summary>
	public abstract class ControlTester : Tester
	{
		private Tester container;

		/// <summary>
		/// Create the tester and link it to an ASP.NET control.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  (In the page's ASP.NET
		/// source code, look for the tag that the control is nested in.  That's probably the
		/// control's container.  Use CurrentWebForm if the control is just nested in the form tag.)</param>
		protected ControlTester(string aspId, Tester container)
		{
			this.AspId = aspId;
			this.container = container;
		}

		/// <summary>
		/// The ASP.NET ID of the control being tested.  It corresponds to the
		/// ID in the ASP.NET source code.
		/// </summary>
		public readonly string AspId;

		/// <summary>
		/// The HTML tag this tester corresponds to.
		/// </summary>
		protected virtual HtmlTag Tag
		{
			get
			{
				return new HtmlTag(Browser, HtmlId, this);
			}
		}

		/// <summary>
		/// Returns true if the control is visible on the current page.
		/// </summary>
		public virtual bool Visible
		{
			get
			{
				return Tag.Visible;
			}
		}

		/// <summary>
		/// Returns the HTML ID of a child control.  Useful when implementing
		/// testers for container controls that do HTML ID mangling.  This method
		/// is very likely to change in a future release.
		/// </summary>
		protected internal override string GetChildElementHtmlId(string aspId)
		{
			return container.GetChildElementHtmlId(aspId);
		}

		/// <summary>
		/// Deprecated--do not use.
		/// </summary>
		[Obsolete("Use the 'Tag' property instead")]
		protected internal virtual XmlElement Element
		{
			get 
			{
				XmlElement element = Browser.CurrentPage.GetElementById(HtmlId);
				if (element == null) throw new HtmlTag.ElementNotVisibleException("Couldn't find " + HtmlId + " on " + Description);
				return element;
			}
		}

		/// <summary>
		/// The browser instance used to load the page containing the form being tested.
		/// </summary>
		protected internal override HttpClient Browser
		{
			get
			{
				return container.Browser;
			}
		}

		/// <summary>
		/// A human-readable description of the location of the form being tested.
		/// This property describes the location of the form in the ASP.NET source
		/// code as well as in the HTML page rendered by the server.
		/// </summary>
		public string HtmlIdAndDescription
		{
			get
			{
				return string.Format("{0} ({1})", HtmlId, Description);
			}
		}

		/// <summary>
		/// A human-readable description of the location of the control.  Unlike
		/// <see cref="HtmlIdAndDescription"/>, this property only describes the
		/// location of the control in the ASP.NET source code.
		/// </summary>
		public override string Description 
		{
			get 
			{
				string controlType = this.GetType().Name;
				return string.Format("{0} '{1}' in {2}", controlType, AspId, container.Description);
			}
		}

		/// <summary>
		/// The HTML ID of the control being tested.  It corresponds to the
		/// ID of the HTML tag rendered by the server.  It's useful for looking at 
		/// raw HTML while debugging.
		/// </summary>
		public virtual string HtmlId
		{
			get
			{
				return container.GetChildElementHtmlId(AspId);
			}
		}

		/// <summary>
		/// True if the control is disabled.
		/// </summary>
		protected virtual bool IsDisabled
		{
			get
			{
				return Tag.HasAttribute("disabled");
			}
		}

		protected internal void EnterInputValue(object owner, string name, string value)
		{
			AssertEnabled();
			Browser.SetFormVariable(owner, name, value);
		}

		protected internal void RemoveInputValue(object owner, string name)
		{
			AssertEnabled();
			Browser.ClearFormVariable(owner, name);
		}

		private void AssertEnabled()
		{
			if (IsDisabled) throw new ControlDisabledException(this);
		}

		protected void EnterInputValue(string name, string value)
		{
			EnterInputValue(Element, name, value);
		}

		protected void RemoveInputValue(string name)
		{
			RemoveInputValue(Element, name);
		}

		/// <summary>
		/// Post this page to the server.  (That is, the page that contains the control being tested.)
		/// </summary>
		protected internal override void Submit()
		{
			container.Submit();
		}

		/// <summary>
		/// Like <see cref="PostBack"/>, but doesn't fail if the control doesn't
		/// have an automatic post-back.
		/// </summary>
		protected void OptionalPostBack(string candidatePostBackScript)
		{
			if (IsPostBack(candidatePostBackScript))
			{
				PostBack(candidatePostBackScript);
			}
		}

		/// <summary>
		/// Checks a URL to see if it is an automatic post-back script.
		/// Typically you should use <see cref="OptionalPostBack"/> instead.
		/// </summary>
		protected bool IsPostBack(string candidatePostBackScript)
		{
			return (candidatePostBackScript != null) && (candidatePostBackScript.IndexOf("__doPostBack") != -1);
		}

		private void SetInputHiddenValue(string name, string value)
		{
			string expression = string.Format("//form//input[@type='hidden'][@name='{0}']", name);
			Browser.SetFormVariable((XmlElement)Element.SelectSingleNode(expression), name, value);
		}

		/// <summary>
		/// Automatically post-back to the server--fails if there isn't an automatic
		/// post-back for the tested control.
		/// </summary>
		/// <param name="postBackScript"></param>
		protected void PostBack(string postBackScript)
		{
			string postBackPattern = @"__doPostBack\('(?<target>.*?)','(?<argument>.*?)'\)";

			Match match = Regex.Match(postBackScript, postBackPattern, RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				throw new ParseException("'" + postBackScript + "' doesn't match expected pattern for postback in " + HtmlIdAndDescription);
			}

			string target = match.Groups["target"].Captures[0].Value;
			string argument = match.Groups["argument"].Captures[0].Value;

			SetInputHiddenValue("__EVENTTARGET", target.Replace('$', ':'));
			SetInputHiddenValue("__EVENTARGUMENT", argument);
			Submit();
		}
	}

	/// <summary>
	/// Exception: The tester has a bug: it was looking for some HTML and didn't find it.
	/// Report this exception to the author of the tester.
	/// </summary>
	public class ParseException : ApplicationException
	{
		internal ParseException(string message) : base(message)
		{
		}
	}

	/// <summary>
	/// Exception: The test is trying to perform a UI operation on a disabled control.
	/// Enable the control in your production code or don't change it in the test.
	/// </summary>
	public class ControlDisabledException : InvalidOperationException
	{
		public ControlDisabledException(ControlTester control) :
			base(GetMessage(control))
		{
		}

		private static string GetMessage(ControlTester control)
		{
			return string.Format(
				"Control {0} (HTML ID: {1}; ASP location: {2}) is disabled",
				control.AspId, control.HtmlId, control.Description);
		}
	}
}
