<%@ Page language="c#" Codebehind="GuestBook.aspx.cs" AutoEventWireup="false" Inherits="GuestBook.GuestBook" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/web/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>GuestBook</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body>
		<form id="GuestBook" method="post" runat="server">
			<asp:TextBox ID="name" Runat="server"></asp:TextBox>
			<asp:TextBox ID="comments" Runat="server"></asp:TextBox>
			<asp:Button ID="save" Runat="server" Text="Save"></asp:Button>
			<asp:DataGrid id="book" runat="server"></asp:DataGrid>
		</form>
	</body>
</HTML>
