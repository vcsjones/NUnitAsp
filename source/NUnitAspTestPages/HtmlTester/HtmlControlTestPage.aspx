<%@ Page language="c#" Codebehind="HtmlControlTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlControlTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>HtmlControlTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="HtmlControlTestPage" method="post" runat="server">
			<a href="#" id="htmlHref" runat="server">normal <b>bold</b> [   spaces] &amp; punctuation</a>
			<asp:Label ID="rendered" Runat="server">unpopulated</asp:Label>
		</form>
	</body>
</HTML>
