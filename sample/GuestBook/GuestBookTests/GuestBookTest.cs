using System;
using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.AspTester;

namespace GuestBookTests
{
	public class GuestBookTest : WebFormTestCase
	{
		public GuestBookTest(string name) : base (name)
		{
		}

		public void TestLayout()
		{
			TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
			TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
			DataGridTester book = new DataGridTester("book", CurrentWebForm);

			Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");

			AssertVisibility(name, true);
			AssertVisibility(comments, true);
			AssertVisibility(book, false);
		}
	}
}
