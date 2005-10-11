<%@ Page language="c#" Codebehind="HtmlSelectTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlSelectTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>HtmlSelectTestPage</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="HtmlSelectTestPage" method="post" runat="server">
			<select id="default">
			</select>
			<br>
			<select id="nonDefault" multiple size="3" runat="server">
				<option>one
				</option>
				<option>two
				</option>
				<option selected>three
				</option>
				<option>four
				</option>
				<option>five
				</option>
			</select>
			<br>
			<select id="singleSelect" runat="server">
				<option>one
				</option>
				<option>two
				</option>
				<option selected>three
				</option>
				<option>four
				</option>
				<option>five
				</option>
			</select>
			<p><asp:LinkButton id="submit" Runat="server">Submit</asp:LinkButton></p>
		</form>
	</body>
</HTML>
