<%@ Page language="c#" Codebehind="FormVariableTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.FormVariableTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/web/dtd/xhtml1-transitional.dtd">
<HTML>
	<head>
		<title>FormVariableTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</head>
	<body>
		<form id="FormVariableTestPage" method="post" runat="server">
			<asp:CheckBox ID="checkbox" Runat="server" Checked="True"></asp:CheckBox>
			<asp:Button ID="submit" Runat="server" Text="Submit"></asp:Button>
		</form>
	</body>
</HTML>
