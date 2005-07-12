<%@ Page language="c#" Codebehind="HtmlInputButtonTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlInputButtonTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>HtmlButtonTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>
		<form id="HtmlButtonTestPage" method="post" action="../RedirectionTarget.aspx">
			<input type="submit" id="button" name="button" value="This is a button." />
			<input type="submit" id="disabledButton" disabled="true" name="button" value="Disabled." />
		</form>
	</body>
</html>
