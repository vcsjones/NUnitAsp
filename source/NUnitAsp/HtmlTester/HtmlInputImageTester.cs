using System;

namespace NUnit.Extensions.Asp.HtmlTester
{
	/// <summary>
	/// Tester for System.Web.UI.HtmlControls.HtmlInputImage
	/// </summary>
	public class HtmlInputImageTester : HtmlControlTester
	{
		/// <summary>
		/// Create a tester for an HTML tag.  Use this constructor
		/// for testing most tags.
		/// </summary>
		/// <param name="htmlId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		public HtmlInputImageTester(string htmlId) : base(htmlId)
		{
		}

		/// <summary>
		/// Create a tester for a server-side HTML control.  Use this constructor
		/// when the HTML tag you are testing has the "runat='server'" attribute.
		/// </summary>
		/// <param name="aspId">The ID of the control to test (look in the
		/// page's ASP.NET source code for the ID).</param>
		/// <param name="container">A tester for the control's container.  
		/// (In the page's ASP.NET source code, look for the tag that the
		/// control is nested in.  That's probably the control's
		/// container.  Use "CurrentWebForm" if you're not sure; it will
		/// probably work.)</param>
		public HtmlInputImageTester(string aspId, Tester container) : base(aspId, container)
		{
		}

		/// <summary>
		/// Click the image button in the upper-left corner (coordinates '0, 0').
		/// </summary>
		public void Click()
		{
			Click(0, 0);
		}

		/// <summary>
		/// Click the image button at the specified coordinates.
		/// </summary>
		public void Click(int x, int y)
		{
			if (Disabled) throw new ControlDisabledException(this);
			
			string name = Tag.OptionalAttribute("name");
			string value = Tag.OptionalAttribute("value");

			string prefix = "";
			if (name != null) prefix = name + ".";

			Form.Variables.Add(prefix + "x", x.ToString());
			Form.Variables.Add(prefix + "y", y.ToString());
			if (name != null && value != null) Form.Variables.Add(name, value);

			Form.Submit();
		}
	}
}
