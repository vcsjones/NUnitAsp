#region Copyright (c) 2003-2005 Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2003-2005, Brian Knowles, Jim Shore
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
	/// <p>An HTML tag.  This class performs some of the magic that 
	/// allows NUnitAsp to construct testers before pages are loaded and
	/// to have testers change as the current page changes.</p>
	/// <p>When using the methods on this class, check the API documentation
	/// before caching the results.  The API of this class may 
	/// change in future releases.</p>
	/// </summary>
	public class HtmlTagTester : ControlTester
	{
		private string xpath = null;
		private string description = null;
		private XmlDocument pageForTestingOnly = null;
		private string idForTestingOnly = null;
		private XmlElement element = null;

		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlTagTester(string htmlId) : base(htmlId)
		{
		}

		/// <summary>
		/// Create a tester for a server-side HTML control or a tag that's on a 
		/// page with multiple forms.  Use this constructor when the HTML tag you
		/// are testing has the "runat='server'" attribute.
		/// Also use this tester when using the non-default webform or HttpClient.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.)  If testing a page with multiple forms or a non-default
		/// HttpClient, pass in the WebFormTester for the form this tag is within.</param>
		public HtmlTagTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// Create a tester for an HTML tag using an XPath description.
		/// </summary>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A human-readable description of this tag (for error reporting).</param>
		public HtmlTagTester(string xpath, string description) : base()
		{
			this.xpath = xpath;
			this.description = description;
		}

		/// <summary>
		/// Create a tester for an HTML tag that's on a page with multiple forms using
		/// an XPath description.
		/// </summary>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A human-readable description of this tag (for error reporting).</param>
		/// <param name="container">A tester for the control's container.  A WebFormTester
		/// will usually be most appropriate.</param>
		public HtmlTagTester(string xpath, string description, Tester container) : base(container)
		{
			this.xpath = xpath;
			this.description = description;
		}

		// testing only
		private HtmlTagTester()
		{
		}

		/// <summary>
		/// For NUnitAsp's test suite only.
		/// </summary>
		public static HtmlTagTester TestInstance(string htmlPage, string id, string description)
		{
			HtmlTagTester instance = new HtmlTagTester();
			instance.pageForTestingOnly = new XmlDocument();
			instance.pageForTestingOnly.LoadXml(htmlPage);
			instance.idForTestingOnly = id;
			instance.description = description;
			return instance;
		}

		/// <summary>
		/// For NUnitAsp's test suite only.
		/// </summary>
		public static HtmlTagTester TestInstance(string htmlPage, string xpath)
		{
			HtmlTagTester instance = new HtmlTagTester();
			instance.pageForTestingOnly = new XmlDocument();
			instance.pageForTestingOnly.LoadXml(htmlPage);
			instance.xpath = xpath;
			instance.description = htmlPage;
			return instance;
		}

		/// <summary>
		/// A static HTML tag constructed for temporary use.  It will not reflect the page currently
		/// loaded by the browser and shouldn't be cached.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="description">A description of this tag (for error reporting).</param>
		private HtmlTagTester(XmlElement element, string description)
		{
			this.element = element;
			this.description = description;
		}

		/// <summary>
		/// Returns true if the tag is visible on the current page.
		/// </summary>
		public override bool Visible
		{
			get
			{
				return OptionalElement != null;
			}
		}

		/// <summary>
		/// Returns the name of the tag.  For example, the tag "&lt;a href='foo'&gt;" will return "a".
		/// </summary>
		public string Name
		{
			get
			{
				return Element.Name;
			}
		}

		/// <summary>
		/// Returns the value of an attribute on this tag or throws an exception if the attribute
		/// isn't present.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <returns>The value of the attribute</returns>
		public string Attribute(string name) 
		{
			string result = OptionalAttribute(name);
			if (result == null) throw new Tester.AttributeMissingException(name, Description);
			return result;
		}

		/// <summary>
		/// Returns the value of an attribute on this tag or null if the attribute isn't present.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <returns>The value of the attribute or null if not present.</returns>
		public string OptionalAttribute(string name)
		{
			XmlAttribute attribute = Element.Attributes[name];
			if (attribute == null) return null;
			return attribute.Value;
		}

		/// <summary>
		/// Returns true if this tag has the specified attribute.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <returns>True if the attribute is present (even if it has no value); false otherwise</returns>
		public bool HasAttribute(string name)
		{
			return OptionalAttribute(name) != null;
		}

		/// <summary>
		/// Returns an attribute as an integer, or -1 if the attribute isn't present.  Throws
		/// an exception if the attribute isn't an integer.
		/// </summary>
		public int AttributeAsIntWithNegOneDefault(string name)
		{
			string attribute = Tag.OptionalAttribute(name);
			if (attribute == null) return -1;
			else return int.Parse(attribute);
		}

		/// <summary>
		/// The raw HTML inside the tag being tested.  For example, 
		/// &lt;a href='foo'&gt;&lt;i&gt;My&lt;/i&gt; Link&lt;/a&gt; will return
		/// "&lt;i&gt;My&lt;/i&gt; Link".
		/// </summary>
		public string InnerHtml
		{
			get
			{
				return Element.InnerXml;
			}
		}

		/// <summary>
		/// The HTML inside the tag being tested, rendered to a string as a web browser would
		/// do.  All inner tags are removed and adjacent whitespace is converted
		/// to a single space.  For example, &lt;a href='foo'&gt;&lt;i&gt;My&lt;/i&gt; Link&lt;/a&gt;
		/// will return "My Link".
		/// </summary>
		public string RenderedInnerHtml
		{
			get
			{
				return Regex.Replace(Element.InnerText, "\\s+", " ");
			}
		}

		/// <summary>
		/// A temporary hack--this method WILL GO AWAY!
		/// </summary>
		public string BodyNoTags
		{
			get
			{
				return Element.InnerText;
			}
		}

		/// <summary>
		/// Returns the tag that contains this one.  Don't cache it.  Don't use 
		/// this on the root element.
		/// </summary>
		public HtmlTagTester Parent
		{
			get
			{
				return new HtmlTagTester((XmlElement)Element.ParentNode, "the parent of " + Description);
			}
		}

		/// <summary>
		/// <p>Returns the immediate children of this tag that match a particular type (such as &lt;tr&gt;).
		/// Does not return "grand-children" -- i.e., calling <c>table.Children("tr")</c> will work, but
		/// calling <c>table.Children("td")</c> will typically return nothing because the 'td' tags
		/// are nested inside 'tr' tags.</p>
		/// <p>Don't cache the results of this call.</p>
		/// </summary>
		/// <param name="tag">The type of tag to return.  Don't include angle brackets.</param>
		/// <example>To get all rows in a table: <code>HtmlTagTester[] rows = table.Children("tr");</code></example>
		/// <returns>The tags, or an empty array if none.</returns>
		public HtmlTagTester[] Children(string tag)
		{
			return ChildrenByXPath(tag, "<" + tag + "> child");
		}

		/// <summary>
		/// <p>Returns the tags that match an XPath expression, starting from this tag (node).  Make
		/// sure that the result of your expression is a set of tags.</p>
		/// <p>Don't cache the results of this call.</p>
		/// </summary>
		/// <param name="xpath">The XPath expression to match.</param>
		/// <returns>The tags, or an empty array if none.</returns>
		public HtmlTagTester[] ChildrenByXPath(string xpath)
		{
			return ChildrenByXPath(xpath, "xpath <" + xpath + "> child");
		}

		private HtmlTagTester[] ChildrenByXPath(string xpath, string description)
		{
			XmlNodeList children = Element.SelectNodes(xpath);
			HtmlTagTester[] result = new HtmlTagTester[children.Count];
			for (int i = 0; i < children.Count; i++) 
			{
				result[i] = new HtmlTagTester((XmlElement)children[i], description + " #" + i + " of " + Description);
			}
			return result;
		}

		/// <summary>
		/// Returns 'true' if this tag has any immediate children that match a particular type
		/// (such as &lt;tr&gt;.  Does not check "grand-children" -- i.e., calling 
		/// <c>table.HasChildren("tr")</c> will usually return true and calling
		/// <c>table.HasChildren("td")</c> will usually return false.
		/// </summary>
		/// <param name="tag">The type of tag to look for.  Don't include angle brackets.</param>
		public bool HasChildren(string tag)
		{
			return Children(tag).Length != 0;
		}

		/// <summary>
		/// Returns the only child (of a particular type) of this tag.  If this tag has more
		/// that one child of the requested type, or if it has no children of the requested type,
		/// this method will throw an exception.  Don't cache the results of this call.
		/// </summary>
		/// <param name="tag">The type of tag to look for.  Don't include angle brackets.</param>
		public HtmlTagTester Child(string tag)
		{
			HtmlTagTester[] tags = Children(tag);
			WebAssert.True(tags.Length == 1, "Expected " + Description + " to have exactly one <" + tag + "> child tag.");
			return tags[0];
		}

		private XmlElement Element
		{
			get
			{
				XmlElement element = OptionalElement;
				if (element == null) throw new ElementNotVisibleException("Couldn't find " + Description);
				return element;
			}
		}

		// Returns null if not found
		private XmlElement OptionalElement
		{
			get
			{
				if (element != null) return element;

				XmlDocument document = pageForTestingOnly;
				if (document == null) document = Browser.CurrentPage.Document;

				string selector = xpath;
				if (selector == null) selector = "//*[@id='" + HtmlId + "']";

				XmlNodeList nodes = document.SelectNodes(selector);
				if (nodes.Count > 1) throw new ApplicationException("Expected only one node to match xpath '" + selector + "' but " + nodes.Count + " nodes matched");
				if (nodes.Count == 0) return null;
				return (XmlElement)nodes[0];

				// BTW, we didn't use GetElementById here because it didn't work on 
				// pageForTestingOnly. Not sure why. No DOCTYPE, perhaps?
			}
		}

		/// <summary>
		/// The HTML ID of the control being tested.  It corresponds to the
		/// ID of the HTML tag rendered by the server.  It's useful for looking at 
		/// raw HTML while debugging.
		/// </summary>
		public override string HtmlId
		{
			get
			{
				if (idForTestingOnly != null) return idForTestingOnly;
				if (xpath == null) return base.HtmlId;

				string id = OptionalAttribute("id");
				if (id == null) throw new NoHtmlIdException(Description);
				else return id;
			}
		}

		/// <summary>
		/// A human-readable description of the location of the control.
		/// </summary>
		public override string Description
		{
			get
			{
				if (description != null) return description;
				return base.Description;
			}
		}

		/// <summary>
		/// The HTML tag we're testing.
		/// </summary>
		protected override HtmlTagTester Tag
		{
			get
			{
				return this;
			}
		}

		public class ElementNotVisibleException : ApplicationException
		{
			internal ElementNotVisibleException(string message) : base(message)
			{
			}
		}

		public class NoHtmlIdException : ApplicationException
		{
			public NoHtmlIdException(string controlDescription)
				: base(string.Format("{0} has no HTML ID", controlDescription))
			{
			}
		}
	}
}