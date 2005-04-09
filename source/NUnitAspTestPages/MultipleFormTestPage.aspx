<%@ Page language="c#" Codebehind="MultipleFormTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.WebFormTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MultipleFormTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="MultipleFormTestPage_1" method="post" runat="server">
			<p><asp:LinkButton ID="one" Runat="server">one</asp:LinkButton></p>
			<p>Submitted:
				<asp:Label ID="submitted" Runat="server"></asp:Label></p>
			<input type="hidden" id="one_tag" name="one_tag" value="one_tag" />
		</form>
		<form id="MultipleFormTestPage_2" method="post">
			<p><input id="two" type="submit" name="two" value="two" /></p>
			<p><input type="hidden" id="two_tag" name="two_tag" value="two_tag" /></p>
		</form>
	</body>
</HTML>
