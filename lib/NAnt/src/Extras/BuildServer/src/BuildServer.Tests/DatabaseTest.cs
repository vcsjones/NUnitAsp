using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

using NUnit.Framework;
using NAnt.BuildServer;

namespace NAnt.BuildServer.Tests {

    public class DatabaseTest : TestCase {

        public DatabaseTest(String name) : base(name) {
        }

        Database _db;

        protected override void SetUp() {
            // TODO: get this string from ../../web/Web.config
            _db = new Database(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\work\nant\src\extras\BuildServer\db\BuildServer.mdb");
            _db.Open();
        }

        protected override void TearDown() {
            _db.Close();
        }

        public void Test_SelectProjects() {
            DataSet ds = _db.SelectProjects();
        }

        public void Test_SelectProjectDetails() {
            DataSet ds = _db.SelectProjectDetails(1);
        }

        public void Test_SelectBuildHistory() {
            DataSet ds = _db.SelectBuildHistory(1);
        }

        public void Test_SelectBuildDetails() {
            DataSet ds = _db.SelectBuildDetails(7);
        }

        public void Test_SelectRecentBuilds() {
            DataSet ds = _db.SelectRecentBuilds(5);
        }
    }
}
