using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NAnt.BuildServer.Tests {

	// This class bundles all tests into a single suite.
	public class AllTests {
		public static ITest Suite {
			get  {
				TestSuite suite = new TestSuite("Build Server Tests");

                // Use reflection to automagically scan all the classes that 
                // inherit from TestCase and add them to the suite.
                Assembly assembly = Assembly.GetExecutingAssembly();
                foreach(Type type in assembly.GetTypes()) {
                    if (type.IsSubclassOf(typeof(TestCase)) && !type.IsAbstract) {
                        suite.AddTestSuite(type);
                    }
                }
                return suite;
            }
        }
    }
}
