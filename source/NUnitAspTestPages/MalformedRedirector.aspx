<%@ Page language="c#" Codebehind="MalformedRedirector.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.MalformedRedirector" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>MalformedRedirector</title>
		<badtag>
	</head>
	<body>
		<form id="MalformedRedirector" method="post" runat="server">
			This is a bogus page that has HTML NUnitAsp can't parse.  But since no tests ever
			assert against this page -- it just immediately redirects -- NUnitAsp should still
			be able to handle it.
		</form>
	</body>
</html>
