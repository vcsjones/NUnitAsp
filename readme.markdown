NUnitASP: Read Me
=================

*   [Overview](#overview)
    * [How It Works](#howitworks)

<h2 id="overview">Overview</h2>
NUnitAsp is a tool for automatically testing ASP.NET web pages.  It's an extension to [NUnit](http://www.nunit.org), a tool for test-driven development in .NET.

Once you have an automated suite of tests, you'll never go back. It gives you incredible confidence in your code. That confidence allows you to code much faster, because you can make risky changes secure in the knowledge that your tests will catch any mistakes.

NUnitAsp is for unit testing ASP.NET code-behind only. It's meant for programmers, not QA teams, and it's not very good for QA-style acceptance tests. It only tests server-side logic. JavaScript and other client-side code is ignored. But if you're using ASP.NET, it's an essential part of your programmers' toolset.

NUnitAsp is freely available under the MIT license.

<h3 id="howitworks">How It Works</h3>

NUnitAsp is a class library for use within your NUnit tests.  It provides NUnit with the ability to download, parse, and manipulate ASP.NET web pages.

With NUnitAsp, your tests don't need to know how ASP.NET renders controls into HTML. Instead, you can rely on the NUnitASP library to do this for you, keeping your test code simple and clean.  For example, your tests don't need to know that a DataGrid control renders as an HTML table.  You can rely on NUnitAsp to handle the details.  This gives you the freedom to focus on functionality questions, like whether the DataGrid holds the expected values.

Simply speaking, NUnitAsp makes it very easy to unit test ASP.NET web pages.

```c#
[Test]
public void TestExample()
{
     // First, instantiate "Tester" objects:
     LabelTester label = new LabelTester("textLabel");
     LinkButtonTester link = new LinkButtonTester("linkButton");
     // Second, visit the page being tested:
     Browser.GetPage("http://localhost/example/example.aspx");
     // Third, use tester objects to test the page:
     Assert.AreEqual("Not clicked.", label.Text);
     link.Click();
     Assert.AreEqual("Clicked once.", label.Text);
     link.Click();
     Assert.AreEqual("Clicked twice.", label.Text);
}
```