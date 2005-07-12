<%@ Page language="c#" Codebehind="RedirectionTarget.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.RedirectionTarget" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RedirectionTarget</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="RedirectionTarget" method="post" runat="server">
			<p>Redirected.</p>
			<p>Form Variables:</p>
			<asp:DataGrid ID="formVars" Runat="server"></asp:DataGrid>
		</form>
	</body>
</HTML>
