<%@ Page language="c#" Codebehind="ButtonTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.ButtonTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ButtonTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="ButtonTestPage" method="post" runat="server">
			<asp:Button id="button" text="Button" runat="server" />
			<br />
			<asp:Button id="disabled" Enabled="False" Text="Disabled" runat="server" />
			<br />
			Click result:
			<asp:Label id="clickResult" runat="server" />
		</form>
	</body>
</HTML>
