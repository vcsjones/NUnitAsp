<%@ Page language="c#" Codebehind="ValidationSummaryTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.ValidationSummaryTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<HTML>
	<body>
		<form id="ValidatorTestPage" method="post" runat="server">
			<asp:TextBox ID="textbox" runat="server" />
			<asp:RequiredFieldValidator ControlToValidate="textbox" EnableClientScript="False" Text="*" Display="Dynamic" ErrorMessage="Text box must not be empty" runat="server" id="RequiredFieldValidator1" />
			<br />
			<asp:ValidationSummary id="validator" ShowSummary="True" ShowMessageBox="False" EnableClientScript="False" runat="server" />
			<asp:Button id="submit" text="Submit" runat="server" />
		</form>
	</body>
</HTML>
