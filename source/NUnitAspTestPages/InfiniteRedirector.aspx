<%@ Page language="c#" Codebehind="InfiniteRedirector.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.InfiniteRedirector" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/web/dtd/xhtml1-transitional.dtd">
<HTML>
	<head>
		<title>InfiniteRedirector</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</head>
	<body MS_POSITIONING="GridLayout">
		<form id="InfiniteRedirector" method="post" runat="server">
			This page is never displayed since it always redirects back to itself
			creating an infinte loop. This is intended to test how NUnitAsp will
			handle such situation.
		</form>
	</body>
</HTML>
