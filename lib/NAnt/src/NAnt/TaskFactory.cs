// NAnt - A .NET build tool
// Copyright (C) 2001-2002 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Gerry Shaw (gerry_shaw@yahoo.com)
// Scott Hernandez (ScottHernandez@hotmail.com)

using System;
using System.IO;
using System.Xml;
using System.Reflection;

namespace SourceForge.NAnt {
    /// <summary>
    /// The TaskFactory comprises all of the loaded, and available, tasks. Use these static methods to register, initialize and create a task.
    /// </summary>
    internal class TaskFactory {

        static TaskBuilderCollection _builders = new TaskBuilderCollection();

        static bool _inited = false;

        /// <summary> Initializes the tasks in the executing assembly, and basedir of the current domain.</summary>
        public static void Initialize() {
            if(!_inited) {
                // initialize builtin tasks
                AddTasks(Assembly.GetExecutingAssembly());
                AddTasks(Assembly.GetCallingAssembly());
                ScanDir(System.AppDomain.CurrentDomain.BaseDirectory);
                //ScanDir(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "tasks"));
                _inited = true;
            }
        }

        /// <summary>Scans the path for any Tasks assemblies and adds them.</summary>
        /// <param name="path">The directory to scan in.</param>
        protected static void ScanDir(string path) {
            // Don't do anything if we don't have a valid directory path
            if(path == null || path == string.Empty) {
                return;
            }

            // intialize tasks found in assemblies that end in Tasks.dll
            DirectoryScanner scanner = new DirectoryScanner();
            scanner.BaseDirectory = path;
            //Log.WriteLine("Scanning Assemblies(*Tests, *Test, *Tasks):" + path);
            scanner.Includes.Add("*Tasks.dll");
            
            //needed for testing
            scanner.Includes.Add("*Tests.dll");
            scanner.Includes.Add("*Test.dll");

            foreach(string assemblyFile in scanner.FileNames) {
                //Log.WriteLine("Add Tasks from:" + assemblyFile);
                AddTasks(Assembly.LoadFrom(assemblyFile));
            }
		
        }
        /// <summary> Adds any Task Assemblies in the Project.BaseDirectory.</summary>
        /// <param name="project">The project to work from.</param>
        public static void AddProject(Project project) {
            //ScanDir(project.BaseDirectory);
            //ScanDir(Path.Combine(project.BaseDirectory, "tasks"));
        }

        /// <summary>Returns the list of loaded TaskBuilders</summary>
        public static TaskBuilderCollection Builders {
            get { return _builders; }
        }
        /// <summary> Scans the given assembly for any classes derived from Task and adds a new builder for them.</summary>
        /// <param name="assembly">The Assembly containing the new tasks to be loaded.</param>
        /// <returns>The count of tasks found in the assembly.</returns>
        public static int AddTasks(Assembly assembly) {
            int taskCount = 0;
            foreach(Type type in assembly.GetTypes()) {
                if (type.IsSubclassOf(typeof(Task)) && !type.IsAbstract) {
                    if (_builders.Add(new TaskBuilder(type.FullName, assembly.Location))) {
                        taskCount++;
                    }
                }
            }
            return taskCount;
        }
        /// <summary> Creates a new Task instance for the given xml and project.</summary>
        /// <param name="taskNode">The XML to initialize the task with.</param>
        /// <param name="proj">The Project that the Task belongs to.</param>
        /// <returns>The Task instance.</returns>
        public static Task CreateTask(XmlNode taskNode, Project proj) {
            string taskName = taskNode.Name;

            TaskBuilder builder = _builders.FindBuilderForTask(taskName);
            if (builder == null && proj != null) {
                Location location = proj.LocationMap.GetLocation(taskNode);
                throw new BuildException(String.Format("Unknown task <{0}>", taskName), location);
            }

            Task task = builder.CreateTask();
            task.Project = proj;
            return task;
        }
    }
}
