<%@ Page Src="Default.aspx.cs" Inherits="NAnt.BuildServer.Web.DefaultPage" %>

<html>

<head>
	<link rel="stylesheet" type="text/css" href="style.css" />
	<title>NAnt Build Server - Continous Integration with NAnt</title>
</head>

<body>
	<form runat="server">

	<table width="100%" border="0" cellspacing="0" cellpadding="2" class="NavBar">
		<tr><td class="NavBar-Cell" width="100%"><b>NAnt Build Server</b> : Continous Integration with NAnt</td></tr>
	</table>

	<table align="right"/><tr><td><a href="http://nant.sourceforge.net"><img alt="NAnt logo (link to NAnt home page)" border="0" src="images/logo.gif"/></a></td></tr></table>

	<h1><span class="N">N</span><span class="Ant">Ant</span> Build Server</h1>

	<p>February 12, 2002<br />Gerry Shaw -- <a title="Send email to Gerry Shaw" href="mailto:gerry_shaw@yahoo.com">gerry_shaw@yahoo.com</a></p>
	<p>NAnt Build Server is designed to make <a title="Essay by Martin Fowler" href="http://www.martinfowler.com/articles/continuousIntegration.html">continuous integration</a> a reality in a .NET world.</p>

	<h2>Projects</h2>
	<asp:DataGrid id="ProjectList" runat="server"
		cellSpacing="0" cellPadding="4" border="1"
		BorderStyle="Solid"
		BorderColor="#cccccc"
		AutoGenerateColumns="false">

		<HeaderStyle
			HorizontalAlign="Center"
			BackColor="#eeeeee">
			<Font 
				Bold="True"/>
		</HeaderStyle>

		<columns>
            <asp:HyperLinkColumn
				HeaderText="Project"
				DataNavigateUrlField="ProjectId"
				DataNavigateUrlFormatString="project.aspx?id={0}"
				DataTextField="Name"/>

            <asp:TemplateColumn
				HeaderText="Last Built">
				<ItemTemplate>
					<asp:Label runat="server"
						Text="[TODO: Date with link to last build]"/>
				</ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn
				HeaderText="Status">
				<ItemTemplate>
					<asp:Label runat="server"
						Text="Successful"/>
				</ItemTemplate>
            </asp:TemplateColumn>
		</columns>
	</asp:DataGrid>

	<p><img alt=">" src="images/bullet.gif"> <a href="buildqueue.aspx">Build Queue</a> <%# BuildQueueStatus %></p>

	<p><img alt=">" src="images/bullet.gif"> <a href="buildserver.asmx">Web Service</a> for build server.</p>

	</form>
</body>

</html>
