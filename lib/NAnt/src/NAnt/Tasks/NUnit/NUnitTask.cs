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

// Ian MacLean (ian_maclean@another.com)
// Gerry Shaw (gerry_shaw@yahoo.com)

using System;
using System.IO;
using System.Xml;
using SourceForge.NAnt.Attributes;
using NUnit.Framework;
using NUnit.Runner;
using System.Reflection;
using System.Runtime.Remoting;

namespace SourceForge.NAnt.Tasks.NUnit {

    /// <summary>Runs tests using the NUnit framework.</summary>
    /// <remarks>
    ///   <para>See the <a href="http://nunit.sf.net">NUnit home page</a> for more information.</para>
    /// </remarks>
    /// <example>
    ///   <para>Run tests in the <c>MyProject.Tests.dll</c> assembly.</para>
    ///   <para>The test results are logged in <c>results.xml</c> and <c>results.txt</c> using the <c>Xml</c> and <c>Plain</c> formatters, respectively.</para>
    ///   <code>
    /// <![CDATA[
    /// <nunit basedir="build" verbose="false" haltonerror="true" haltonfailure="true">
    ///     <formatter type="Xml"/>
    ///     <formatter type="Plain"/>
    ///     <test name="MyProject.Tests.AllTests" assembly="MyProject.Tests.dll" outfile="results"/>
    /// </nunit>
    /// ]]>
    ///   </code>
    /// </example>
    [TaskName("nunit")]
    public class NUnitTask : Task {

        bool _fork = false;
        bool _haltOnError = false;
        bool _haltOnFailure = false;
        string _AppDomainLoadPath = "";
        int _timeout = 0;

        // If NUnit integrates the change that I posted this should be uncommented out
        // See NUnit discussion board for post by gerry_shaw on October 10, 2001
        // <summary>Break in the debugger whenever a test fails.</summary>
        //[TaskAttribute("breakindebugger")]
        //[BooleanValidator()]
        //string _breakindebugger = Boolean.FalseString;
        //public bool BreakInDebugger     { get { return Convert.ToBoolean(_breakindebugger); } }

        // Attribute properties

         /// <summary>Run the tests in a separate AppDomain.</summary>
        [TaskAttribute("fork")]
        [BooleanValidator()]
        public bool Fork                { get { return _fork; } set { _fork = value; } }

        /// <summary>Stop the build process if an error occurs during the test run.</summary>
        /// <remarks>Implies haltonfailure.</remarks>
        [TaskAttribute("haltonerror")]
        [BooleanValidator()]
        public bool HaltOnError         { get { return _haltOnError; }set { _haltOnError = value; } }

        /// <summary>Stop the build process if a test fails (errors are considered failures as well).</summary>
        [TaskAttribute("haltonfailure")]
        [BooleanValidator()]
        public bool HaltOnFailure       { get { return _haltOnFailure; } set { _haltOnFailure = value; }}

        /// <summary>Cancel the individual tests if they do not finish in the specified time (measured in milliseconds). Ignored if fork is disabled.</summary>
        [TaskAttribute("timeout")]
        public int  Timeout             { get { return _timeout; } set { _timeout = value; } }

        // child element collections
        NUnitTestCollection _tests = new NUnitTestCollection(); // TODO make a type safe collection
        FormatterElementCollection _formatterElements = new FormatterElementCollection();

        bool _failuresPresent = false;
        bool _errorsPresent = false;

        void ExecuteTest(NUnitTest test) {
            // Set Defaults
            RunnerResult result = RunnerResult.Success;

            if (test.ToDir == null) {
                test.ToDir = Project.BaseDirectory;
            }
            if (test.OutFile == null) {
                test.OutFile = "TEST-" + test.Class;
            }
            if (test.Fork == true) {
                result = ExecuteInAppDomain(test);
            } else {
                result = ExecuteInProc(test);
            }

            // Handle return code:
            // If there is an error/failure and that it should halt, stop
            // everything otherwise just log a statement.
            bool errorOccurred   = (result == RunnerResult.Errors);
            bool failureOccurred = (result != RunnerResult.Success);

            if ((errorOccurred && test.HaltOnError) || (failureOccurred && test.HaltOnFailure)) {
                // Only thrown if this test should halt as soon as the first
                // error/failure is detected.  In most cases all tests will
                // be run to get a full list of problems.
                throw new BuildException("Test " + test.Class + " Failed" , Location);

            } else if (errorOccurred || failureOccurred) {
                // let the formatters report the errors
                //Log.WriteLine(LogPrefix + "Test {0} Failed ", test.Class);
            }

            // Used for reporting the final result from the task.
            if (errorOccurred) {
                _errorsPresent = true;
            }
            if (failureOccurred) {
                _failuresPresent = true;
            }
        }

