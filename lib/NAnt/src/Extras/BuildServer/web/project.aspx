<%@ Page Src="Project.aspx.cs" Inherits="NAnt.BuildServer.Web.ProjectPage" %>

<html>

<head>
	<link rel="stylesheet" type="text/css" href="style.css" />
	<title><%# ProjectName %></title>
</head>

<body>
	<form runat="server">

	<table width="100%" border="0" cellspacing="0" cellpadding="2" class="NavBar">
		<tr><td class="NavBar-Cell" width="100%"><a title="Home page" href="default.aspx"><b>NAnt Build Server</b></a> <img src="images/arrow.gif" alt="->"/> <%# ProjectName %></td></tr>
	</table>

	<h1><%# ProjectName %></h1>
	<%# ProjectComment %>
	<p>
		<asp:Button id="BuildButton" runat="server"
			Text="Build Project"
			OnClick="BuildButton_Click"/>
	</p>

	<h2>Build History</h2>
	<asp:DataGrid id="BuildList" runat="server"
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

	<h2>Build File</h2>
	<pre><%# ProjectBuildFile %></pre>

	</form>
</body>

</html>
