<%@ Page language="c#" Codebehind="LabelTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnit.Extensions.Asp.Test.AspTester.LabelTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AspLabelTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="AspLabelTestPage" method="post" runat="server">
			<asp:Label ID="textLabel" runat="server"></asp:Label>
			<br />[<asp:Label ID="spaceLabel" Runat="server">foo </asp:Label>]
			<br />
			<br /><asp:Label ID="formattedLabel" Runat="server">a <i>HTML</i> tag</asp:Label>
			<br />
			<asp:Label ID="outerLabel" Runat="server">outer
				<asp:Label ID="innerLabel" Runat="server">inner</asp:Label>
			</asp:Label>
		</form>
	</body>
</HTML>
