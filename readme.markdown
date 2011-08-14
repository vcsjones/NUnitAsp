NUnitASP: Read Me
=================

*   [Overview](#overview)
    * [How It Works](#howitworks)
	* [Credits and History](#credits)

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

NUnitAsp can test complex web sites involving multiple pages and nested controls. The common ASP.NET controls are supported (see complete list below), and support for additional controls is easy to add.

<h2 id="credits">Credits and History</h2>
[Kevin Jones](http://vcsjones.com) Is currently maintaining this NUnitAsp fork. He believes while others may have moved onto other frameworks, those that continue to use it on should be able to report bugs, as well as implementing the occasional feature.

[James Shore](http://www.jamesshore.com) worked on NUnitAsp while leading a team creating a commercial web application in the beta days of ASP.NET.  Unwilling to develop without the safety net of test-driven development, he took over Brian Knowles' open-source application for testing ASP.NET and updated it to support full TDD of ASP.NET code-behind.  Since releasing the first version of NUnitAsp on SourceForge in 2002, he's seen it grow into an application that's downloaded thousands of times every month.

Since the original release, dozens of people have contributed time and effort to NUnitAsp.  Of particular note is Levi Khatskevitch, who joined the team in November 2003 and coordinated the integration of patches and contributed many new features and improvements to version 1.4 and 1.5.  For a complete list of contributors, see our change log.