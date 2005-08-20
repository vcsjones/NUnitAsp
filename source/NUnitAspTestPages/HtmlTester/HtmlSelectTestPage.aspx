<%@ Page language="c#" Codebehind="HtmlSelectTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlSelectTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>HtmlSelectTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>
		<form id="HtmlSelectTestPage" method="post" runat="server">
			<select id="default">
			</select>
			<br />
			<select id="nonDefault" size="5" multiple="true">
				<option>one</option>
				<option>two</option>
				<option selected="true">three</option>
				<option>four</option>
				<option>five</option>
			</select>
		</form>
	</body>
</html>
