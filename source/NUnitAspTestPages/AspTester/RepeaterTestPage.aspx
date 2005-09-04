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
			<p>Repeater with nested repeater:</p>
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
			</asp:repeater>
			
			<p>Simple repeater:</p>			
			<table>
				<asp:repeater id="Repeater2" runat="server">
					<ItemTemplate>
						<tr id="row" runat="server">
							<td>
								<asp:Label ID="Label1" Runat="server">Go Suns!</asp:Label></td>
						</tr>
					</ItemTemplate>
				</asp:repeater>
			</table>

			<p>Repeater with nested button and data binding:</p>
			<asp:Repeater ID="Repeater3" Runat="server">
				<ItemTemplate>
					<asp:Button ID="btnInner" Runat="server" ThingName='<%# DataBinder.Eval(Container.DataItem, "Item") %>' OnClick="btnInner_Click" Text="Click me" >
					</asp:Button><br>
				</ItemTemplate>
			</asp:Repeater>

			<p>Repeater with separator template:</p>
			<asp:Repeater ID="separatorRepeater" Runat="server">
				<ItemTemplate>
					<asp:Label id="separatorLabel" Runat="server"><%# DataBinder.Eval(Container.DataItem, "Item") %></asp:Label>
				</ItemTemplate>
				<SeparatorTemplate>
					<asp:Label ID="nestedSeparatorLabel" Runat="server"> | </asp:Label>
				</SeparatorTemplate>
			</asp:Repeater>
			
			<p>Repeater with header template:</p>
			<asp:Repeater ID="headerRepeater" Runat="server">
				<HeaderTemplate>
					<asp:Label ID="nestedHeaderLabel" runat="server">header</asp:Label>
				</HeaderTemplate>
				<ItemTemplate>
					<asp:Label id="headerLabel" Runat="server"><%# DataBinder.Eval(Container.DataItem, "Item") %></asp:Label>
				</ItemTemplate>
			</asp:Repeater>

			<p>Repeater with footer template:</p>
			<asp:Repeater ID="footerRepeater" Runat="server">
				<ItemTemplate>
					<asp:Label id="footerLabel" Runat="server"><%# DataBinder.Eval(Container.DataItem, "Item") %></asp:Label>
				</ItemTemplate>
				<FooterTemplate>
					<asp:Label ID="nestedFooterLabel" Runat="server">footer</asp:Label>
				</FooterTemplate>				
			</asp:Repeater>
	
			<p>Repeater with alternating item template:</p>
			<asp:Repeater id="alternatingRepeater" Runat="server">
				<ItemTemplate>
					<asp:Label ID="alternatingLabel" Runat="server"><%# DataBinder.Eval(Container.DataItem, "Item") %></asp:Label>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<span style="font-variant: small-caps"><asp:Label ID="alternatingLabel" runat="server"><%# DataBinder.Eval(Container.DataItem, "Item") %></asp:Label></span>
				</AlternatingItemTemplate>
			</asp:Repeater>
	
			<p>Repeater with all templates:</p>
			<asp:Repeater ID="allRepeater" Runat="server">
				<HeaderTemplate>
					<asp:Label id="allHeaderTemplateLabel" runat="server">header</asp:Label>
				</HeaderTemplate>
				<ItemTemplate>
					<asp:Label id="allLabel" Runat="server"><%# DataBinder.Eval(Container.DataItem, "Item") %></asp:Label>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<span style="font-variant: small-caps"><asp:Label ID="allLabel" runat="server"><%# DataBinder.Eval(Container.DataItem, "Item") %></asp:Label></span>
				</AlternatingItemTemplate>
				<SeparatorTemplate>
					<asp:Label ID="allSeparatorTemplateLabel" Runat="server"> | </asp:Label>
				</SeparatorTemplate>
				<FooterTemplate>
					<asp:Label ID="allFooterTemplateLabel" Runat="server">footer</asp:Label>
				</FooterTemplate>				
			</asp:Repeater>
					
			<p>
				<asp:Button id="Button1" runat="server" Text="Button"></asp:Button>
				<asp:Label id="Label1" runat="server">Label</asp:Label>
			</p>
		</form>
	</body>
</HTML>
