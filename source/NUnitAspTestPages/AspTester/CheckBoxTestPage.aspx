<%@ Page language="c#" Codebehind="CheckBoxTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.CheckBoxTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>CheckBoxTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body>
		<form id="CheckBoxTestPage" method="post" runat="server">
			<asp:CheckBox id="checkBox" runat="server" Text="Test me"></asp:CheckBox>
			<br /><asp:LinkButton ID="submit" Runat="server">Submit</asp:LinkButton>
		</form>
	</body>
</HTML>
