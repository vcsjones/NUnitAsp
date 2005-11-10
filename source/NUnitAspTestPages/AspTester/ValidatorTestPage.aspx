<%@ Page language="c#" Codebehind="ValidatorTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.AspTester.ValidatorTestPage" %>
<!DOCTYPE html public "-//w3c//dtd html 4.0 transitional//en" >
<HTML>
	<body>
		<form id="RequiredFieldValidatorTestPage" method="post" runat="server">
			<p><strong>Required Field Validator</strong></p>
			<table>
				<tr>
					<td>Name:</td>
					<td><asp:textbox id="txtName" runat="server" /></td>
					<td><asp:requiredfieldvalidator id="rfvName" controltovalidate="txtName" enableclientscript="False" display="Dynamic" errormessage="Name is required" runat="server" /></td>
				</tr>
			</table>
			<p><strong>Compare Validator</strong></p>
			<table>
				<tr>
					<td>Password:</td>
					<td><asp:textbox id="txtPassword" textmode="Password" runat="server" /></td>
					<td><asp:comparevalidator id="cvPassword" controltocompare="txtPassword" controltovalidate="txtPassword2" enableclientscript="False" display="Dynamic" errormessage="Passwords do not match" runat="server" /></td>
				</tr>
				<tr>
					<td>Confirm Password:</td>
					<td><asp:textbox id="txtPassword2" textmode="Password" runat="server" /></td>
					<td></td>
				</tr>
			</table>
			<p><strong>Range Validator</strong></p>
			<table>
				<tr>
					<td>Age:</td>
					<td><asp:textbox id="txtAge" size="5" runat="server" /></td>
					<td>(must be 18 or higher)&nbsp;<asp:rangevalidator id="rvAge" controltovalidate="txtAge" type="Integer" minimumvalue="18" maximumvalue="100" enableclientscript="False" display="Dynamic" errormessage="Age must be 18 or higher" runat="server" /></td>
				</tr>
			</table>
			<p><strong>Regular Expression Validator</strong></p>
			<table>
				<tr>
					<td>Phone:</td>
					<td><asp:textbox id="txtPhone" runat="server" /></td>
					<td>(###) ###-####&nbsp;<asp:regularexpressionvalidator id="revPhone" controltovalidate="txtPhone" enableclientscript="False" display="Dynamic" errormessage="Phone must be in (###) ###-#### format" validationexpression="\((?<AreaCode>\d{3})\)\s*(?<Number>\d{3}(?:-|\s*)\d{4})(?x)" runat="server" /></td>
				</tr>
			</table>
			<p><strong>Custom Validator</strong></p>
			<table>
				<tr>
					<td>Enter a multiple of 5:</td>
					<td><asp:textbox id="txtNum" size="5" runat="server" /></td>
					<td><asp:customvalidator id="cuvValidateNum" controltovalidate="txtNum" display="Dynamic" errormessage="Value must be a multiple of 5" runat="server" /></td>
				</tr>
			</table>
			<p><strong>Static Client-Side Validator</strong></p>
			<table>
				<tr>
					<td>Mother's Maiden Name:</td>
					<td><asp:TextBox ID="staticClientSideTextBox" Runat="server" /></td>
					<td><asp:RequiredFieldValidator ID="staticClientSideValidator" display="Static" ControlToValidate="staticClientSideTextBox" EnableClientScript="True" ErrorMessage="required" runat="server"></asp:RequiredFieldValidator></td>
				</tr>
			</table>
			<p><strong>Dynamic Client-Side Validator</strong></p>
			<table>
				<tr>
					<td>Mother's Maiden Name:</td>
					<td><asp:TextBox ID="dynamicClientSideTextBox" Runat="server" /></td>
					<td><asp:RequiredFieldValidator ID="dynamicClientSideValidator" display="Dynamic" ControlToValidate="dynamicClientSideTextBox" EnableClientScript="True" ErrorMessage="required" runat="server"></asp:RequiredFieldValidator></td>
				</tr>
			</table>
			<p><strong>Display:None Client-Side Validator</strong></p>
			<table>
				<tr>
					<td>Mother's Maiden Name:</td>
					<td><asp:TextBox ID="noneClientSideTextBox" Runat="server" /></td>
					<td><asp:RequiredFieldValidator ID="noneClientSideValidator" display="None" ControlToValidate="noneClientSideTextBox" EnableClientScript="True" ErrorMessage="required" runat="server"></asp:RequiredFieldValidator></td>
				</tr>
			</table>
			<p><asp:button id="btnSubmit" text="Submit" runat="server" /></p>
		</form>
	</body>
</HTML>
