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
			ButtonTester save = new ButtonTester("save", CurrentWebForm);
			DataGridTester book = new DataGridTester("book", CurrentWebForm);

			Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
			AssertVisibility(name, true);
			AssertVisibility(comments, true);
			AssertVisibility(save, true);
			AssertVisibility(book, false);
		}

		public void TestSave()
		{
			TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
			TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
			ButtonTester save = new ButtonTester("save", CurrentWebForm);
			DataGridTester book = new DataGridTester("book", CurrentWebForm);

			Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
			name.Text = "Dr. Seuss";
			comments.Text = "One Guest, Two Guest!  Guest Book, Best Book!";
			save.Click();

			AssertEquals("name", "", name.Text);
			AssertEquals("comments", "", comments.Text);
		}
	}
}
