namespace NUnit.GUI
{
	using System;
	using System.Collections;
	using System.Reflection;
	using System.Security;
	using System.Windows.Forms;
	using NUnit.Framework;
	using NUnit.Runner;
	using System.Runtime.Remoting;
	
	#region Data Classes
	[Serializable]
	internal sealed class TestDescriptor
	{
		#region Instance variables
		//private string fTestName;
		//private TestDescriptor fParent = null;
		private string fRunPath;	// path through test 
									// names to allow re-running.
		private string ftestText;
		private string fFixtureClassName;
		private string fQualifiedFixtureName;
		private int fCountTestCases;
		private IObjectHandle fRealTestHandle;
		private TestDescriptor[] fChildTests = new TestDescriptor[0];
		private AppDomain fRealTestDomain;

		#endregion

		private static string fNestedSeparator = 
			System.IO.Path.DirectorySeparatorChar.ToString();

		#region Factory
		private static Hashtable sDescriptors = new Hashtable();
		public static TestDescriptor GetDescriptor(ITest test)
		{
			return GetDescriptor(test,null);
		}		
		private static TestDescriptor GetDescriptor(ITest test, TestDescriptor parent)
		{
			if(test !=null)
			{
				lock(sDescriptors.SyncRoot)
				{
					TestDescriptor ret = (TestDescriptor) sDescriptors[test];
					if(ret == null)
					{
						ret = new TestDescriptor(test,parent);
						sDescriptors.Add(test,ret);
					}
					return ret;
				}
			}
			else
				throw new ArgumentNullException("test");
		}
		#endregion

		#region Constructors
		private TestDescriptor(ITest test, TestDescriptor parent)
		{
			if(test==null)
				throw new ArgumentNullException("test");
			
			//this.fTestName = test.Name;
			this.fRealTestHandle = new ObjectHandle(test);
			this.fRealTestDomain = AppDomain.CurrentDomain;
			this.fCountTestCases = test.CountTestCases;
			this.ftestText = test.ToString();

			Type testType = test.GetType();
			//if(testType != null && !testType.Equals(typeof(TestSuite)))
			//{
				this.fFixtureClassName = testType.FullName;
				this.fQualifiedFixtureName = 
					fFixtureClassName+","+testType.Assembly.CodeBase;
			//}
			//else
			//{
			//	this.fFixtureClassName = string.Empty;
			//	this.fQualifiedFixtureName = string.Empty;
			//}

			//this.fParent = parent;
			if(parent == null)
			{
				this.fRunPath = fNestedSeparator+".";
			}
			else
			{
				if( parent.fRunPath.Equals(string.Empty)
					|| this.ToString().Equals(string.Empty))
				{
					this.fRunPath=string.Empty;
				}
				else
				{
					this.fRunPath = parent.fRunPath
						+ fNestedSeparator
						+ this.ToString();
				}
			}

			// create nested descriptors
			TestSuite suite = test as TestSuite;
			if(suite != null)
			{
				//this.fTestName suite.Name;
				ITest[] tests = suite.Tests;
				this.fChildTests = new TestDescriptor[tests.Length];
				int i =0;
				foreach(ITest childTest in tests)
				{
					this.fChildTests[i] = GetDescriptor(childTest,this);
					i++;
				}
			}
		}
		#endregion

		#region Properties
		//public string TestName
		//{get{return fTestName;}}

		public string RunPath
		{
			get { return this.fRunPath;}
		}
		
		public int CountTestCases
		{
			get{return this.fCountTestCases;}
		}
		
		public string TestFixtureName
		{
			get{return fFixtureClassName;}
		}

		public string QualifiedFixtureName
		{
			get{return fQualifiedFixtureName;}
		}
		public TestDescriptor[] Tests
		{
			get{return (TestDescriptor[]) this.fChildTests.Clone();}
		}
		
		#endregion

		#region Overrides
		/// <summary>
		/// Returns a string representation of the test case.
		/// </summary>
		public override string ToString() 
		{
			//return this.fTestName+"("+fFixtureName+")";
			return this.ftestText; 
		}
		#endregion
	}

	[Serializable]
	internal sealed class TestFailureDescriptor
	{
		#region Instance variables
		private TestDescriptor fTestDescriptor;
		private string fExceptionTypeName;
		private string fExceptionMessage;
		private string fExceptionStackTrace;
		private string fExceptionText;
		#endregion

		#region Constructors
		internal TestFailureDescriptor(TestFailure failure)
		{
			if(failure==null)
				throw new ArgumentNullException("failure");

			this.fTestDescriptor = TestDescriptor.GetDescriptor(failure.FailedTest);
			this.fExceptionTypeName = failure.ThrownException.GetType().FullName;
			this.fExceptionMessage = failure.ThrownException.Message;
			this.fExceptionStackTrace = failure.ThrownException.StackTrace;
			this.fExceptionText = failure.ThrownException.ToString();
		}
		#endregion

