using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using NAnt.BuildServer;

namespace NAnt.BuildServer.Web {

	public class BuildPage : System.Web.UI.Page {

        public string BuildId;
        public string PageTitle;
        public string ProjectId;
        public string Reason;
        public string ProjectName;
        public string Completed;
        public string Successful;
        public string BuildLog;

        public void Page_Load(Object sender, EventArgs e) {
            using (Database db = new Database()) {
                db.Open();

                int buildId = Int32.Parse(Request.QueryString["id"]);
                DataSet buildDetails = db.SelectBuildDetails(buildId);
                DataRow row = buildDetails.Tables[0].Rows[0];

                ProjectName = row["ProjectName"].ToString();
                ProjectId   = row["ProjectId"].ToString();
                Reason      = row["Reason"].ToString();
                BuildLog    = row["Log"].ToString();
                Successful  = row["Successful"].ToString();
                Completed   = row["DateCompleted"].ToString();
                BuildId     = buildId.ToString();

                db.Close();
            }

            Page.DataBind();
        }
    }
}
