using System;
using System.IO;
using System.Text.RegularExpressions;

using NUnit.Framework;
using NAnt.BuildServer;

// Use this test as a template to create new tests.

namespace NAnt.BuildServer.Tests {

    public class EmptyTest : TestCase {

        public EmptyTest(String name) : base(name) {
        }

        protected override void SetUp() {
        }

        protected override void TearDown() {
        }

        public void Test_Nothing() {
            Assert("This should not have failed.", true);
        }
    }
}
