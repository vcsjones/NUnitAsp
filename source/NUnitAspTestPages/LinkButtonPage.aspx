<%@ Page language="c#" Codebehind="LinkButtonPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTest.LinkButtonPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>PostbackPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body id="LinkButton" MS_POSITIONING="GridLayout">
		<form id="PostbackPage" method="post" runat="server">
			<asp:LinkButton id="link" style="Z-INDEX: 101; LEFT: 92px; POSITION: absolute; TOP: 61px" runat="server">Click Me</asp:LinkButton>
			<asp:Label id="status" style="Z-INDEX: 102; LEFT: 104px; POSITION: absolute; TOP: 104px" runat="server" Width="64px" Height="27px">unclicked</asp:Label>
		</form>
	</body>
</HTML>
