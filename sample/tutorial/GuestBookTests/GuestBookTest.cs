using System;
using NUnit.Framework;
using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.AspTester;

namespace GuestBookTests
{
	[TestFixture]
	public class GuestBookTest : WebFormTestCase
	{
		[Test]
		public void TestLayout()
		{
			TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
			TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
			ButtonTester save = new ButtonTester("save", CurrentWebForm);
			DataGridTester book = new DataGridTester("book", CurrentWebForm);

			Browser.GetPage("http://localhost/NUnitAsp/sample/tutorial/GuestBook/GuestBook.aspx");
			AssertVisibility(name, true);
			AssertVisibility(comments, true);
			AssertVisibility(save, true);
			AssertVisibility(book, false);
		}

		[Test]
		public void TestSave()
		{
			TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
			TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
			ButtonTester save = new ButtonTester("save", CurrentWebForm);
			DataGridTester book = new DataGridTester("book", CurrentWebForm);

			Browser.GetPage("http://localhost/NUnitAsp/sample/tutorial/GuestBook/GuestBook.aspx");
			name.Text = "Dr. Seuss";
			comments.Text = "One Guest, Two Guest!  Guest Book, Best Book!";
			save.Click();

			AssertEquals("name", "", name.Text);
			AssertEquals("comments", "", comments.Text);

			string[][] expected = new string[][]
			{
				new string[] {"Dr. Seuss", "One Guest, Two Guest!  Guest Book, Best Book!"}
			};
			AssertEquals("book", expected, book.TrimmedCells);
		}
	}
}
