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

// Gerry Shaw (gerry_shaw@yahoo.com)

using System;
using System.IO;
using System.Text;
using System.Xml;

using NUnit.Framework;
using SourceForge.NAnt.Tasks;

namespace SourceForge.NAnt.Tests {

    /// <summary>Base class for running build files and checking results.</summary>
    /// <remarks>
    ///   <para>Provides support for quickly running a build and capturing the output.</para>
    /// </remarks>
    public abstract class BuildTestBase : TestCase {

        string _tempDirName = null;
       
        public BuildTestBase(string name) : base(name) {
        }

        public string TempDirName {
            get { return _tempDirName; }
        }

        /// <remarks>
        ///   <para>Super classes that override SetUp must call the base class first.</para>
        /// </remarks>
        protected override void SetUp() {
            _tempDirName = TempDir.Create(this.GetType().FullName);
        }

        /// <remarks>
        ///   <para>Super classes that override must call the base class last.</para>
        /// </remarks>
        protected override void TearDown() {
            TempDir.Delete(_tempDirName);
        }

        public string RunBuild(string xml) {
            Project p = CreateFilebasedProject(xml);
            return ExecuteProject(p);
        }

        public string ExecuteProject(Project p) {
            using (ConsoleCapture c = new ConsoleCapture()) {
                // Most tests won't have a target but it doesn't hurt to call it.
                p.Execute();

                return c.Close();
            }
        }
        
        public Project CreateFilebasedProject(string xml) {
            // create the build file in the temp folder
            string buildFileName = Path.Combine(TempDirName, "test.build");
            TempFile.CreateWithContents(xml, buildFileName);

            return new Project(buildFileName);
        }

        public string CreateTempFile(string name) {
            return CreateTempFile(name, null);
        }

        public string CreateTempFile(string name, string contents) {
            string filename = Path.Combine(TempDirName, name);
            
            if(Path.IsPathRooted(name))
                filename=name;

            if(contents == null)
                return TempFile.Create(filename);
            
            return TempFile.CreateWithContents(contents, filename);
        }
        public string CreateTempDir(string name) {
            return TempDir.Create(Path.Combine(TempDirName, name));
        }
    }
}
