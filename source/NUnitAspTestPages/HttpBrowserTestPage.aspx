<%@ Page language="c#" Codebehind="HttpBrowserTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HttpBrowserTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<HTML>
	<head>
		<title>HttpBrowserTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</head>
	<body>
		<form id="HttpBrowserTestPage" method="post" runat="server">
			<asp:LinkButton ID="redirect" OnClick="redirect_Click" Runat="server">Redirect</asp:LinkButton>
			<br /><asp:LinkButton ID="dropCookie" OnClick="dropCookie_Click" Runat="server">Drop Cookie</asp:LinkButton>
			<br /><asp:LinkButton ID="dropCookieAndRedirect" OnClick="dropCookieAndRedirect_Click" Runat="server">Drop Cookie and Redirect</asp:LinkButton>
			<p>Test Cookie: [<asp:Label ID="cookie" Runat="server"></asp:Label>]</p>
		</form>
	</body>
</HTML>
