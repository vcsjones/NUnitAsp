<%@ Page language="c#" Codebehind="ListBoxTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.ListBoxTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ListBoxTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body>
		<form id="ListBoxTestPage" method="post" runat="server">
			<p>
				Test List:
				<asp:ListBox id="list" runat="server"></asp:ListBox>
				<br /><asp:CheckBox ID="auto" Runat="server" Checked="False" Text="Auto Post-Back"></asp:CheckBox>
				<br /><asp:CheckBox ID="multi" Runat="server" Checked="False" Text="Multi-Select"></asp:CheckBox>
				<br />Selected Index Changed: <asp:Label ID="indexChanged" Runat="server"></asp:Label>
				<br /><asp:Button ID="add" Runat="server" Text="Add Item to List" OnClick="add_Click"></asp:Button>
				<br /><asp:LinkButton ID="clearSelection" onclick="clearSelection_Click" Runat="server">Clear Selection</asp:LinkButton>
			</p>
			
			<hr />
			<p>Empty List: <asp:ListBox ID="emptyList" Runat="server"></asp:ListBox></p>
			
			<hr />
			<p>Disabled List: 
			<asp:ListBox ID="disabledList" Enabled="False" Runat="server">
				<asp:ListItem Text="One" Value="1" Selected="True" />
				<asp:ListItem Text="Two" Value="2" />
			</asp:ListBox></p>
			
			<hr />
			<p>
				<asp:LinkButton ID="submit" Runat="server">Submit</asp:LinkButton>
			</p>
		</form>
	</body>
</HTML>
