using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using NAnt.BuildServer;

namespace NAnt.BuildServer.Web {

	public class ProjectPage : System.Web.UI.Page {

        public string ProjectName;
        public string ProjectComment;
        public string ProjectBuildFile;
        public DataGrid BuildList;
        public Button BuildButton;

        public void Page_Load(Object sender, EventArgs e) {
            using (Database db = new Database()) {
                db.Open();

                int projectId = Int32.Parse(Request.QueryString["id"]);

                DataSet projectDetails = db.SelectProjectDetails(projectId);
                ProjectName = projectDetails.Tables[0].Rows[0]["Name"].ToString();
                ProjectComment = projectDetails.Tables[0].Rows[0]["Comment"].ToString();

                ProjectBuildFile = projectDetails.Tables[0].Rows[0]["BuildFile"].ToString();
                ProjectBuildFile = ProjectBuildFile.Replace("<", "&lt;");
                ProjectBuildFile = ProjectBuildFile.Replace(">", "&gt;");

                DataSet buildList = db.SelectBuildHistory(projectId);
                BuildList.DataSource = buildList.Tables[0].DefaultView;
                BuildList.DataBind();

                db.Close();
            }

            Page.DataBind();
        }

        public void BuildButton_Click(Object sender, EventArgs e) {
            int projectId = Int32.Parse(Request.QueryString["id"]);

            BuildQueue q = BuildQueue.GetBuildQueue();
            q.AddBuild(new Build(projectId, "Build request from web site."));

            // TODO: post a fake query string to force page never to be cached
            Response.Redirect("buildqueue.aspx");
        }
    }
}
