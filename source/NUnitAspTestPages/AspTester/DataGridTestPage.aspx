<%@ Page language="c#" Codebehind="DataGridTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnit.Extensions.Asp.Test.AspTester.DataGridTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
    <title>AspDataGridTestPage</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
    <meta name="CODE_LANGUAGE" Content="C#" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
  </HEAD>
  <body>
    <form id="AspDataGridTestPage" method="post" runat="server">
      <asp:datagrid id="dataGrid1" runat="server">
        <Columns>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:LinkButton id="link1" onclick="link1_Clicked" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "RowNumber")%>' Runat="server">Link</asp:LinkButton>
            </ItemTemplate>
          </asp:TemplateColumn>
        </Columns>
      </asp:datagrid>
      <asp:datagrid id="dataGrid2" runat="server">
        <Columns>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:LinkButton id="link2" onclick="link2_Clicked" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "RowNumber")%>' Runat="server">Link</asp:LinkButton>
            </ItemTemplate>
          </asp:TemplateColumn>
        </Columns>
      </asp:datagrid>
      <p>The last row clicked was: [<asp:Label ID="clickResult" Runat="server"></asp:Label>]</p>
    </form>
  </body>
</HTML>
