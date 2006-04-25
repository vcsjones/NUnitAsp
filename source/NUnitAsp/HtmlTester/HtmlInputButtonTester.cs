using System;

namespace NUnit.Extensions.Asp.HtmlTester
{
	/// <summary>
	/// Tester for System.Web.UI.HtmlControls.HtmlInputButton
	/// </summary>
	public class HtmlInputButtonTester : HtmlButtonTester
	{
		#region Standard Constructors
		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlInputButtonTester(string htmlId) : base(htmlId)
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
		public HtmlInputButtonTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// Create a tester for an HTML tag using an XPath description.
		/// </summary>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A human-readable description of this tag (for error reporting).</param>
		public HtmlInputButtonTester(string xpath, string description) : base(xpath, description)
		{
		}

		/// <summary>
		/// Create a tester for an HTML tag that's on a page with multiple forms using
		/// an XPath description.
		/// </summary>
		/// <param name="xpath">The XPath description of the tag.</param>
		/// <param name="description">A human-readable description of this tag (for error reporting).</param>
		/// <param name="container">A tester for the control's container.  A WebFormTester
		/// will usually be most appropriate.</param>
		public HtmlInputButtonTester(string xpath, string description, Tester container) : base(xpath, description, container)
		{
		}
		#endregion

		/// <summary>
		/// Get the text on the button
		/// </summary>
		public override string Text
		{
			get
			{
				return Tag.Attribute("value");
			}
		}
	}
}
