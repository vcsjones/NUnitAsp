<%@ Page language="c#" Codebehind="HtmlInputCheckBoxTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlInputCheckBoxTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>HtmlInputCheckBoxTestPage</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<!-- All controls, including HTML server controls, must have an 'id' attribute when the page 
			 renders so that NUnitASP can find them. ASP.NET HTML input controls render a 'name' 
			 and an 'id' attribute from an ASP.NET 'id' attribute. However, non-server controls 
			 (HTML input controls without runat equal server) don't render anything additional, so
			 we must include an 'id' attribute when we enter the HTML, as we've done below for the 
			 three non-server controls. If we don't include the 'id' attribute then NUnitASP can't 
			 find the control and will choke when we try to test it. -->
		<form id="HtmlInputCheckBoxTestPage" method="post" runat="server">
			<input id="checkNotCheckedServer" type="checkbox" runat="server"><br>
			<input id="checkCheckedServer" type="checkbox" checked runat="server"><br>
			<input id="checkNotCheckedNotServer" name="checkNotCheckedNotServer" type="checkbox" /><br>
			<input id="checkCheckedNotServer" name="checkCheckedNotServer" type="checkbox" checked="" /><br> <!-- the 'checked' attribute must have an '=' and quotes so the NUnitASP XML parser doesn't choke -->
			<input id="checkVaryServer" type="checkbox" runat="server" NAME="checkVaryServer"><br>
			<input id="checkVaryNotServer" name="checkVaryNotServer" type="checkbox"><br>
			<input id="checkDisabled" name="checkDisabled" type="checkbox" disabled="disabled"><br>
			<asp:LinkButton ID="submit" Runat="server">Submit</asp:LinkButton></form>
	</body>
</HTML>
