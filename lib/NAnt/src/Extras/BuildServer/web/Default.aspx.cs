using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using NAnt.BuildServer;

namespace NAnt.BuildServer.Web {

	public class DefaultPage : System.Web.UI.Page {

        public DataGrid ProjectList;
        public string BuildQueueStatus;

        public void Page_Load(Object sender, EventArgs e) {
            using (Database db = new Database()) {
                db.Open();
                ProjectList.DataSource = db.SelectProjects().Tables[0].DefaultView;
                ProjectList.DataBind();
                db.Close();
            }

            BuildQueue q = BuildQueue.GetBuildQueue();
            Build build = q.CurrentBuild;
            if (build != null) {
                BuildQueueStatus = String.Format("has build in progress, {0} jobs waiting.", q.PendingBuilds.Count);
            } else {
                BuildQueueStatus = "is empty.";
            }
            DataBind();
        }
    }
}

/*

        public Button BuildButton;
        public Label BuildOutput;
	<asp:DataGrid id="ProjectList" runat="server"
		AutoGenerateColumns="false">

		<Columns>
			<asp:HyperLinkColumn
				HeaderText="Project"
				DataNavigateUrlField="ProjectId"
				DataNavigateUrlFormatString="project.aspx?id={0}"
				DataTextField="Name"
				DataTextFormatString="{0:c}"
				Target="_new"/>
		</Columns>
	</asp:DataGrid>

			<asp:TemplateColumn>
				<HeaderTemplate><b>Name</b></HeaderTemplate>
				<ItemTemplate>
					<asp:Label
						Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'
						runat="server"/>
				</ItemTemplate>
			</asp:TemplateColumn>
*/