<%@ Page language="c#" Codebehind="TestBed.aspx.cs" AutoEventWireup="false" Inherits="NUnitAspTestPages.TestBed" %>
<%@ Register TagPrefix="uc1" TagName="DataGridTestBed" Src="DataGridTestBed.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://localhost/NUnitAsp/dtd/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>TestBed</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</HEAD>
	<body id="NUnitAspTestPagesTestBed">
		<form id="TestBed" method="post" runat="server">
			<uc1:DataGridTestBed id="DataGridTestBed1" runat="server"></uc1:DataGridTestBed>
		</form>
	</body>
</HTML>
