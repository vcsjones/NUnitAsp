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
// Scott Hernandez (ScottHernandez@hotmail.com)

using System;
using System.IO;

using NUnit.Framework;
using SourceForge.NAnt;

namespace SourceForge.NAnt.Tests {

    public class ProjectTest : BuildTestBase {

        string _format = @"<?xml version='1.0'?>
            <project name='ProjectTest' default='test' basedir='{0}'>
                {1}
                <target name='test'>
                    {2}
                </target>
            </project>";

        string _buildFileName;

        public ProjectTest(String name) : base(name) {
        }

        protected override void SetUp() {
            base.SetUp();
            _buildFileName = Path.Combine(TempDirName, "test.build");
        }

        public void Test_Initialization() {
            // create the build file in the temp folder
			string buildFileName = Path.Combine(TempDirName, "test.build");
            TempFile.CreateWithContents(FormatBuildFile("", ""), buildFileName);

            Project p = new Project(buildFileName);

            AssertNotNull("Property not defined.", p.Properties["nant.version"]);

            AssertEquals("ProjectTest", p.Properties["nant.project.name"]);
            AssertEquals(new Uri(buildFileName), p.Properties["nant.project.buildfile"]);
            AssertEquals(TempDirName,   p.Properties["nant.project.basedir"]);
            AssertEquals("test",        p.Properties["nant.project.default"]);

            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.call"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.copy"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.delete"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.echo"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.exec"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.fail"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.include"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.mkdir"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.move"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.nant"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.property"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.script"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.sleep"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.style"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.sysinfo"]);
            //AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.taskdef"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.touch"]);
            AssertEquals(Boolean.TrueString, p.Properties["nant.tasks.tstamp"]);

            AssertEquals("The value is " + Boolean.TrueString + ".", p.ExpandProperties("The value is ${nant.tasks.fail}."));
        }

        private string FormatBuildFile(string globalTasks, string targetTasks) {
            return String.Format(_format, TempDirName, globalTasks, targetTasks);
        }
    }
}
