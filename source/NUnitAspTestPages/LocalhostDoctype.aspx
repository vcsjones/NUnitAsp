<%@ Page language="c#" Codebehind="LocalhostDoctype.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.LocalhostDoctype" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<!-- Old versions of NUnitAsp did something incredibly stupid: They recommended
that you use the above DOCTYPE.  We've fixed that oversight, but now we want programs
that use the above DOCTYPE to get fixed.  This page exists so we can test that
functionality. -->

<html>
	<head>
		<title>LocalhostDoctype</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body MS_POSITIONING="GridLayout">
		<form id="LocalhostDoctype" method="post" runat="server">
			<!-- This control is here so we have something to assert against. -->
			<asp:Label ID="label" Runat="server"></asp:Label>
		</form>
	</body>
</html>
