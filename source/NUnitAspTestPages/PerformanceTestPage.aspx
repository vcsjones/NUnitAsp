<%@ Page language="c#" Codebehind="PerformanceTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.PerformanceTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <head>
    <title>NUnitAsp - ASP.NET unit testing: Tutorial</title>
		<link rel="stylesheet" type="text/css" href="../style.css" />
  </head>
  <body>
	<form runat="server">
		<asp:TextBox id="textBox1" Runat="server">textBox1</asp:TextBox>
		<asp:TextBox id="textBox2" Runat="server">textBox2</asp:TextBox>
		<asp:TextBox id="textBox3" Runat="server">textBox3</asp:TextBox>
		<asp:TextBox id="textBox4" Runat="server">textBox4</asp:TextBox>
		<asp:TextBox id="textBox5" Runat="server">textBox5</asp:TextBox>
		<asp:Button ID="button" Runat="server" Text="Button"></asp:Button>
	</form>
		<p class="title">NUnitAsp
		<br /><small>ASP.NET unit testing</small></p>

		<table class="layout">
			<tr>
				<td>
					<div class="menu">
						<p><a href="../download.html"><b>Download</b></a></p>
						<p>
							<a href="../index.html">Overview</a>
							<br /><a href="../documentation.html">Documentation</a>
							<br /><a href="../support.html">Support</a>
							<br /><a href="../consulting.html">Training and Consulting</a>
						</p>
						<p><a href="../contribute.html">Contribute</a></p>
						<p><A href="http://sourceforge.net/projects/nunitasp/"><IMG src=
						"http://sourceforge.net/sflogo.php?group_id=49940" width="88" height="31" border="0" alt="SourceForge Logo" /></A></p>
					</div>
				</td>
				<td>
					<div class="content">
						<h1>Test-Driven Development with NUnitAsp
						<br /><small>The NUnitAsp Tutorial</small></h1>

						<p>NUnitAsp is based on the principle that testing web pages should use the same concepts as creating them. When you use NUnitAsp, you will be using classes and IDs similar to those you used when creating the web page you are testing.</p>

						<p>In this tutorial, we'll walk you through the process of creating a simple one-page web application and tests. We assume you're using Visual Studio .NET and Internet Information Server. The "Guestbook" solution we create in this tutorial is located in the \samples directory of the NUnitAsp download. Advanced users may get started more quickly by jumping straight to the <a href="../quickstart.html">QuickStart Guide</a>.</p>

						<p>This tutorial was last updated for NUnitAsp v1.2.</p>


						<h2>About the Sample Application</h2>

						<p>The application we'll be creating in this tutorial is a simple on-line guestbook.  It allows users to enter a name and comment, which is then displayed on the page.</p>

						<div class="figure" align="center">
							<p>Screen shot goes here.</p>
						</div>


						<h2>Create a Test Project</h2>

						<div class="figure" style="float:right">
							<img src="create-project.gif" alt="Figure" />
							<br />Create a project...
						</div>

						<p>Start by creating a class library project to contain your tests. We created a Visual Studio .NET solution named "GuestBook" and a C# class library project named "GuestBookTests."  Create a text fixture class that describes what you're testing.  We used "GuestBookTest."  Finally, add references to the NUnitAsp and NUnit frameworks.  They're in the \bin directory of the NUnitAsp download.</p>

						<p>Once your project and text fixture class have been created, modify your class to extend WebFormTestCase.  In a normal NUnit test fixture, you would extend TestCase.  With NUnitAsp, you extend WebFormTestCase.  WebFormTestCase provides a "Browser" property for loading web pages.  It also provides some handy extra assertions.</p>

						<p>As with a normal NUnit test fixture, you'll need to update your constructor to take a string argument and pass that argument to the base class.  You'll also need to include two common NUnitAsp namespaces, NUnit.Extensions.Asp and NUnit.Extensions.Asp.AspTester, with a "using" line.</p>

						<p>We're almost ready to try it out.  The last thing we need is a test.  The simplest possible test is an empty test: Add a public method that starts with the word "Test" and has no body. We used "TestNothing."</p>

						<div class="code">
							<span class="keyword">using</span> System;
							<br /><span class="keyword">using</span> NUnit.Extensions.Asp;
							<br /><span class="keyword">using</span> NUnit.Extensions.AspTester;
							<br />
							<br /><span class="keyword">namespace</span> GuestBookTests
							<br />{
							<br />&nbsp;&nbsp;&nbsp;<span class="keyword">public class</span> GuestBookTest :
							WebFormTestCase
							<br />&nbsp;&nbsp;&nbsp;{
							<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="keyword">public</span> GuestBookTest(<span class="keyword">string</span> name) : <span class="keyword">base</span>(name)
							<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{
							<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}
							<br />
							<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue">public void</font> TestNothing()
							<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{
							<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}
							<br />&nbsp;&nbsp;&nbsp;}
							<br />}
						</div>

						<p>Now you have a working test!  Admittedly, it doesn't do much.  We'll expand it later.  For now, let's run it to make sure everything is set up properly.</p>

						<p>If you're using VS.NET, you can configure your tests to automatically run when you press F5. Right-click your test project (in our case, "GuestBookTests") and select Properties. Modify the properties of the "Debugging" pane in the "Configuration Properties" folder as follows:</p>

						<ul>
							<li><b>Debug Mode:</b> "Program"</li>
							<li><b>Start Application:</b> Location of NUnitGui.exe (in the "\bin" directory of the NUnitAsp download)</li>
							<li><b>Command Line Arguments:</b> Full path to test project's DLL</li>
						</ul>

						<div class="figure" align="center">
							<img src="modify-properties.gif" alt="Modify the project properties..." />
						</div>

						<p>You can see this test pass by pressing "F5" and clicking the "Run" button on the NUnit GUI.</p>

						<div class="figure" align="center">
							<img src="run-nunit.gif" alt="Run the tests..." />
						</div>


						<h2>Create a Real Test</h2>

						<p>Now we're ready to start testing. That's right, we're going to write our tests <i>first</i>.  It's a style of programming called "Test-Driven Development," and it's part of the Extreme Programming methodology that programs like NUnit were created to support.  In test-driven development, you create tests for features that you want, then write production code to make the tests pass.  By doing so, you increase test coverage and reduce the amount of unnecessary production code.</p>

						<p>In our case, the feature we want is a guest book.  Testing every aspect of a guest book would take far too long, but we can test to see if the web page even exists.  That's a broad test, but we can refine from there.  We don't want to take too long between running tests -- the goal is to check our code frequently so we always know we're on the right track.</p>

						<p>First we'll modify our empty test to load the Web page we're going to write.  WebFormTestCase, our parent class, provides a "Browser" object for getting web pages. Call "Browser.GetPage(<i>web page url</i>)" to load the page.  We also changed the name of the test to something a little more meaningful.</p>

						<div class="code">
							<span class="keyword">public void</span> TestLayout()
							<br />{
							<br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
							<br />}
						</div>

						<div class="figure" style="float:right">
							<img src="create-webproject.gif" alt="Figure"/>
							<br />Create a web project...
						</div>

						<p>Now press "F5" and run the test. It will fail, because the page doesn't exist yet, but we want to run it anyway. The reason is so that we find out immediately if things aren't working the way we expect. The sooner we know about problems, the easier it is to fix them. When testing with NUnitAsp (and NUnit), get into the habit of writing a small test, watching it fail, writing a few lines of code, and watching the test pass. You should repeat this cycle every few minutes. If you do, you'll never get into a situation where you spend a lot of time debugging.</p>

						<p>The test fails, as expected, so now we want to make it pass. Create a web project and the corresponding page. In our case, we created "GuestBookPages" as our web project and "GuestBook.aspx" as our web page.</p>

						<p>If you run the test again, you'll see that it still doesn't pass. That's because NUnitAsp requires web pages to be in XHTML. There's three changes that you have to make in order for the pages to parse correctly:</p>

						<table>
							<tr>
								<th>Error Message</th>
								<th>Solution</th>
							</tr>
							<tr>
								<td>This is an unexpected token. Expected 'QUOTE'.</td>
								<td>
									Modify the DOCTYPE header to point to the NUnitAsp XHTML DTD:
									<br /><code>&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot;
									&quot;http://localhost/NUnitAsp/web/dtd/xhtml1-transitional.dtd&quot;&gt;</code>
								</td>
							</tr>
							<tr>
								<td>The 'x' character, hexidecimal value 0x78, cannot be included in a name.</td>
								<td>Make sure the &lt;HTML&gt; and &lt;/HTML&gt; tags are upper-case.</td>
							</tr>
							<tr>
								<td>The '<i>tag 1</i>' start tag on line '<i>##</i>' does not match the end tag of <i>tag 2</i>', line '<i>##</i>'</td>
								<td>Close all stand-alone tags -- particularly "meta" and "br".</td>
							</tr>
						</table>

						<p>After these changes were made, our code looked like this:</p>

						<div class="html">
							<span class="asp">&lt;%@ Page language=&quot;c#&quot; Codebehind=&quot;GuestBook.aspx.cs&quot; AutoEventWireup=&quot;false&quot; Inherits=&quot;GuestBook.GuestBook&quot; %&gt;</span>
							<br /><span class="literal">&lt;!</span><span class="tag">DOCTYPE</span> <span class="attribute">HTML PUBLIC</span> <span class="literal">&quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://localhost/NUnitAsp/dtd/xHTML1-transitional.dtd&quot;&gt;</span>
							<br />
							<font color="blue">&lt;</font><font color="brown">HTML</font><font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">HEAD</font><font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">title</font><font color="blue">&gt;</font>GuestBook<font color="blue"><font color="blue">&lt;</font>/</font><font color="brown">title</font><font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">meta</font>
							<font color="red">name</font><font color="blue">=&quot;GENERATOR&quot;</font> <font color="red">
								Content</font><font color="blue">=&quot;Microsoft Visual Studio 7.0&quot; </font>
							/<font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">meta</font>
							<font color="red">name</font><font color="blue">=&quot;CODE_LANGUAGE&quot;</font>
							<font color="red">Content</font><font color="blue">=&quot;C#&quot;</font> /<font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">meta</font>
							<font color="red">name</font><font color="blue">=&quot;vs_defaultClientScript&quot;</font>
							<font color="red">content</font><font color="blue">=&quot;JavaScript&quot;</font>
							/<font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">meta</font>
							<font color="red">name</font><font color="blue">=&quot;vs_targetSchema&quot;</font>
							<font color="red">content</font><font color="blue">=&quot;http://schemas.microsoft.com/intellisense/ie5&quot;</font>
							/<font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;<font color="blue"><font color="blue">&lt;</font>/</font><font color="brown">HEAD</font><font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">body</font> <font color="red">
								MS_POSITIONING</font><font color="blue">=&quot;GridLayout&quot;</font><font color="blue">&gt;</font>
							<br />
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue">&lt;</font><font color="brown">form</font>
							<font color="red">id</font><font color="blue">=&quot;GuestBook&quot;</font> <font color="red">
								method</font><font color="blue">=&quot;post&quot;</font> <font color="red">runat</font><font color="blue">=&quot;server&quot;&gt;</font>
								<br />
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue"><font color="blue">&lt;</font>/</font><font color="brown">form</font><font color="blue">&gt;</font>
								<br />
								&nbsp;&nbsp;&nbsp;<font color="blue"><font color="blue">&lt;</font>/</font><font color="brown">body</font><font color="blue">&gt;</font>
								<br />
								<font color="blue"><font color="blue">&lt;</font>/</font><font color="brown">HTML</font><font color="blue">&gt;</font>
						</div>


						<h2>Test the Layout</h2>

						<p>The next thing we'll test is the layout of our page.  We don't test the look of our web page &mdash; we don't want to have to update our tests every time someone changes a font &mdash; but we do test the functionality.  The first step towards doing that is just making sure the right components are on the page.</p>

						<p>In our guest book application, we just want four components: a text field for entering a name, a text field for entering a comment, a button to submit the data, and a datagrid for displaying what people have entered.</p>

						<p class="figure">Hand-drawn sketch of screen layout here</p>

						<p>To test these components, we have to create NUnitAsp "tester" objects.  A tester is an object that corresponds to an ASP.NET control you want to test.  You can instantiate the tester at any time &mdash; even before the page is loaded, if you like.  When you instantiate it, you tell the tester what the name of the ASP.NET control it's supposed to test and where it's located.  Since we're going to test four controls on our guest book, we need to create four tester objects.</p>

						<div class="code">
							<span class="demph">public void TestLayout()
							<br />{</span>
							<br /><span class="emph">&nbsp;&nbsp;&nbsp;TextBoxTester name = <span class="keyword">new</span> TextBoxTester("name", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester comments = <span class="keyword">new</span> TextBoxTester("comments", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;ButtonTester save = <span class="keyword">new</span> ButtonTester("save", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;DataGridTester book = <span class="keyword">new</span> DataGridTester("book", CurrentWebForm);</span>
							<br />
							<span class="demph"><br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
							<br />}</span>
						</div>

						<p>Let's take a closer look at the first declaration:</p>

						<p>
							<table>
								<tr>
									<th>Code Fragment</th>
									<th>Meaning</th>
								</tr>
								<tr>
									<td><code><b>TextBoxTester name = <span class="keyword">new</span> TextBoxTester</b><span class="demph">("name", CurrentWebForm);</span></code></td><td>The object named 'name' will test a text box.</td>
								</tr>
								<tr>
									<td><code><span class="demph">TextBoxTester name = new TextBoxTester(</span><b>"name"</b><span class="demph">, CurrentWebForm);</span></code></td>
									<td>The ASP.NET id of the text box is "name."</td>
								</tr>
								<tr>
									<td><code><span class="demph">TextBoxTester name = new TextBoxTester("name", </span><b>CurrentWebForm</b><span class="demph">);</span></code></td>
									<td>Look for the control on the web form currently loaded by the browser.</td>
								</tr>
							</table>
						</p>

						<p>Now that we have the testers, we can assert that their controls are visible on the page.</p>

						<div class="code">
							<span class="demph">public void TestLayout()
							<br />{
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;ButtonTester save = new ButtonTester("save", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;DataGridTester book = new DataGridTester("book", CurrentWebForm);
							<br />
							<br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");</span>
							<br /><span class="emph">&nbsp;&nbsp;&nbsp;AssertVisibility(name, <span class="keyword">true</span>);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(comments, <span class="keyword">true</span>);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(save, <span class="keyword">true</span>);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(book, <span class="keyword">true</span>);</span>
							<span class="demph"><br />}</span>
						</div>

						<p>We've just told NUnitAsp to assert that the four controls are visible on the page.</p>

						<p>
							<table>
								<tr>
									<th>Code Fragment</th>
									<th>Meaning</th>
								</tr>
								<tr>
									<td><code><b>AssertVisibility</b><span class="demph">(name, true);</span></code></td>
									<td>Call the method in WebFormTestCase that checks control visibility.</td>
								</tr>
								<tr>
									<td><code><span class="demph">AssertVisibility(</span><b>name</b><span class="demph">, true);</span></code></td>
									<td>Check the visibility using the 'name' tester.  The tester knows its control is a TextBox whose ID is 'name.'</td>
								</tr>
								<tr>
									<td><code><span class="demph">AssertVisibility(name, </span><span class="keyword">true</span>);</code></td>
									<td>The control should be visible.</td>
								</tr>
							</table>
						</p>

						<p>Go ahead and run the tests.  You should get an error from NUnitAsp:</p>

						<blockquote>
							<code class="error">name control should be visible (HTML ID: name; ASP location: TextBoxTester 'name' in web form 'GuestBook')</code>
						</blockquote>

						<p>This error message tells you three things:</p>

						<p>
							<ul>
								<li>The 'name' control wasn't visible</li>
								<li>In the browser's 'View Source' HTML, the control is named 'name'</li>
								<li>In the ASP.NET source code, the control is a text box named 'name' and it's on a form named 'GuestBook'</li>
							</ul>
						</p>

						<p>In this simple example, all that information seems redundant.  However, in ASP.NET, controls can be nested within other controls.  The ID you give a control in ASP.NET can change into a completely different ID in HTML.  The information in the error message will help you track down all the information you need to solve problems.</p>

						<p>Let's make the tests pass by adding the four controls.  We won't worry about looks just yet.</p>

						<div class="html">
							&lt;form id=&quot;GuestBook&quot; method=&quot;post&quot; runat=&quot;server&quot;&gt;
							<br />&nbsp;&nbsp;&nbsp;&lt;asp:TextBox ID=&quot;name&quot; Runat=&quot;server&quot;&gt;&lt;/asp:TextBox&gt;
							<br />&nbsp;&nbsp;&nbsp;&lt;asp:TextBox ID=&quot;comments&quot; Runat=&quot;server&quot;&gt;&lt;/asp:TextBox&gt;
							<br />&nbsp;&nbsp;&nbsp;&lt;asp:Button ID=&quot;save&quot; RunAt=&quot;server&quot; Text=&quot;Save&quot;&gt;&lt;/asp:Button&gt;
							<br />&nbsp;&nbsp;&nbsp;&lt;asp:DataGrid ID=&quot;book&quot;&gt;&lt;/asp:DataGrid&gt;
							<br />&lt;/form&gt;
						</div>

						<p>But the tests didn't pass!  The data grid wasn't visible.  This is a perfect example of why we run the tests so often.  Sometimes we get surprised.</p>

						<p>The data grid probably didn't show up because we haven't put any data in it yet.  Well, after thinking about it, that's actually the behavior we want... when there's no data, don't display any results.  Let's update the test to reflect this.</p>

						<div class="code">
							<span class="demph">public void TestLayout()
							<br />{
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;DataGridTester book = new DataGridTester("book", CurrentWebForm);
							<br />
							<br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(name, true);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(comments, true);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(save, true);</span>
							<br /><span class="emph">&nbsp;&nbsp;&nbsp;AssertVisibility(book, <span class="keyword">false</span>);</span>
							<span class="demph"><br />}</span>
						</div>
						<p>Now the tests pass.</p>


						<h2>Test the Behavior</h2>

						<p>We have a web page, but it doesn't do anything yet.  The next thing we want to see is that entering something updates the guest book.  Naturally, we start by writing a test.</p>

						<div class="code">
							<span class="demph">public void TestLayout()
							<br />{
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;DataGridTester book = new DataGridTester("book", CurrentWebForm);
							<br />
							<br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(name, true);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(comments, true);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(save, true);
							<br />&nbsp;&nbsp;&nbsp;AssertVisibility(book, false);
							<br />}</span>
							<br />
							<br /><span class="emph">public void TestSave()</span>
							<span class="demph"><br />{
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;DataGridTester book = new DataGridTester("book", CurrentWebForm);
							<br />
							<br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");</span>
							<br /><span class="emph">&nbsp;&nbsp;&nbsp;name.Text = &quot;Dr. Seuss&quot;;
							<br />&nbsp;&nbsp;&nbsp;comments.Text = &quot;One Guest, Two Guest!  Guest Book, Best Book!&quot;;
							<br />&nbsp;&nbsp;&nbsp;save.Click();</span>
							<span class="demph"><br />}</span>
						</div>

						<p>We've created a new test named 'TestSave().'  It's has the same tester set up and page load that TestLayout() does, but then it does something different: It assigns values to the web page and presses the 'Save' button.</p>

						<p>
							<table>
								<tr>
									<th>Code Fragment</th>
									<th>Meaning</th>
								</tr>
								<tr>
									<td><code>name.Text = &quot;Dr. Seuss&quot;;</code></td><td>Set the text of the 'name' text box to 'Dr. Seuss.'</td>
								</tr>
								<tr>
									<td><code>comments.Text = &quot;One Guest, Two Guest!  Guest Book, Best Book!&quot;;</code></td><td>Set the text of the 'comments' text box.</td>
								</tr>
								<tr>
									<td><code>save.Click();</code></td>
									<td>Click the 'save' button</td>
								</tr>
							</table>
						</p>

						<p>Stop and take another look at that code.  It's the heart of what NUnitAsp is all about.  Although you're not actually working with an ASP.NET page &mdash; actually, the page you got was raw HTML &mdash; you can manipulate the controls on it with simple ASP.NET-like properties and methods.  Pretty cool, and very easy.</p>

						<p>Run the test.  It passes, unsurprisingly.  We're filling in the text and clicking 'save,' but we're not actually asserting anything.  Let's fix that.  We'll start by asserting that the name and comment fields are reset to be blank after you save.</p>

						<div class="code">
							<br /><span class="demph">public void TestSave()
							<br />{
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;DataGridTester book = new DataGridTester("book", CurrentWebForm);
							<br />
							<br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
							<br />&nbsp;&nbsp;&nbsp;name.Text = &quot;Dr. Seuss&quot;;
							<br />&nbsp;&nbsp;&nbsp;comments.Text = &quot;One Guest, Two Guest!  Guest Book, Best Book!&quot;;
							<br />&nbsp;&nbsp;&nbsp;save.Click();</span>
							<br />
							<span class="emph"><br />&nbsp;&nbsp;&nbsp;AssertEquals("name", "", name.Text);
							<br />&nbsp;&nbsp;&nbsp;AssertEquals("comments", "", comments.Text);</span>
							<span class="demph"><br />}</span>
						</div>

						<p>The AssertEquals() method is just a standard NUnit assertion, but here's the breakdown in case you're not familiar with it:</p>

						<p>
						<table>
								<tr>
									<th>Code Fragment</th>
									<th>Meaning</th>
								</tr>
								<tr>
									<td><code><span class="emph">AssertEquals</span><span class="demph">(&quot;name&quot;, &quot;&quot;, name.Text);</span></code></td><td>Assert that two objects are equal.</td>
								</tr>
								<tr>
									<td><code><span class="demph">AssertEquals(</span><span class="emph">&quot;name&quot;</span><span class="demph">, &quot;&quot;, name.Text);</span></code></td><td>The text to use in the failure message.</td>
								</tr>
								<tr>
									<td><code><span class="demph">AssertEquals(&quot;name&quot;, </span><span class="emph">&quot;&quot;</span><span class="demph">, name.Text);</span></code></td><td>The expected results (an empty string).</td>
								</tr>
								<tr>
									<td><code><span class="demph">AssertEquals(&quot;name&quot;, &quot;&quot;, </span><span class="emph">name.Text</span><span class="demph">);</span></code></td><td>The actual results (the text in the 'name' text box).</td>
								</tr>
						</table>
						</p>

						<p>When you run the test, it fails!</p>

						<blockquote>
							<code class="error">name expected:&lt;&gt; but was:&lt;Dr. Seuss&gt;</code>
						</blockquote>

						<p>Were you surprised?  I was.  I had forgotten that the default behavior in ASP.NET is to preserve the values of its controls.  We fixed the problem by adding some code into the web page's code-behind to always initialize the text boxes to be blank.</p>

						<div class="code">
							<span class="keyword">private void </span>Page_Load(<span class="keyword">object</span> sender, System.EventArgs e)
							<br />{
							<br />&nbsp;&nbsp;&nbsp;name.Text = "";
							<br />&nbsp;&nbsp;&nbsp;comments.Text = "";
							<br />}
						</div>

						<p>Notice how similar the code-behind is to the tests?  That's intentional.  NUnitAsp will generally have the same methods on its testers that are on real ASP.NET objects.</p>

						<p>Now for the meat of the test.  We want to assert that the 'book' data grid is populated with the user's name and comments.  Since checking the contents of a data grid is a common need, there's functionality to make this easy.</p>

						<p>NUnitAsp allows you to treat data grid as an array of cells.  Actually, it's an array of string arrays.  The string arrays in the outer array correspond to the rows of the data grid, and the strings in the inner arrays correspond to the cells of the data grid.  So a string array that looks like this:</p>

						<div class="code">
							<span class="keyword">string</span>[][] expected = <span class="keyword">new string</span>[][]
							<br />{
							<br />&nbsp;&nbsp;&nbsp;<span class="keyword">new string</span>[] {"Dr. Seuss", "One Guest, Two Guest!  Guest Book, Best Book!"},
							<br />&nbsp;&nbsp;&nbsp;<span class="keyword">new string</span>[] {"Dr. Freud", "Nice slip you have there."}
							<br />};
						</div>

						<p>Corresponds to a data grid table that looks like this:</p>

						<p>
							<table>
								<tr>
									<td>Dr. Seuss</td>
									<td>One Guest, Two Guest!  Guest Book, Best Book!</td>
								</tr>
								<tr>
									<td>Dr. Freud</td>
									<td>Nice slip you have there.</td>
								</tr>
							</table>
						</p>

						<p>Unsurprisingly, DataGridTester has a method that returns the data grid's contents in an array like that.  WebFormTestCase also provides several assertion methods that allow you to assert things about the contents of this kind of array.  We'll use that to assert that the guest book contains Dr. Seuss' comments after he presses the 'Save' button.</p>

						<div class="code">
							<br /><span class="demph">public void TestSave()
							<br />{
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester name = new TextBoxTester("name", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;TextBoxTester comments = new TextBoxTester("comments", CurrentWebForm);
							<br />&nbsp;&nbsp;&nbsp;DataGridTester book = new DataGridTester("book", CurrentWebForm);
							<br />
							<br />&nbsp;&nbsp;&nbsp;Browser.GetPage("http://localhost/NUnitAsp/sample/GuestBook/GuestBook/GuestBook.aspx");
							<br />&nbsp;&nbsp;&nbsp;name.Text = &quot;Dr. Seuss&quot;;
							<br />&nbsp;&nbsp;&nbsp;comments.Text = &quot;One Guest, Two Guest!  Guest Book, Best Book!&quot;;
							<br />&nbsp;&nbsp;&nbsp;save.Click();
							<br />
							<br />&nbsp;&nbsp;&nbsp;AssertEquals("name", "", name.Text);
							<br />&nbsp;&nbsp;&nbsp;AssertEquals("comments", "", comments.Text);</span>
							<br />
							<br />&nbsp;&nbsp;&nbsp;<span class="emph"><span class="keyword">string</span>[][] expected = <span class="keyword">new string</span>[][]
							<br />&nbsp;&nbsp;&nbsp;{
							<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="keyword">new string</span>[] {"Dr. Seuss", "One Guest, Two Guest!  Guest Book, Best Book!"}
							<br />&nbsp;&nbsp;&nbsp;};
							<br />&nbsp;&nbsp;&nbsp;AssertEquals("book", expected, book.TrimmedCells);</span>
							<span class="demph"><br />}</span>
						</div>

						<p class="end">The remainder of the tutorial is unfinished.</p>


						<h2>Test Boundary Conditions</h2>


						<h2>Test Performance</h2>


						<h2>Make it Pretty</h2>


						<h2>Conclusion</h2>

						<p>As you start writing your own tests with NUnitAsp, try following the pattern used in this tutorial.  To write this tutorial, I simply wrote down what I did while creating the sample application.  Other than the limited scope of the application, nothing about this tutorial is simplified.  Experienced test-driven development programmers really do run tests every couple of minutes, even when they know the results.  Once you get used to it, it evolves into a predictable rhythm:</p>

						<blockquote>
							Write test...
							<br />...watch test fail.
							<br />Write code...
							<br />...watch test pass.
							<br />Write test...
							<br />...watch test fail.
							<br />Write code...
							<br />...watch test pass.
							<br />Write test...
							<br />...watch test fail.
							<br />Write code...
							<br />...watch test pass.
						</blockquote>

						<p>Every so often, the rhythm will be broken.  A test will fail when it shouldn't, or fail for the wrong reason, or even pass when it shouldn't.  If you've been running tests every few minutes, then the defect is in the last few minutes' work... it only takes a few seconds to figure out your mistake.  If you haven't been running tests often, though, you'll have to spend a lot more time debugging.</p>

						<p>These are the steps I really do follow when creating new web pages:</p>

						<ul>
							<li>Create a test class.</li>
							<li>Create an empty test.
							<br /><i>(Watch test pass)</i></li>
							<li>Modify the test to navigate to the web page.
							<br /><i>(Watch test fail)</i></li>
							<li>Create the web page.
							<br /><i>(Watch test pass)</i></li>
							<li>Test the layout.
							<br /><i>(Watch test fail)</i></li>
							<li>Add controls to the web page.
							<br /><i>(Watch test pass)</i></li>
							<li>Repeat the following until all desired behavior is present:
								<ul>
									<li>Test a small amount of behavior
									<br /><i>(Watch test fail)</i></li>
									<li>Implement a small amount of behavior
									<br /><i>(Watch test pass)</i></li>
								</ul>
							</li>
							<li>Repeat the following until all boundary conditions are handled:
								<ul>
									<li>Test a single boundary condition
									<br /><i>(Watch test fail)</i></li>
									<li>Fix a single boundary condition
									<br /><i>(Watch test pass)</i></li>
								</ul>
							</li>
							<li>Optional: Test and optimize performance.</li>
							<li>Make it pretty.</li>
						</ul>

						<p>Test-driven development is a relaxing, low-risk approach to software development.  Once you get used to it, it's fast and reliable.  Follow the pattern above, and you'll spend more time adding features and less time fixing defects.</p>

						<p class="end">by Jim Little</p>
					</div>
				</td>
			</tr>
		</table>
  </body>
</HTML>