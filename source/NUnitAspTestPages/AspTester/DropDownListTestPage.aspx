<%@ Page language="c#" Codebehind="DropDownListTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.DropDownListTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/web/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>DropDownListTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body>
		<form id="DropDownListTestPage" method="post" runat="server">
			<p>
				Test List:
				<asp:DropDownList id="list" runat="server"></asp:DropDownList>
				<br /><asp:CheckBox ID="auto" Runat="server" Checked="False" Text="Auto Post-Back"></asp:CheckBox>
				<br />Selected Index Changed: <asp:Label ID="indexChanged" Runat="server"></asp:Label>
				<br /><asp:Button ID="add" Runat="server" Text="Add Item to List" OnClick="add_Click"></asp:Button>
				<br /><asp:LinkButton ID="clearSelection" onclick="clearSelection_Click" Runat="server">Clear Selection</asp:LinkButton>
			</p>
			
			<hr />
			<p>Empty List: <asp:DropDownList ID="emptyList" Runat="server"></asp:DropDownList></p>
			
			<hr />
			<p>
				<asp:LinkButton ID="submit" Runat="server">Submit</asp:LinkButton>
			</p>
		</form>
	</body>
</HTML>
