<%@ Page language="c#" Codebehind="TextBoxTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.TextBoxTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/web/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>TextBoxTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body>
		<form id="TextBoxTestPage" method="post" runat="server">
			<asp:TextBox id="textBox" runat="server"></asp:TextBox>
			<asp:TextBox ID="multiline" Runat="server" TextMode="MultiLine">default</asp:TextBox>
			<asp:Button ID="postback" Text="postback" Runat="server"></asp:Button>
		</form>
	</body>
</HTML>