        // TODO implement launching in a seperate App Domain
        RunnerResult ExecuteInAppDomain(NUnitTest test) {
            // set this so that we load out of the correct directory
            _AppDomainLoadPath = this.Project.BaseDirectory + "\build";

            // spawn new domain in specified directory
            AppDomain newDomain =  AppDomain.CreateDomain("NAnt Remote Domain", null, _AppDomainLoadPath, _AppDomainLoadPath, false);

            // instantiate subclassed test runner in new domain
            ObjectHandle oh = newDomain.CreateInstance("NAnt.Core", "SourceForge.NAnt.Tasks.NUnit.NUnitTestRunner");
            NUnitTestRunner runner = (NUnitTestRunner)(oh.Unwrap());
            Log.WriteLine(LogPrefix + "Running {0} ", test.Class);

            // The Log formatter is special in that it always writes to the
            // Log class rather than the TextWriter set in SetOutput().
            LogFormatter logFormatter = new LogFormatter(LogPrefix, Verbose);
            runner.Formatters.Add(logFormatter);

            runner.Run();
            return runner.ResultCode;
        }

        RunnerResult ExecuteInProc(NUnitTest test) {
            try {
                NUnitTestRunner runner = new NUnitTestRunner(test);

                Log.WriteLine(LogPrefix + "Running {0} ", test.Class);

                // The Log formatter is special in that it always writes to the
                // Log class rather than the TextWriter set in SetOutput().
                LogFormatter defaultFormatter = new LogFormatter(LogPrefix, Verbose);
                runner.Formatters.Add(defaultFormatter);

                // Now add the specified formatters
                foreach (FormatterElement element in mergeFormatters(test)) {
                    // determine file
                    FileInfo outFile = getOutput(element, test);
                    IResultFormatter formatter = CreateFormatter(element, outFile);
                    runner.Formatters.Add(formatter);
                }

                runner.Run();
                return runner.ResultCode;

            } catch (Exception e) {
                throw new BuildException("Error running unit test.", Location, e);
            }
        }

        /// <param name="taskNode">Xml node used to initialize this task instance.</param>
        protected override void InitializeTask(XmlNode taskNode) {

            // get all child tests
            foreach (XmlNode testNode in taskNode) {
				if(testNode.Name.Equals("test"))
				{
					NUnitTest test = new NUnitTest();
					test.Project = Project; 
					test.Initialize(testNode);
					_tests.Add(test);
				}
            }

            // now get formatters
            foreach (XmlNode formatterNode in taskNode) {
				if(formatterNode.Name.Equals("formatter"))
				{
					FormatterElement formatter = new FormatterElement();
					formatter.Project = Project;
					formatter.Initialize(formatterNode);
					_formatterElements.Add(formatter);
				}
            }
        }

        protected override void ExecuteTask() {
            // If NUnit integrates the change that I posted this should be uncommented out
            // See NUnit discussion board for post by gerry_shaw on October 10, 2001
            //Assertion.BreakInDebugger = BreakInDebugger;

            foreach (NUnitTest test in _tests) {
                //test.AutoExpandAttributes();
                ExecuteTest(test);
            }

            if (_failuresPresent && HaltOnFailure) {
                throw new BuildException("Unit test failed, see build log.", Location);
            }
            if (_errorsPresent && (HaltOnError || HaltOnFailure)) {
                throw new BuildException("Unit test had errors, see build log.", Location);
            }
        }
        protected IResultFormatter CreateFormatter(FormatterElement element, FileInfo outfile) {
            //Create new element based on ...
            IResultFormatter retFormatter = null;
            switch (element.Type) {
               case FormatterType.Plain:
                    retFormatter = (IResultFormatter) new PlainTextFormatter();
                    break;
                case FormatterType.Xml:
                   retFormatter = (IResultFormatter) new XmlResultFormatter();
                    break;
                case FormatterType.Custom:
                    // Create based on class name
                    break;
                default:
                    //retFormatter = Custom;
                    break;
            }
            TextWriter tw = new StreamWriter( outfile.Create());
            retFormatter.SetOutput(tw);
            return retFormatter;

        }

        /// <summary>Return the file or null if does not use a file.</summary>
        protected FileInfo getOutput(FormatterElement fe, NUnitTest test){
            if (fe.UseFile ) {
                string filename = test.OutFile + fe.Extension;
                FileInfo info = new FileInfo(test.ToDir + Path.DirectorySeparatorChar + filename);

                string absFilename = Project.GetFullPath( test.ToDir) + Path.DirectorySeparatorChar + filename;

                return new FileInfo(absFilename);
            }
            return null;
        }

        protected FormatterElementCollection mergeFormatters(NUnitTest test){
            // TODO flesh this out to merge test level and NUnit task level formatters
            FormatterElementCollection allFormatters = new FormatterElementCollection();
            allFormatters.AddRange(_formatterElements);
            //FormatterElementCollection allFormatters = (FormatterElementCollection)_formatterElements.Clone();
            allFormatters.AddRange(test.FormatterElements);
            return allFormatters;
        }
    }
}
