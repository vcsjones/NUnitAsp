using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using NAnt.BuildServer;

namespace NAnt.BuildServer.Web {

	public class BuildQueuePage : System.Web.UI.Page {

        public Label PendingBuilds;
        public Label CurrentBuild;
        public DataGrid RecentBuildList;

        // How often clients should refresh this page (in seconds)
        public string RefreshInterval;

        public void Page_Load(Object sender, EventArgs e) {

            RefreshInterval = ConfigurationSettings.AppSettings["BuildQueueRefreshInterval"];

            using (Database db = new Database()) {
                db.Open();

                DataSet ds = db.SelectRecentBuilds(5);
                RecentBuildList.DataSource = ds.Tables[0].DefaultView;
                RecentBuildList.DataBind();

                db.Close();
            }

            BuildQueue q = BuildQueue.GetBuildQueue();

            BuildCollection pending = q.PendingBuilds;
            if (pending.Count > 0) {
                lock (pending.SyncRoot) {
                    StringBuilder s = new StringBuilder();
                    s.Append("<ol>");
                    foreach (Build b in pending) {
                        s.Append("<li>" + b.ToString() + "</li>");
                    }
                    s.Append("</ol>");
                    PendingBuilds.Text = s.ToString();
                }
            }

            Build build = q.CurrentBuild;
            if (build != null) {
                CurrentBuild.Text = build.ToString();
            }

            DataBind();
        }
    }
}