		#region Properties
		public TestDescriptor Test
		{
			get{return fTestDescriptor;}
		}
		public string ExceptionTypeName
		{
			get{return fExceptionTypeName;}
		}
		public string ExceptionMessage
		{
			get{return fExceptionMessage;}
		}
		public string ExceptionStackTrace
		{
			get{return fExceptionStackTrace;}
		}
		public string ExceptionText
		{
			get{return fExceptionStackTrace;}
		}
		#endregion

		#region Overrides
		/// <summary>Returns a short description of the failure.</summary>
		public override string ToString() 
		{
			return this.fTestDescriptor + ": " + this.fExceptionMessage;
		}
		#endregion
	}
	#endregion

	#region Events Definitions
	internal delegate void RemoteTestErrorHandler(object sender, RemoteTestErrorArgs failure);
	internal delegate void RemoteTestEventHandler(object sender, RemoteTestEventArgs test);
	
	internal class RemoteTestEventArgs : System.EventArgs
	{
		private TestDescriptor fTestDescriptor;
		internal RemoteTestEventArgs(TestDescriptor descriptor) : base()
		{
			fTestDescriptor = descriptor;
		}
		internal TestDescriptor Test
		{
			get { return fTestDescriptor;}
		}
	}
	internal class RemoteTestErrorArgs : System.EventArgs
	{
		private TestFailureDescriptor fErrorDescriptor;
		internal RemoteTestErrorArgs(TestFailureDescriptor descriptor) : base()
		{
			fErrorDescriptor = descriptor;
		}
		internal TestFailureDescriptor TestError
		{
			get { return fErrorDescriptor;}
			
		}
	}

	internal interface IRemoteTestEvents
	{
		event RemoteTestErrorHandler TestErred;
		event RemoteTestErrorHandler TestFailed;
		event RemoteTestEventHandler TestStarted;
		event RemoteTestEventHandler TestEnded;
		event RemoteTestEventHandler RunStarted;
		event RemoteTestEventHandler RunEnded;
	}
	#endregion

	/// <summary>
	/// Summary description for RemoteRunner.
	/// </summary>
	internal sealed class RemoteRunner : MarshalByRefObject, ITestListener, IRemoteTestEvents
	{
		#region Instance Variables
		public event RemoteTestErrorHandler TestErred;
		public event RemoteTestErrorHandler TestFailed;
		public event RemoteTestEventHandler TestStarted;
		public event RemoteTestEventHandler TestEnded;
		public event RemoteTestEventHandler RunStarted;
		public event RemoteTestEventHandler RunEnded;

		private StandardLoader fLoader;
		private NestedTestLoader fNestedLoader;
		private RemoteRunner fRemoteProxy;
		#endregion

		#region Constructors
		private RemoteRunner(RemoteRunner proxy) : base()
		{
			this.fRemoteProxy = proxy;
			// only create loader in remote scenario
			this.fLoader = new StandardLoader();
			this.fNestedLoader = new NestedTestLoader(fLoader);
			Console.SetOut(proxy.RemoteStdOut);
			Console.SetError(proxy.RemoteStdErr);
		}
		
		internal RemoteRunner() : base()
		{
			// TODO: Add constructor logic here
			// TODO: Add facilities for redirecting Stdin,out,err
			//Console.Open
		}
		#endregion
		
