using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace NAnt.BuildServer {
    /// <summary>Data access layer.</summary>
    public class Database : IDisposable {
        private bool _disposed = false;
        private string _connectionString;
        private OleDbConnection _connection;

        /// <summary>
        /// The name of all the data set's returned from this class.
        /// </summary>
        public static readonly string DefaultDataSetName = "BuildServer";

        private static readonly string JetDateTimeFormat = @"MM/dd/yyyy HH:mm:ss";

        public Database() {
            _connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        }

        public Database(string connectionString) {
            _connectionString = connectionString;
        }

        ~Database() {
            Dispose();
        }

        protected OleDbConnection Connection {
            get { return _connection; }
        }

        /// <summary>
        /// Open the connection.
        /// </summary>
        public void Open() {
            // open connection
            if (_connection == null) {
                _connection = new OleDbConnection(_connectionString);
                _connection.Open();
            }
        }

        /// <summary>
        /// Close the connection.
        /// </summary>
        public void Close() {
            if (_disposed) {
                throw new ObjectDisposedException("Database", "Database has already been closed/disposed.");
            }
            Dispose();
        }

        public void Dispose() {
            if (!_disposed) {
                _connection.Close();
                _connection = null;
            }
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public DataSet SelectProjects() {
            string sql = "SELECT ProjectId, Name FROM Projects";
            return ExecuteQuery(sql, "Projects");
        }

        public DataSet SelectProjectDetails(int projectId) {
            string sql = String.Format("SELECT ProjectId, Name, BuildFile, Comment FROM Projects WHERE ProjectId = {0}", projectId);
            return ExecuteQuery(sql, "ProjectDetails");
        }

        public DataSet SelectBuildHistory(int projectId) {
            string sql = String.Format("SELECT BuildId, DateCompleted, Successful FROM Builds WHERE ProjectId = {0} ORDER BY DateCompleted DESC", projectId);
            return ExecuteQuery(sql, "BuildHistory");
        }

        public DataSet SelectBuildDetails(int buildId) {
            string sql = String.Format("SELECT * FROM BuildDetails WHERE BuildId = {0}", buildId);
            return ExecuteQuery(sql, "BuildDetails");
        }

        public DataSet SelectRecentBuilds(int count) {
            string sql = String.Format("SELECT TOP {0} * FROM BuildDetails ORDER BY DateCompleted DESC", count);
            return ExecuteQuery(sql, "RecentBuilds");
        }

        public int InsertBuild(int projectId, string reason, DateTime dateCompleted, bool successful, string log) {
            string sql = String.Format("INSERT INTO Builds(ProjectId, Reason, DateCompleted, Successful, Log) VALUES('{0}', '{1}', '{2}', {3}, '{4}')",
                projectId, reason,
                dateCompleted.ToString(JetDateTimeFormat),
                successful, log);

            OleDbCommand command = new OleDbCommand(sql, Connection);
            command.ExecuteNonQuery();

            return -1; // TODO: return the build id of the just inserted record
        }

        /// <summary>Run a DataSet returning query.</summary>
        /// <returns>DataSet filled with query result.</returns>
        private DataSet ExecuteQuery(string sql, string tableName) {
            // fill dataset
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = new OleDbCommand(sql, Connection);
            // This line is only needed if we want to change the dataset
            //OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
            DataSet ds = new DataSet(DefaultDataSetName);
            adapter.Fill(ds, tableName);
            return ds;
        }
    }
}
