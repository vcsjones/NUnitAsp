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
using System.Text;
using System.Text.RegularExpressions;

using NUnit.Framework;
using SourceForge.NAnt;

namespace SourceForge.NAnt.Tests {

    public class NAntTest : TestCase {

        public NAntTest(String name) : base(name) {
        }

        protected override void SetUp() {
        }

        protected override void TearDown() {
        }

        public void Test_GetBuildFileName() {
            try {
                NAnt.GetBuildFileName(null, null, false);
                Fail("Exception not thrown.");
            } catch {
            }

            string baseDirectory = Path.Combine(Path.GetTempPath(), "SourceForge.NAnt.Tests.ProjectTest.Test_GetBuildFileName");
            string build1FileName = Path.Combine(baseDirectory, "file1.build");
            string build2FileName = Path.Combine(baseDirectory, "file2.build");
            Assert(baseDirectory + " exists.", !Directory.Exists(baseDirectory));
            Directory.CreateDirectory(baseDirectory);

            try {
                NAnt.GetBuildFileName(baseDirectory, null, false);
                Fail("ApplicationException not thrown.");
            } catch (ApplicationException) {
            }

            TempFile.Create(build1FileName);
            Assert(build1FileName + " does not exists.", File.Exists(build1FileName));

            AssertEquals(build1FileName, NAnt.GetBuildFileName(Path.GetDirectoryName(build1FileName), null, false));

            // create a second build file in same directory
            TempFile.Create(build2FileName);
            Assert(build2FileName + " does not exists.", File.Exists(build2FileName));
            AssertEquals(Path.GetDirectoryName(build1FileName), Path.GetDirectoryName(build2FileName));

            try {
                NAnt.GetBuildFileName(Path.GetDirectoryName(build1FileName), null, false);
                Fail("ApplicationException not thrown.");
            } catch (ApplicationException) {
            }

            Directory.Delete(baseDirectory, true);
            Assert(baseDirectory + " exists.", !Directory.Exists(baseDirectory));
        }

        public void Test_FindInParentOption() {
			string baseDirectory = TempDir.Create("NAnt.Tests.ProjectTest.Test_Find");
            string buildFileName = Path.Combine(baseDirectory, "file.build");
            string subDirectory = Path.Combine(baseDirectory, "SubDirectory");

            // create a build file
            TempFile.Create(buildFileName);
            Assert(buildFileName + " does not exists.", File.Exists(buildFileName));

            // create a sub directory
            Assert(subDirectory + " exists.", !Directory.Exists(subDirectory));
            Directory.CreateDirectory(subDirectory);
            Assert(subDirectory + " does not exists.", Directory.Exists(subDirectory));

            // find the build file from the sub directory
            AssertEquals(buildFileName, NAnt.GetBuildFileName(subDirectory, null, true));

            // create a second build file
            string secondBuildFileName = Path.Combine(baseDirectory, "file2.build");
            TempFile.Create(secondBuildFileName);
            Assert(secondBuildFileName + " does not exists.", File.Exists(secondBuildFileName));

            // try to find build file in sub directory 
            // expect an exception - multiple *.build files found
            try {
                NAnt.GetBuildFileName(subDirectory, null, true);
                Fail("ApplicationException not thrown.");
            } catch (ApplicationException) {
            }

            // try to find a build file that doesn't exist
            // expect an exception - build file not found
            try {
                NAnt.GetBuildFileName(subDirectory, "foobar.xml", true);
                Fail("ApplicationException not thrown.");
            } catch (ApplicationException) {
            }

            // try to find a build file with a bad pattern
            try {
                // buildFileName has a full path while GetBuildFileName will only accept a filename/pattern or null.
                NAnt.GetBuildFileName(subDirectory, buildFileName, true);
                Fail("Exception not thrown.");
            } catch {
            }

            // try to find specific build file in sub directory (expect success)
            AssertEquals(buildFileName, NAnt.GetBuildFileName(subDirectory, Path.GetFileName(buildFileName), true));

            // delete everything we just created
            TempDir.Delete(baseDirectory);
        }
        public void Test_ShowHelp() {
            string[] args = { "-help" };

            string result = null;
            using (ConsoleCapture c = new ConsoleCapture()) {
                SourceForge.NAnt.NAnt.Main(args);
                result = c.Close();
            }

            // using a regular expression look for a plausible version number and valid copyright date
            string expression = @"^NAnt version (?<major>[0-9]+).(?<minor>[0-9]+).(?<build>[0-9]+) Copyright \(C\) 2001-(?<year>200[0-9]) Gerry Shaw";

            Match match = Regex.Match(result, expression);
            Assert("Help text does not appear to be valid.", match.Success);
            int major = Int32.Parse(match.Groups["major"].Value);
            int minor = Int32.Parse(match.Groups["minor"].Value);
            int build = Int32.Parse(match.Groups["build"].Value);
            int year  = Int32.Parse(match.Groups["year"].Value);
            Assert("Version numbers must be positive.", major >= 0);
            Assert("Version numbers must be positive.", minor >= 0);
            Assert("Version numbers must be positive.", build >= 0);
            AssertEquals(DateTime.Now.Year, year);
        }

