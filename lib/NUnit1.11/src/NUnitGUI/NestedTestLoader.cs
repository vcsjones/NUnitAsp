using System;
using System.IO;
using NUnit.Framework;
using NUnit.Runner;

namespace NUnit.GUI
{
	/// <summary>
	/// Loader that loads nested tests from a test instance by name.
	/// Uses the the platform directory separator to distinguish nested names.
	/// 
	/// eg: "NamespaceFoo.TestClassBar.\NestedTestName\NestTestName".
	/// 
	/// nested names must start with ".\". and "\" is the
	/// System.IO.Path.DirectorySeparatorChar.
	/// </summary>
	internal sealed class NestedTestLoader : ITestLoader
	{
		#region Instance Variables
		private ITestLoader fLoader;
		#endregion
		private static string fNestedSeparator = 
			System.IO.Path.DirectorySeparatorChar.ToString();


		#region Constructors
		/// <summary>
		/// Creates a new NestedTestLoader using a newly constructed StandardLoader
		/// </summary>
		internal NestedTestLoader ()
			: this(new StandardLoader()){}

		/// <summary>
		/// Creates a new NestedTestLoader that delegates root class loading to the 
		/// supplied loader.
		/// </summary>
		/// <param name="loader">The loader to delegate npon nested loading to</param>
		internal NestedTestLoader (ITestLoader loader)
		{
			if(loader != null)
				fLoader = loader;
			else
				throw new ArgumentNullException("loader");
		}
		#endregion

		#region ITestLoader Methods
		/// <summary>
		/// Implements ILoader.GetLoadName
		/// </summary>
		/// <param name="test"></param>
		/// <returns></returns>
		public string GetLoadName(ITest test)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Implements ILoader.LoadTest()
		/// Loads the nested test using the following form for test naming:
		/// "NamespaceFoo.TestClassBar\NestedTestName\NestTestName".
		/// the special nested name "\." or "\" are a self reference. Where "\" is the
		/// System.IO.Path.DirectorySeparatorChar
		/// </summary>
		/// <param name="loadableName">The qualified name of the nested test</param>
		/// <returns></returns>
		public ITest LoadTest(string loadableName)
		{
			if(loadableName!=null)
			{
				int endPos = loadableName.IndexOf(',');
				int startPos=loadableName.IndexOf(fNestedSeparator,0,endPos);
				if(startPos<0)
					startPos=endPos;
				string nestedName = loadableName.Substring(
					startPos, endPos-startPos);
				loadableName = loadableName.Substring(0,startPos)
					+ loadableName.Substring(endPos);
				return LoadTest(loadableName,nestedName);
			}
			else
				throw new ArgumentNullException("loadableName");
		}
		#endregion

		/// <summary>
		/// Loads the nested test using distinct parameters for the root test 
		/// name and the nested test name. Standard rules apply for the root 
		/// test name.
		/// </summary>
		/// <param name="loadableName"></param>
		/// <param name="nestedName"></param>
		/// <returns></returns>
		public ITest LoadTest(string loadableName, string nestedName)
		{
			if(loadableName!=null)
			{
				if(nestedName==null)
					throw new ArgumentNullException("nestedName");

				if(nestedName.StartsWith(fNestedSeparator))
				{
					nestedName=nestedName.Substring(1);
				}
				if(nestedName.EndsWith(fNestedSeparator))
				{
					nestedName=nestedName.Substring(0,nestedName.Length-1);
				}
				if(nestedName.Equals(string.Empty))
				{
					nestedName = ".";
				}
				ITest ParentTest=fLoader.LoadTest(loadableName);
				string[] subTestNames = nestedName.Split(
					Path.DirectorySeparatorChar);
				foreach(string subTestName in subTestNames)
				{
					if(!subTestName.Equals("."))
					{
						TestSuite suite = ParentTest as TestSuite;
						if(suite!=null)
						{
							ParentTest=null;
							foreach(ITest SubTest in suite.Tests)
							{
								if(subTestName.Equals(SubTest.ToString()))
								{
									ParentTest = SubTest;
								}
							}
							if(ParentTest==null)
								break;
						}
					}
				}
				return ParentTest;
			}
			else
				throw new ArgumentNullException("loadableName");
		}
	}
}
