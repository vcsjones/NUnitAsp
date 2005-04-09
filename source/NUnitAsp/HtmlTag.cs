#region Copyright (c) 2003-2004 Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2003-2004, Brian Knowles, Jim Shore
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

namespace NUnit.Extensions.Asp
{
	/// <summary>
	/// An HTML tag.  This class performs some of the magic that 
	/// allows NUnitAsp to construct testers before pages are loaded.
	/// When using the methods on this class, check the API documentation
	/// before caching the results.  The API of this class may 
	/// change in future releases.
	/// </summary>
	public class HtmlTag
	{
		private readonly HttpClient browser;
		private readonly string id;
		private readonly string xpath;
		private readonly Tester owner;
		private readonly string description;
		private readonly XmlDocument pageForTestingOnly;
		private readonly XmlElement element;

		/// <summary>
		/// Construct a dynamic HTML tag using an ID.  The state of the tag will reflect the page 
		/// currently loaded by the browser, even as it changes.
		/// </summary>
		/// <param name="browser">The browser to look at for the current page.</param>
		/// <param name="id">The HTML ID of the tag.</param>
		/// <param name="owner">The tester that corresponds to this tag (for error reporting).</param>
		public HtmlTag(HttpClient browser, string id, Tester owner)
		{
			this.browser = browser;
			this.id = id;
			this.owner = owner;
		}

		/// <summary>
		/// Construct a dynamic HTML tag using an XPath description.  The state of the tag will reflect
		/// the page currently loaded by the browser, even as it changes.
		/// </summary>
		/// <param name="browser">The browser to look at for the current page.</param>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A description of this tag (for error reporting).</param>
		public HtmlTag(HttpClient browser, string xpath, string description)
		{
			this.browser = browser;
			this.xpath = xpath;
			this.description = description;
		}

		/// <summary>
		/// For NUnitAsp's test suite only.
		/// </summary>
		public HtmlTag(string htmlPage, string id, string description)
		{
			pageForTestingOnly = new XmlDocument();
			pageForTestingOnly.LoadXml(htmlPage);
			this.id = id;
			this.description = description;
		}

		/// <summary>
		/// For NUnitAsp's test suite only.
		/// </summary>
		public HtmlTag(string htmlPage, string xpath)
		{
			pageForTestingOnly = new XmlDocument();
			pageForTestingOnly.LoadXml(htmlPage);
			this.xpath = xpath;
			this.description = htmlPage;
		}

		/// <summary>
		/// A static HTML tag constructed for temporary use.  It will not reflect the page currently
		/// loaded by the browser and shouldn't be cached.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="description">A description of this tag (for error reporting).</param>
		private HtmlTag(XmlElement element, string description)
		{
			this.element = element;
			this.description = description;
		}

		/// <summary>
		/// Returns true if the tag is visible on the current page.
		/// </summary>
		public bool Visible
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
		/// Returns the value of an attribute on this tag or throws an exception if the attribute isn't present.
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
		/// Returns the contents of the tag, but not the tag itself.  For example, &lt;a href='foo'&gt;My Link&lt;/a&gt;
		/// will return "My Link".
		/// </summary>
		public string Body
		{
			get
			{
				return Element.InnerXml;
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
		public HtmlTag Parent
		{
			get
			{
				return new HtmlTag((XmlElement)Element.ParentNode, "the parent of " + Description);
			}
		}

		/// <summary>
		/// Returns the immediate children of this tag that match a particular type (such as &lt;tr&gt;).
		/// Does not return "grand-children" -- i.e., calling <c>table.Children("tr")</c> will work, but
		/// calling <c>table.Children("td")</c> will typically return nothing.
		/// Don't cache the results of this call.
		/// </summary>
		/// <param name="tag">The type of tag to return.  Don't include angle brackets.</param>
		/// <example>To get all rows in a table: <code>HtmlTag[] rows = table.Children("tr");</code></example>
		/// <returns>The tags, or an empty array if none.</returns>
		public HtmlTag[] Children(string tag)
		{
			XmlNodeList children = Element.SelectNodes(tag);
			HtmlTag[] result = new HtmlTag[children.Count];
			for (int i = 0; i < children.Count; i++) 
			{
				result[i] = new HtmlTag((XmlElement)children[i], "<" + tag + "> child #" + i + " of " + Description);
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
		public HtmlTag Child(string tag)
		{
			HtmlTag[] tags = Children(tag);
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
				if (browser != null) document = browser.CurrentPage;

				string selector = "//*[@id='" + id + "']";
				if (xpath != null) selector = xpath;

				XmlNodeList nodes = document.SelectNodes(selector);
				if (nodes.Count > 1) throw new ApplicationException("Expected only one node to match xpath '" + selector + "' but " + nodes.Count + " nodes matched");
				if (nodes.Count == 0) return null;
				return (XmlElement)nodes[0];

				// BTW, we didn't use GetElementById here because it didn't work on 
				// pageForTestingOnly. Not sure why. No DOCTYPE, perhaps?
			}
		}

		private string Description
		{
			get
			{
				if (owner != null) 
				{
					try
					{
						return owner.Description;
					}
					catch (StackOverflowException)
					{
						return "? (" + owner.GetType() + ".Description malfunction: stack overflow)";
					}
				}
				else
				{
					return description;
				}
			}
		}

		internal class ElementNotVisibleException : ApplicationException
		{
			internal ElementNotVisibleException(string message) : base(message)
			{
			}
		}
	}
}