        public void Test_BadArgument() {
            string[] args = { "-asdf", "-help", "-verbose" };

            string result = null;
            using (ConsoleCapture c = new ConsoleCapture()) {
                SourceForge.NAnt.NAnt.Main(args);
                result = c.Close();
            }

            // using a regular expression look for a plausible version number and valid copyright date
            string expression = @"Unknown argument '-asdf'";

            Match match = Regex.Match(result, expression);
            Assert("Argument did not cause an error.", match.Success);
        }

        public void Test_DefineProperty() {
            string buildFileContents = @"<?xml version='1.0' ?>
                <project name='Test' default='test' basedir='.'>
                    <target name='test'>
                        <property name='project.name' value='Foo.Bar'/>
                        <echo message='project.name = ${project.name}'/>
                    </target>
                </project>";

            // write build file to temp file
            string buildFileName = TempFile.CreateWithContents(buildFileContents);
            Assert(buildFileName + " does not exists.", File.Exists(buildFileName));

            string[] args = { 
                "-D:project.name=MyCompany.MyProject",
                String.Format("-buildfile:{0}", buildFileName),
            };

            string result = null;
            using (ConsoleCapture c = new ConsoleCapture()) {
                SourceForge.NAnt.NAnt.Main(args);
                result = c.Close();
            }

            // regular expression to look for expected output
            string expression = @"project.name = MyCompany.MyProject";
            Match match = Regex.Match(result, expression);
            Assert("Property 'project.name' appears to have been overridden by <property> task.\n" + result, match.Success);

            // delete the build file
            File.Delete(buildFileName);
            Assert(buildFileName + " exists.", !File.Exists(buildFileName));
        }

        public void Test_ShowProjectHelp() {
            string buildFileContents = @"<?xml version='1.0' ?>
                <project name='Hello World' default='build' basedir='.'>

                    <property name='basename' value='HelloWorld'/>
                    <target name='init'/> <!-- fake subtarget for unit test -->

                    <target name='clean' description='cleans build directory'>
                        <delete file='${basename}.exe' failonerror='false'/>
                    </target>

                    <target name='build' description='compiles the source code'>
                        <csc target='exe' output='${basename}.exe'>
                            <sources>
                                <includes name='${basename}.cs'/>
                            </sources>
                        </csc>
                    </target>

                    <target name='test' depends='build' description='run the program'>
                        <exec program='${basename}.exe'/>
                    </target>
                </project>"; 

            // write build file to temp file
            string buildFileName = TempFile.CreateWithContents(buildFileContents);
            Assert(buildFileName + " does not exists.", File.Exists(buildFileName));

            string[] args = { 
                "-projecthelp", 
                String.Format("-buildfile:{0}", buildFileName),
            };

            string result = null;
            using (ConsoleCapture c = new ConsoleCapture()) {
                SourceForge.NAnt.NAnt.Main(args);
                result = c.Close();
            }

            /* expecting output in the form of"

                Default Target:

                 build         compiles the source code

                Main Targets:

                 clean         cleans build directory
                 build         compiles the source code
                 test          run the program

                Sub Targets:

                 init
            */

            // using a regular expression look for a plausible version number and valid copyright date
            // expression created by RegEx http://www.organicbit.com/regex/
            string expression = @"Default Target:[\s\S]*(?<default>build)\s*compiles the source code[\s\S]*Main Targets:[\s\S]*(?<main1>build)\s*compiles the source code[\s\S]*(?<main2>clean)\s*cleans build directory[\s\S]*(?<main3>test)\s*run the program[\s\S]*Sub Targets:[\s\S]*(?<subtarget1>init)";

            Match match = Regex.Match(result, expression);
            if (match.Success) {
                AssertEquals("build", match.Groups["default"].Value);
                AssertEquals("build", match.Groups["main1"].Value);
                AssertEquals("clean", match.Groups["main2"].Value);
                AssertEquals("test", match.Groups["main3"].Value);
                AssertEquals("init", match.Groups["subtarget1"].Value);
            } else {
                Fail("Project help text does not appear to be valid, see results for details:\n" + result);
            }

            // delete the build file
            File.Delete(buildFileName);
            Assert(buildFileName + " exists.", !File.Exists(buildFileName));
        }
    }
}