<%@ Page language="c#" Codebehind="ValidationSummaryTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.ValidationSummaryTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<body>
		<ul>
			<li>test one</li>
			<li>test two</li>
		</ul>
		<form id="ValidatorTestPage" method="post" runat="server">
			<asp:TextBox ID="textbox" runat="server" />
			<asp:RequiredFieldValidator ControlToValidate="textbox" EnableClientScript="False" Text="*" Display="Dynamic" ErrorMessage="Text box must not be empty" runat="server" id="RequiredFieldValidator1" />
			<br />
			<asp:ValidationSummary id="validator" ShowSummary="True" ShowMessageBox="False" EnableClientScript="False" runat="server" />
			<asp:Button id="submit" text="Submit" runat="server" />
		</form>
	</body>
</HTML>
