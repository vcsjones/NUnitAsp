<%@ Page Src="BuildQueue.aspx.cs" Inherits="NAnt.BuildServer.Web.BuildQueuePage" %>

<html>

<head>
	<meta http-equiv="refresh" content="<%# RefreshInterval %>" />
	<link rel="stylesheet" type="text/css" href="style.css" />
	<title>Build Queue</title>
</head>

<body>
	<form runat="server">

	<table width="100%" border="0" cellspacing="0" cellpadding="2" class="NavBar">
		<tr><td class="NavBar-Cell" width="100%"><a title="Home page" href="default.aspx"><b>NAnt Build Server</b></a> <img src="images/arrow.gif" alt="->"/> Build Queue</td></tr>
	</table>

	<h1>Build Queue</h1>
	<p>This page will refresh in <%# RefreshInterval %> seconds.</p>

	<h2>Current Build</h2>
	<pre><asp:Label id="CurrentBuild" runat="server" Text="No build in progress."/></pre>

	<h2>Pending Builds</h2>
	<pre><asp:Label id="PendingBuilds" runat="server" Text="No pending builds."/></pre>

	<h2>Recent Builds</h2>
	<asp:DataGrid id="RecentBuildList" runat="server"
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
				DataTextField="ProjectName"
				DataTextFormatString="{0}"/>

			<asp:HyperLinkColumn
				HeaderText="Build"
				DataNavigateUrlField="BuildId"
				DataNavigateUrlFormatString="build.aspx?id={0}"
				DataTextField="BuildId"
				DataTextFormatString="Build {0}"/>

			<asp:BoundColumn
				HeaderText="Date"
				DataField="DateCompleted"/>

			<asp:BoundColumn
				HeaderText="Successful"
                DataField="Successful">
				<ItemStyle
					HorizontalAlign="Center"/>
			</asp:BoundColumn>
		</columns>
	</asp:DataGrid>
	</form>
</body>

</html>
