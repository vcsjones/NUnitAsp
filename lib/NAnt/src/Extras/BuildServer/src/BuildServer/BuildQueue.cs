using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

using SourceForge.NAnt;

namespace NAnt.BuildServer {

    public class Build {
        int _projectId;
        string _reason;
        DateTime _dateCreated;
        DateTime _dateCompleted;
        bool _successful;
        StringLogger _log = new StringLogger();

        public Build(int projectId, string reason) {
            _projectId = projectId;
            _reason = reason;
            _dateCreated = DateTime.Now;
            _successful = false;
        }

        public int ProjectId {
            get { return _projectId; }
        }

        public string Reason {
            get { return _reason; }
        }

        public DateTime DateCreated {
            get { return _dateCreated; }
        }

        public string BuildLog {
            get { return _log.ToString(); }
        }

        public override string ToString() {
            return String.Format("<a href='project.aspx?id={0}'>Project {0}</a>: {1} - {2}\n{3}", ProjectId, DateCreated, Reason, BuildLog);
        }

        internal void Run() {
            Log.Listeners.Clear();
            Log.Listeners.Add(_log);
            Log.IndentSize = 12;

            string baseDir = Path.Combine(Path.GetTempPath(), "BuildServer.Project-" + _projectId);
            if (Directory.Exists(baseDir)) {
                Directory.Delete(baseDir, true);
            }
            Directory.CreateDirectory(baseDir);
            string buildFileName = Path.Combine(baseDir, "bootstrap.buildserver");

            using (Database db = new Database()) {
                db.Open();
                DataSet ds = db.SelectProjectDetails(_projectId);
                db.Close();

                string buildFileContents = ds.Tables[0].Rows[0]["BuildFile"].ToString();
                // save build file to basedir
                using (FileStream f = File.OpenWrite(buildFileName)) {
                    StreamWriter s = new StreamWriter(f);
                    s.Write(buildFileContents);
                    s.Close();
                    f.Close();
                }
            }

            try {
                Project project = new SourceForge.NAnt.Project();
                project.BaseDirectory = baseDir;
                project.BuildFileName = buildFileName;
                project.Verbose = true;
                _successful = project.Run();

            } catch (Exception e) {
                Log.WriteLine("BUILD SERVER INTERNAL ERROR: " + e.ToString());
                _successful = false;
            }
            
            _dateCompleted = DateTime.Now;

            using (Database db = new Database()) {
                try {
                    db.Open();
                    db.InsertBuild(_projectId, _reason, _dateCompleted, _successful, _log.ToString());
                    db.Close();
                } catch (Exception e) {
                    // TODO: decide what to do here?
                    // -  log an event in the system log?
                    // -  do nothing
                    // => display error for 1 minute and move on?
                    Log.WriteLine(e.ToString());
                    Thread.Sleep(60*1000);
                }
            }
        }
    }

    public class BuildCollection : ArrayList {
    }

    internal class BuildRunner {
        Build _currentBuild = null;

        public Build CurrentBuild {
            get { return _currentBuild; }
        }

        public void Run() {
            BuildQueue q = BuildQueue.GetBuildQueue();
            BuildCollection pending = q.PendingBuilds;
            while (pending.Count > 0) {
                // get next build
                lock (pending.SyncRoot) {
                    if (pending.Count == 0) {
                        break;
                    }
                    _currentBuild = (Build) pending[0];
                    pending.RemoveAt(0);
                }
                _currentBuild.Run();
            }
            q._buildRunner = null;
        }
    }

    public sealed class BuildQueue {

		private static BuildQueue _singleton;

		public static BuildQueue GetBuildQueue() {
			if (_singleton == null) {
				_singleton = new BuildQueue();
            }
			return _singleton;
		}

        BuildCollection _pending = new BuildCollection();

        internal BuildRunner _buildRunner = null;

		private BuildQueue() {
        }

        public BuildCollection PendingBuilds {
            get { return _pending; }
        }

        public Build CurrentBuild {
            get {
                if (_buildRunner == null) {
                    return null;
                }
                return _buildRunner.CurrentBuild;
            }
        }

        public void AddBuild(Build build) {
            _pending.Add(build);

            if (_buildRunner == null) {
                _buildRunner = new BuildRunner();
                Thread t = new Thread(new ThreadStart(_buildRunner.Run));
                t.Start();
            }
        }
    }
}
/*


    public class BuildManager {

        private static string _lastBuildLog = "";

        public static string CurrentBuildLog {
            get {
                if (_runner == null) {
                    return _lastBuildLog;
                }
                return _runner.BuildLog;
            }
        }

        internal static BuildRunner _runner = null;
        internal static Thread _runnerThread = null;

        internal static void BuildFinished(BuildRunner runner) {
            _lastBuildLog = runner.BuildLog;
            _runnerThread = null;
            _runner = null;
        }

        public static void BuildProject(int projectId) {
            if (_runner == null) {

                // start the build runner thread
                _runner = new BuildRunner(projectId);
                _runnerThread = new Thread(new ThreadStart(_runner.Run));
                _runnerThread.Start();
            } // else add to queue

        }
    }

    internal class BuildRunner {

        int _projectId;
        StringLogger _logListener = new StringLogger();

        public BuildRunner(int projectId) {
            _projectId = projectId;
        }

        public string BuildLog {
            get {
                return _logListener.ToString();
            }
        }

        public void Run() {

            DateTime dateStarted = DateTime.Now;
            bool successful;

            string baseDir = Path.Combine(Path.GetTempPath(), "BuildServer.ProjectId-" + _projectId);
            if (Directory.Exists(baseDir)) {
                Directory.Delete(baseDir, true);
            }
            Directory.CreateDirectory(baseDir);

            string buildFileName = Path.Combine(baseDir, "bootstrap.buildserver");

            Log.Listeners.Clear();
            Log.Listeners.Add(_logListener);
            Log.IndentSize = 12;
            Log.WriteLine(buildFileName);

            using (Database db = new Database()) {
                db.Open();
                DataSet ds = db.SelectProjectDetails(_projectId);
                db.Close();

                string buildFileContents = ds.Tables[0].Rows[0]["BuildFile"].ToString();

                // save build file to basedir
                using (FileStream f = File.OpenWrite(buildFileName)) {
                    StreamWriter s = new StreamWriter(f);
                    s.Write(buildFileContents);
                    s.Close();
                    f.Close();
                }
            }


            try {
                Project project = new SourceForge.NAnt.Project();
                project.BaseDirectory = baseDir;
                project.BuildFileName = buildFileName;
                project.Verbose = true;
                project.FindInParent = false;
                successful = project.Run();

            } catch (Exception e) {
                Log.WriteLine("BUILD SERVER INTERNAL ERROR: " + e.ToString());
                successful = false;
            }
            
            DateTime dateEnded = DateTime.Now;

            using (Database db = new Database()) {
                db.Open();
                db.InsertBuild(_projectId, dateStarted, dateEnded, successful, _logListener.ToString());
                db.Close();
            }


            BuildManager.BuildFinished(this);
        }
    }
}

*/