// NAnt - A .NET build tool
// Copyright (C) 2001 Gerry Shaw
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

using NUnit.Framework;
using NUnit.Runner;
using System.Reflection;
using System.Runtime.Remoting;

namespace SourceForge.NAnt.Tasks.NUnit {

    using System;
    using System.Reflection;

    public enum RunnerResult {
        Success,
        Failures,
        Errors,
    }

    public class NUnitTestRunner : BaseTestRunner {
        FormatterCollection _formatters = new FormatterCollection();
        NUnitTest           _nunittest = null;
        ITest               _suite = null;
        TestResultExtra     _result = null;
        RunnerResult        _resultCode = RunnerResult.Success;

        /// <summary>Collection of the registered formatters.</summary>
        public FormatterCollection Formatters { get { return _formatters; } }

        public RunnerResult ResultCode        { get { return _resultCode; } }

        public NUnitTestRunner(NUnitTest test) {
            _nunittest = test;
            string nunitsuite = test.Class + "," + test.Assembly;
            _suite = GetSuite(nunitsuite);
            test.Suite = _suite;
        }

        /// <summary>Returns the test suite from a given class.</summary>
        /// <remarks>
        /// The assemblyQualifiedName parameter needs to be in form:
        /// "full.qualified.class.name,Assembly"
        /// </remarks>
        ITest GetSuite(string assemblyQualifiedName) {
            // Don't worry about catching exceptions in this method.  The
            // NUnitTask will catch them and throw a BuildException back to
            // NAnt with the correct location in the build file. [gs]

            StandardLoader loader = new StandardLoader();
            ITest test = loader.LoadTest(assemblyQualifiedName);
            return test;
        }

        /// <summary>Runs a Suite extracted from a TestCase subclass.</summary>
        public void Run() {

            _result = new TestResultExtra();

            _result.AddListener(this);
            long startTime = System.DateTime.Now.Ticks;

            FireStartTestSuite();

            // Fire start events
            _suite.Run(_result);

            // finished test
            long endTime = System.DateTime.Now.Ticks;
            long runTime = (endTime-startTime) / 10000;

            _result.RunTime = runTime;

            // Handle completion
            FireEndTestSuite();

            if (_result.WasSuccessful == false) {
                if (_result.ErrorCount != 0) {
                    _resultCode = RunnerResult.Errors;
                }
                else if (_result.FailureCount !=0) {
                    _resultCode = RunnerResult.Failures;
                }
            }
        }

        //---------------------------------------------------------
        // BaseTestRunner overrides
        //---------------------------------------------------------

        protected override void RunFailed(string message) {
        }

        //---------------------------------------------------------
        // IListener overrides
        //---------------------------------------------------------
        public override void AddError(ITest test, Exception t) {
            foreach (IResultFormatter formatter in Formatters) {
                formatter.AddError(test, t);
            }

            if (_nunittest.HaltOnError) {
                _result.Stop();
            }
        }

        public override void AddFailure(ITest test, AssertionFailedError t) {
            foreach (IResultFormatter formatter in Formatters) {
                formatter.AddFailure(test, t);
            }

            if (_nunittest.HaltOnFailure) {
                _result.Stop();
            }
        }

        public override void StartTest(ITest test) {
            foreach (IResultFormatter formatter in Formatters) {
                formatter.StartTest(test);
            }
        }

        public override void EndTest(ITest test) {
            foreach (IResultFormatter formatter in Formatters) {
                formatter.EndTest(test);
            }
        }

        //---------------------------------------------------------
        // Formatter notification methods
        //---------------------------------------------------------

        void FireStartTestSuite() {
            foreach (IResultFormatter formatter in Formatters) {
                formatter.StartTestSuite(_nunittest);
            }
        }

        void FireEndTestSuite() {
            foreach (IResultFormatter formatter in Formatters) {
                formatter.EndTestSuite(_result);
            }
        }
    }
}
