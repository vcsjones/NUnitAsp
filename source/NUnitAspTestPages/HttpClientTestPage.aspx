<%@ Page language="c#" Codebehind="HttpClientTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HttpBrowserTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
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
			<br />
			<asp:LinkButton ID="dropCookie" OnClick="dropCookie_Click" Runat="server">Drop Cookie</asp:LinkButton>
			<br />
			<asp:LinkButton ID="dropCookieAndRedirect" OnClick="dropCookieAndRedirect_Click" Runat="server">Drop Cookie and Redirect</asp:LinkButton>
			<br />
			<asp:LinkButton ID="postBack" OnClick="postBack_Click" Runat="server">Post Back</asp:LinkButton>
			<br />
			<asp:LinkButton ID="dropCookieWithExpiry" OnClick="dropCookieWithExpiry_Click" Runat="server">Drop Cookie with Expiry</asp:LinkButton>
			<p>Test Parameter: [<asp:Label ID="testParm" Runat="server"></asp:Label>]
				<br />
				Test Cookie: [<asp:Label ID="cookie" Runat="server"></asp:Label>]
				<br />
				Post Back link: [<asp:Label ID="postBackStatus" Runat="server">Not Clicked</asp:Label>]
				<br />
				User Agent: [<asp:Label ID="userAgent" Runat="server"></asp:Label>]
 				<br />
				User Languages: [<asp:label ID="userLanguages" Runat="server"></asp:label>]
			</p>
		</form>
	</body>
</HTML>
