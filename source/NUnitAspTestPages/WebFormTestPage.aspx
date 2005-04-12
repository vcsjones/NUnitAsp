<%@ Page language="c#" Codebehind="WebFormTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.WebFormTestPage" %>
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
		<form id="Form1" method="post" runat="server">
			<p><asp:LinkButton ID="one" Runat="server">one</asp:LinkButton></p>
			<p>Submitted:
				<asp:Label ID="submitted" Runat="server"></asp:Label></p>
			<input type="hidden" id="tag_one" name="tag_one" value="tag_one_value" />
			<div>
				<input type="hidden" id="nested_tag" name="nested_tag" value="nested_tag_value" />
			</div>
		</form>
		<form id="Form2" method="post">
			<p><input id="two" type="submit" name="two" value="two" /></p>
			<p><input type="hidden" id="tag_two" name="tag_two" value="tag_two_value" /></p>
		</form>
		<form id="RedirectForm" method="get" action="RedirectionTarget.aspx">
		</form>
	</body>
</HTML>
