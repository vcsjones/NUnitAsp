<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FirstPage.aspx.vb" Inherits="VBSamplePages.FirstPage" ClientTarget="DownLevel"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title></title>
	</HEAD>
	<body id="FirstPage">
		<form id="Form1" method="post" runat="server">
			<P>
				Name:
				<asp:TextBox id="Name" runat="server">Bob</asp:TextBox>
				<asp:RequiredFieldValidator id="NameValidator" runat="server" ErrorMessage="A name is required." ControlToValidate="Name"></asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator id="NameValueValidator" runat="server" ErrorMessage="Only Bob can be a cartoon character." ControlToValidate="Name" ValidationExpression="Bob"></asp:RegularExpressionValidator>
			</P>
			<P>
				Occupation:
				<asp:DropDownList id="Occupation" runat="server">
					<asp:ListItem Value="Builder" Selected="True">Builder</asp:ListItem>
					<asp:ListItem Value="Senator">Senator</asp:ListItem>
					<asp:ListItem Value="Sportscaster">Sportscaster</asp:ListItem>
				</asp:DropDownList>
			</P>
			<P>
				<asp:Button id="CartoonName" runat="server" Text="Create Cartoon Name"></asp:Button>
			</P>
			<H1>
				<asp:Label id="BuiltName" runat="server" ForeColor="Red"></asp:Label>
			</H1>
		</form>
	</body>
</HTML>
