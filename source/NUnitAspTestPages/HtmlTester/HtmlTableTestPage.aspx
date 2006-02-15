<%@ Page language="c#" Codebehind="HtmlTableTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.HtmlTester.HtmlTableTestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>HtmlTableTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body MS_POSITIONING="GridLayout">
		<form id="HtmlTableTestPage" method="post" runat="server">
			<table id="table">
				<tr>
					<td>UL</td>
					<td>UR</td>
				</tr>
				<tr>
					<td>ML</td>
					<td>MR</td>
				</tr>
				<tr>
					<td>BL</td>
					<td>BR</td>
				</tr>
				<tr>
				</tr>
				<tr><td><b>Has markup</b></td></tr>
			</table>
			
			<table id="emptyTable">
			</table>
			
			<table id="outerNestedTable">
				<tr>
					<td>
						<table id="innerNestedTable">
							<tr><td>Inner Left</td><td>Inner Right</td></tr>
						</table>
					</td>
					<td>Outer</td>
				</tr>
			</table>
			
			<table id="malformed">
				<tr>Malformed</tr>
			</table>
			
			<table id="headerTags">
				<tr><th>header 1</th><td>data 1</td><th>header 2</th><td>data 2</td></tr>
			</table>
		</form>
	</body>
</html>
