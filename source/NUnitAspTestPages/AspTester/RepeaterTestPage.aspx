<%@ Page language="c#" Codebehind="RepeaterTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.RepeaterTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Repeater</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="divRepeater1">
				<asp:repeater id="Repeater1" runat="server">
					<ItemTemplate>
						<asp:TextBox ID="txtBlah" Runat="server"></asp:TextBox>
						<span id="spnInnerRepeater1" row="true" runat="server">
							<asp:Repeater ID="innerRepeater1" Runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "InnerList") %>' >
								<ItemTemplate>
									<span id="innerSpan" row="true" runat="server">
										<asp:Label ID="lblBlah" runat="server">HELLO</asp:Label></span>
								</ItemTemplate>
							</asp:Repeater>
						</span>
					</ItemTemplate>
				</asp:repeater></div>
			<table id="tblRepeater2">
				<asp:repeater id="Repeater2" runat="server">
					<ItemTemplate>
						<tr id="row" runat="server">
							<td>
								<asp:Label ID="Label1" Runat="server">Go Suns!</asp:Label></td>
						</tr>
					</ItemTemplate>
				</asp:repeater></table>
			<asp:Repeater ID="Repeater3" Runat="server">
				<ItemTemplate>
					<asp:Button ID="btnInner" Runat="server" ThingName='<%# DataBinder.Eval(Container.DataItem, "Thing") %>' OnClick="btnInner_Click" Text="Click me" >
					</asp:Button><br>
				</ItemTemplate>
			</asp:Repeater>
			<asp:Button id="Button1" runat="server" Text="Button"></asp:Button>
			<asp:Label id="Label1" runat="server">Label</asp:Label>
		</form>
	</body>
</HTML>
