<%@ Page language="c#" Codebehind="ValidationSummaryTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.ValidationSummaryTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<body>
		<ul>
			<li>
			test one
			<li>
				test two</li>
		</ul>
		<form id="ValidatorTestPage" method="post" runat="server">
			<asp:TextBox ID="textbox1" runat="server" />
			<asp:RequiredFieldValidator ControlToValidate="textbox1" EnableClientScript="False" Text="*" Display="Dynamic" ErrorMessage="First message" runat="server" id="RequiredFieldValidator1" />
			
			<br>
			<asp:TextBox ID="textbox2" runat="server" />
			<asp:RequiredFieldValidator ControlToValidate="textbox2" EnableClientScript="False" Text="*" Display="Dynamic" ErrorMessage="Second message" runat="server" id="RequiredFieldValidator2" />

			<br>
			<asp:TextBox ID="textbox3" runat="server" />
			<asp:RequiredFieldValidator ControlToValidate="textbox3" EnableClientScript="False" Text="*" Display="Dynamic" ErrorMessage="Third message" runat="server" id="RequiredFieldValidator3" />

			<br>
			<asp:ValidationSummary id="bulletedSummary" ShowSummary="True" ShowMessageBox="False" DisplayMode="BulletList" EnableClientScript="False" runat="server" />
			<asp:ValidationSummary id="listSummary" ShowSummary="True" ShowMessageBox="False" DisplayMode="List" EnableClientScript="False" runat="server" />
			<br>
			<asp:Button id="submit" text="Submit" runat="server" />
		</form>
	</body>
</HTML>
