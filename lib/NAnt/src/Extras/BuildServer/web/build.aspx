<%@ Page Src="Build.aspx.cs" Inherits="NAnt.BuildServer.Web.BuildPage" %>

<html>

<head>
	<link rel="stylesheet" type="text/css" href="style.css" />
	<title><%# ProjectName %> Build <%# BuildId %></title>
</head>

<body>
	<table width="100%" border="0" cellspacing="0" cellpadding="2" class="NavBar">
		<tr><td class="NavBar-Cell" width="100%"><a title="Home page" href="default.aspx"><b>NAnt Build Server</b></a> <img src="images/arrow.gif" alt="->"/> <a href="project.aspx?id=<%# ProjectId %>"><%# ProjectName %></a> <img src="images/arrow.gif" alt="->"/> Build <%# BuildId %></td></tr>
	</table>

	<h1><%# ProjectName %> Build <%# BuildId %></h1>
	<table border="0">
		<tr>
			<td>Reason:</td>
			<td><%# Reason %></td>
		</tr>
		<tr>
			<td>Completed:</td>
			<td><%# Completed %></td>
		</tr>
		<tr>
			<td>Successful:</td>
			<td><%# Successful %></td>
		</tr>
	</table>

	<h2>Build Log</h2>
	<pre><%# BuildLog %></pre>
</body>

</html>
