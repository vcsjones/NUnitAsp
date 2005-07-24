<%@ Page language="c#" Codebehind="HtmlInputImageTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlInputImageTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>HtmlInputImageTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>
		<form id="HtmlInputImageTestPage" method="post" action="../RedirectionTarget.aspx">
			<input id="image" name="image" type="image" src="nonexistant.jpg" value="foo" />
			<input id="noName" type="image" src=nonexistant.jpg" value="bar" />
			<input id="disabled" disabled="true" type="image" src="nonexistant.jpg" />
		</form>
	</body>
</html>
