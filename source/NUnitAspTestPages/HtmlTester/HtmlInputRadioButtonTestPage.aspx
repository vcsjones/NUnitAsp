<%@ Page language="c#" Codebehind="HtmlInputRadioButtonTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlInputRadioButtonTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>HtmlInputRadioButtonTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="HtmlInputRadioButtonTestPage" method="post" runat="server">
			<input id="button1A" type="radio" name="group1" value="1A" runat="server">Group 
			1 Button A
			<br>
			<input id="button1B" type="radio" name="group1" value="1B" checked runat="server">Group 
			1 Button B
			
			<p></p>
			
			<input id="button2A" type="radio" name="group2"/>Group 2 Button A (no value)
			
			<p></p>
			
			<input id="button3A" type="radio" name="group3" value="3A" disabled/>Group 3 Button A (disabled)
			
			<p>
			<asp:LinkButton ID="postback" Runat="server">Postback</asp:LinkButton>
			</p>
			
			<asp:DataGrid ID="formVars" Runat="server"></asp:DataGrid>
		</form>
	</body>
</HTML>