		#region TestRunner Methods
		/// <summary>
		/// Runs Test in a temporary AppDomain, then unloads the
		/// AppDomain
		/// </summary>
		public void Run(string assemblyQualifiedTestClassName)
		{
			this.Run(assemblyQualifiedTestClassName,@".\");
		}
		
		/// <summary>
		/// Runs Test in a temporary AppDomain, then unloads the
		/// AppDomain
		/// </summary>
		public void Run(string assemblyQualifiedTestClassName, string testPath)
		{
			RemoteRunner stub = CreateRemoteRunner(
				GetDirectory(assemblyQualifiedTestClassName));
			RemoteTestEventArgs test=null;
			try
			{
				test = new RemoteTestEventArgs(stub.RemoteGetDescriptor(
					assemblyQualifiedTestClassName,testPath));
				if(RunStarted!=null)
					this.RunStarted(this,test);
				stub.RemoteRun(assemblyQualifiedTestClassName,testPath);
			}
			finally
			{
				AppDomain.Unload(stub.RemoteDomain);
				if(RunEnded!=null)
					this.RunEnded(this,test);
			}
		}

		private string GetDirectory(string qualifiedName)
		{
			string fileName = qualifiedName.Split(new char[]{','},2)[1];
			return System.IO.Path.GetDirectoryName(fileName);
		}
		
		public string[] GetAssemblyTestClasses(string assemblyFileName)
		{
			RemoteRunner stub = CreateRemoteRunner(
				System.IO.Path.GetDirectoryName(assemblyFileName));
			try
			{
				return stub.RemoteGetAssemblyTestClasses(assemblyFileName);
			}
			finally
			{
				AppDomain.Unload(stub.RemoteDomain);
			}
		}

		public TestDescriptor GetTestDescriptor(string qualifiedName)
		{
			RemoteRunner stub = CreateRemoteRunner(
				System.IO.Path.GetDirectoryName(qualifiedName));
			try
			{
				return stub.RemoteGetDescriptor(qualifiedName,".");
			}
			finally
			{
				AppDomain.Unload(stub.RemoteDomain);
			}
		}
		#endregion

		#region Remoter
		private AppDomain RemoteDomain
		{
			get {return AppDomain.CurrentDomain;}
		}
		
		private System.IO.TextWriter RemoteStdOut
		{
			get {return Console.Out;}
		}

		private System.IO.TextWriter RemoteStdErr
		{
			get {return Console.Error;}
		}

		private TestDescriptor RemoteGetDescriptor(
			string assemblyQualifiedTestClassName,string testPath)
		{
			return TestDescriptor.GetDescriptor(
				fNestedLoader.LoadTest(assemblyQualifiedTestClassName
				,testPath));
		}
		private void RemoteRun(string assemblyQualifiedTestClassName
			,string testPath)
		{

			ITest test = fNestedLoader.LoadTest(assemblyQualifiedTestClassName
				,testPath);
			TestResult result = new TestResult();
			result = new TestResult();
			result.AddListener(this);
			test.Run(result);
		}
		
		private string[] RemoteGetAssemblyTestClasses(
			string assemblyQualifiedTestClassName)
		{
			return new AssemblyTestCollector(assemblyQualifiedTestClassName
				,this.fLoader).CollectTestsClassNames();
		}
		
		private RemoteRunner CreateRemoteRunner(string basePath)
		{
			AppDomain domain = AppDomain.CreateDomain(
				"UnloadableDomain"
				,null
				,null //, basePath
				,null
				,false);
			System.Runtime.Remoting.Lifetime.ILease lease = 
				(System.Runtime.Remoting.Lifetime.ILease) 
				domain.InitializeLifetimeService();
			RemoteRunner ret;
			ret = (RemoteRunner)domain.CreateInstance(
				typeof(RemoteRunner).Assembly.FullName
				,typeof(RemoteRunner).FullName
				,false					//Case sensitive
				,BindingFlags.Instance | BindingFlags.NonPublic
				,null					//Defult binder
				,new object[1]{this} 	//constructor args
				,null					//
				,null					//
				,null).Unwrap();
			//ret.Remoteout.;
			//ret.fRemoteProxy = this;
			//TODO: manage lifetime of Loader in temp domain
			//ret.TestFailed+= new RemoteTestErrorHandler(ReceiveTestFailed);
			//ret.TestErred+= new RemoteTestErrorHandler(ReceiveTestErred);
			//ret.TestEnded+= new RemoteTestEventHandler(ReceiveTestEnded);
			//ret.TestStarted+= new RemoteTestEventHandler(ReceiveTestStarted);
			return ret;
		}
		private void ReceiveTestErred(TestFailureDescriptor testFailure)
		{
			if(TestErred!=null)
				TestErred(this, new RemoteTestErrorArgs(testFailure));
		}
		private void ReceiveTestFailed(TestFailureDescriptor testError)
		{
			if(TestFailed!=null)
				TestFailed(this, new RemoteTestErrorArgs(testError));
		}
		private void ReceiveTestEnded(TestDescriptor test)
		{
			if(TestEnded!=null)
				TestEnded(this,new RemoteTestEventArgs(test));
		}
		private void ReceiveTestStarted(TestDescriptor test)
		{
			if(TestStarted!=null)
				TestStarted(this,new RemoteTestEventArgs(test));
		}
		#endregion
		
		#region ITest Methods
		void ITestListener.AddError(ITest test, Exception ex)
		{
			this.fRemoteProxy.ReceiveTestErred(
				new TestFailureDescriptor(new TestFailure(test,ex)));
		}
    
		void ITestListener.AddFailure(ITest test, AssertionFailedError ex)
		{
			this.fRemoteProxy.ReceiveTestFailed(
				new TestFailureDescriptor(new TestFailure(test,ex)));
		}
		void ITestListener.EndTest(ITest test)
		{
			this.fRemoteProxy.ReceiveTestEnded(
				TestDescriptor.GetDescriptor(test));
		}
		void ITestListener.StartTest(ITest test)
		{
			this.fRemoteProxy.ReceiveTestStarted(
				TestDescriptor.GetDescriptor(test));
		}
		#endregion
	}
}