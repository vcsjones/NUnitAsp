#region Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
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
	/// Base class for all tag-based controls.  Extend this class if you're creating a
	/// custom tester.
	/// 
	/// The API for this class will change in future releases.
	/// </summary>
	public abstract class ControlTester : Tester
	{
		public readonly string AspId;
		private Tester container;

		internal ControlTester(string aspId, Tester container)
		{
			this.AspId = aspId;
			this.container = container;
		}

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

		protected string GetAttributeValue(string name) 
		{
			return Tag.Attribute(name);
		}

		/// <summary>
		/// Returns null if attribute not found
		/// </summary>
		protected string GetOptionalAttributeValue(string name)
		{
			return Tag.OptionalAttribute(name);
		}

		protected internal override string GetChildElementHtmlId(string aspId)
		{
			return container.GetChildElementHtmlId(aspId);
		}

//		private XmlElement Element 
		protected internal virtual XmlElement Element
		{
			get 
			{
				XmlElement element = Browser.CurrentPage.GetElementById(HtmlId);
				if (element == null) throw new HtmlTag.ElementNotVisibleException("Couldn't find " + HtmlId + " on " + Description);
				return element;
			}
		}

		protected internal override HttpClient Browser
		{
			get
			{
				return container.Browser;
			}
		}

		public string HtmlIdAndDescription
		{
			get
			{
				return string.Format("{0} ({1})", HtmlId, Description);
			}
		}

		public override string Description 
		{
			get 
			{
				string controlType = this.GetType().Name;
				return string.Format("{0} '{1}' in {2}", controlType, AspId, container.Description);
			}
		}

		public virtual string HtmlId
		{
			get
			{
				return container.GetChildElementHtmlId(AspId);
			}
		}

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

		protected internal override void Submit()
		{
			container.Submit();
		}

		/// <summary>
		/// Works properly even if candidatePostBackScript is null.
		/// </summary>
		protected void OptionalPostBack(string candidatePostBackScript)
		{
			if (IsPostBack(candidatePostBackScript))
			{
				PostBack(candidatePostBackScript);
			}
		}

		private bool IsPostBack(string candidate)
		{
			return (candidate != null) && (candidate.StartsWith("__doPostBack"));
		}

		private void SetInputHiddenValue(string name, string value)
		{
			string expression = string.Format("//form//input[@type='hidden'][@name='{0}']", name);
			Browser.SetFormVariable((XmlElement)Element.SelectSingleNode(expression), name, value);
		}

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

	public class ParseException : ApplicationException
	{
		internal ParseException(string message) : base(message)
		{
		}
	}

	/// <summary>
	/// The test is trying to perform a UI operation on a disabled control.  Enable the control in your 
	/// production code or don't change it in the test.
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
