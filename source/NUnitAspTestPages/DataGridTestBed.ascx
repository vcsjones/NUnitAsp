<%@ Control Language="c#" AutoEventWireup="false" Codebehind="DataGridTestBed.ascx.cs" Inherits="NUnitAspTestPages.DataGridTestBed" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:datagrid id="DataGrid1" runat="server">
	<Columns>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:LinkButton id="viewLink" Runat="server">View</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
