<%@ Page language="c#" Codebehind="AnchorTestPage.aspx.cs" AutoEventWireup="false" Inherits="NUnit.Extensions.Asp.Test.HtmlTester.AnchorTestPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/web/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>AnchorTestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body>
		<form id="AnchorTestPage" method="post" runat="server">
			<!-- NOTE: href attribute of this anchor is capitalized to test case insensitivity of HTML parser -->
			<a id="testLink" Href="../RedirectionTarget.aspx?a=a&amp;b=b">Click Here</a>
			<br />
			<a id="popupLink" onclick="javascript: x=(screen.availWidth/2)-250; y=(screen.availHeight/2)-200;
			some_Win = window.open('../RedirectionTarget.aspx','some_Popup',
			'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=500,height=400,top='+y+',left='+x);" href="#">
				Popup Link</a>
			<br />
			<!-- NOTE: id attribute of this anchor is capitalized to test case insensitivity of HTML parser -->
			<a ID="disabledLink" disabled="disabled" href="../RedirectionTarget.aspx?a=a&amp;b=b">
				Disabled Link</a>
		</form>
	</body>
</HTML>
