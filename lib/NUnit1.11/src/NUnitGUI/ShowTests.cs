namespace NUnit.GUI 
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using NUnit.Framework;
	using NUnit.Runner;

	/// <summary>
	///    Summary description for ShowTests.
	/// </summary>
	internal class ShowTests : System.Windows.Forms.Form//, ITestListener 
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TreeView testBrowser;
		private System.Windows.Forms.Button collapseButton;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.Panel buttonPanel;

		private bool testFailure;
		//private Hashtable testNameAssociation = new Hashtable();
		private Hashtable fNodeDescriptorAssociation = new Hashtable();
		private Hashtable fNameNodeAssociation = new Hashtable();
		private ArrayList changedNodes = new ArrayList();
		private RemoteRunner fRunner;
		private string fRootTestName;
		//private ILoader fLoader;

		internal ShowTests(string rootTestName, RemoteRunner testRunner) 
		{
			// Required for WinForm Designer support
			InitializeComponent();
			fRunner = testRunner;
			fRootTestName = rootTestName;
			//fLoader = loader;

			TestDescriptor rootTest = fRunner.GetTestDescriptor(rootTestName);
			//ITest rootTest = loader.LoadTest(rootTestname);

			TreeNode node = new TreeNode(rootTest.ToString(), GetTestNodes(rootTest));
			fNodeDescriptorAssociation.Add(node,rootTest);
			fNameNodeAssociation.Add(rootTest.RunPath,node);

			TreeNode[] allNodes = new TreeNode[] { node };

			testBrowser.Nodes.Clear();
			testBrowser.Nodes.AddRange(allNodes); 
		}

		// Don't use enum here because it stops the form designer 
		// from recognising
		private const int IconClosedFolder = 0;
		private const int IconOpenFolder = 1;
		private const int IconOk = 2;
		private const int IconError = 3;
		private const int IconFailure = 4;

		private TreeNode[] GetTestNodes(TestDescriptor test) 
		{
			//string qualifiedName;
			TreeNode[] testNodes = null;
			TestDescriptor[] tests = test.Tests;

			//TestSuite suite = maybeSuite as TestSuite;
			testNodes = new TreeNode[tests.Length];
			if(tests.Length>0) 
			{
				int i = 0;
				foreach(TestDescriptor childTest in tests) 
				{
					TreeNode testNode;
				
					testNode = new TreeNode(childTest.ToString(),
						GetTestNodes(childTest));
					
					if(childTest.RunPath.Equals(string.Empty))
					{
						testNode.ForeColor=SystemColors.GrayText;
					}
					else
					{
						// don't add if runpath is empty
						fNameNodeAssociation.Add(childTest.RunPath,testNode);
						fNodeDescriptorAssociation.Add(testNode,childTest);
					}
					testNodes[i++] = testNode;
				}
			}
			return testNodes;
		}

		private TestDescriptor GetNodeTest(TreeNode node)
		{
			TestDescriptor test=null;
			if(node!=null)
			{
				test = (TestDescriptor)	fNodeDescriptorAssociation[node];
			}
			return test;
		}

		private TreeNode GetTestNode(TestDescriptor test)
		{
			TreeNode node=null;
			if(test!=null && !test.RunPath.Equals(string.Empty))
			{
				node = (TreeNode) fNameNodeAssociation[test.RunPath];
			}
			return node;
		}

		#region GUI Event Handler
		private void AfterSelect(object source, TreeViewEventArgs e) 
		{
			TestDescriptor test = GetNodeTest(e.Node);//  testBrowser.SelectedNode);
			if(test != null && !test.RunPath.Equals(string.Empty))
			{
				runButton.Enabled = true;
			}
			else
			{
				runButton.Enabled = false;
			}
		}

		private void RunButtonClicked(object sender, System.EventArgs e) 
		{
			this.ClearResults();
			this.RunTest();
		}
		private void CloseButtonClicked(object sender, System.EventArgs e) 
		{
			this.Close();
		}

		private void CollapseButtonClicked(object sender, System.EventArgs e) 
		{ 
			testBrowser.CollapseAll(); 
		}
		#endregion

		private void ClearResults() 
		{
			foreach(TreeNode node in changedNodes) 
			{
				node.ImageIndex = IconClosedFolder;
			}
			changedNodes.Clear();
		}

		private void RunTest() 
		{
			TreeNode node = testBrowser.SelectedNode;
			TestDescriptor test = GetNodeTest(node);
			if(test != null) 
			{
				if(node.GetNodeCount(false) > 0)
					node.ExpandAll();
				string testPath = test.RunPath;
				if(!testPath.Equals(string.Empty))
					fRunner.Run(fRootTestName,testPath);
				testBrowser.SelectedNode = null;
				runButton.Enabled = false;
			}
		}

		#region Test Events
		private void RemoteTestFailed(object sender, RemoteTestErrorArgs e)
		{
			testFailure = true;
			TreeNode node = GetTestNode(e.TestError.Test);
			if(node!=null)
			{
				node.ImageIndex = IconFailure;
				changedNodes.Add(node);
			}
		}

		private void RemoteTestErred(object sender, RemoteTestErrorArgs e)
		{
			testFailure = true;
			TreeNode node = GetTestNode(e.TestError.Test);
			if(node!=null)
			{
				node.ImageIndex = IconError;
				changedNodes.Add(node);
			}
		}

		private void RemoteTestStarted(object sender, RemoteTestEventArgs e)
		{
			testFailure = false;
		}

		private void RemoteTestEnded(object sender, RemoteTestEventArgs e)
		{
			if(testFailure == false) 
			{
				TreeNode node = GetTestNode(e.Test);
				if(node!=null)
				{
					node.ImageIndex = IconOk;
					changedNodes.Add(node);
				}
			}
		}
		
		private void RemoteRunStarted(object sender, RemoteTestEventArgs e)
		{
		}

		private void RemoteRunEnded(object sender, RemoteTestEventArgs e)
		{
		}
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		///    Required method for Designer support - do not modify
		///    the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ShowTests));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.collapseButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.testBrowser = new System.Windows.Forms.TreeView();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // collapseButton
            // 
            this.collapseButton.Location = new System.Drawing.Point(89, 8);
            this.collapseButton.Name = "collapseButton";
            this.collapseButton.Size = new System.Drawing.Size(94, 30);
            this.collapseButton.TabIndex = 3;
            this.collapseButton.Text = "Collapse &All";
            this.collapseButton.Click += new System.EventHandler(this.CollapseButtonClicked);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(351, 8);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(73, 30);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClicked);
            // 
            // runButton
            // 
            this.runButton.Enabled = false;
            this.runButton.Location = new System.Drawing.Point(9, 8);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(70, 30);
            this.runButton.TabIndex = 2;
            this.runButton.Text = "&Run";
            this.runButton.Click += new System.EventHandler(this.RunButtonClicked);
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                      this.collapseButton,
                                                                                      this.closeButton,
                                                                                      this.runButton});
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 275);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(438, 46);
            this.buttonPanel.TabIndex = 0;
            // 
            // testBrowser
            // 
            this.testBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testBrowser.HideSelection = false;
            this.testBrowser.ImageList = this.imageList;
            this.testBrowser.Name = "testBrowser";
            this.testBrowser.Size = new System.Drawing.Size(438, 275);
            this.testBrowser.TabIndex = 1;
            this.testBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect);
            // 
            // ShowTests
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(438, 321);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.testBrowser,
                                                                          this.buttonPanel});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowTests";
            this.Text = "Show Tests";
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
	}
}
#if false
namespace NUnit.GUI 
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using NUnit.Framework;

	/// <summary>
	///    Summary description for ShowTests.
	/// </summary>
	public class ShowTests : System.Windows.Forms.Form, ITestListener 
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TreeView testBrowser;
		private System.Windows.Forms.Button collapseButton;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.Panel buttonPanel;

		private bool testFailure;
		private Hashtable testNameAssociation = new Hashtable();
		private Hashtable nodeTestAssociation = new Hashtable();
		private ArrayList changedNodes = new ArrayList();
		private ITestRunner runner;

		public ShowTests(ITest root, ITestRunner testRunner) 
		{
			runner = testRunner;

			// Required for Win Form Designer support
			InitializeComponent();

			testNameAssociation.Add(root.ToString(), root);
			TreeNode node = new TreeNode(root.ToString(), GetTestNodes(root));
			nodeTestAssociation.Add(root, node);

			TreeNode[] allNodes = new TreeNode[] { node };

			testBrowser.Nodes.Clear();
			testBrowser.Nodes.AddRange(allNodes); 
		}

		// Don't use enum here because it stops the form designer from recognising
		private const int IconClosedFolder = 0;
		private const int IconOpenFolder = 1;
		private const int IconOk = 2;
		private const int IconError = 3;
		private const int IconFailure = 4;

		private TreeNode[] GetTestNodes(ITest maybeSuite) 
		{
			TreeNode[] testNodes = null;
			ArrayList tests = null;

			TestSuite suite = maybeSuite as TestSuite;
			if(suite != null) 
			{
				tests = new ArrayList(suite.Tests);
				testNodes = new TreeNode[tests.Count]; 
			}

			int i = 0;
			foreach(ITest test in tests) 
			{
				TreeNode testNode;
				if(test is TestSuite)
					testNode = new TreeNode(test.ToString(), GetTestNodes(test));
				else
					testNode = new TreeNode(test.ToString());

				nodeTestAssociation.Add(test, testNode);
				testNameAssociation.Add(test.ToString(), test);
				testNodes[i++] = testNode;
			}

			return testNodes;
		}

		private void AfterSelect(object source, TreeViewEventArgs e) 
		{
			if(testBrowser.SelectedNode != null)
				runButton.Enabled = true;
			else
				runButton.Enabled = false;
		}


		protected void RunButtonClicked(object sender, System.EventArgs e) 
		{
			ClearResults();
			RunTest();
		}

		private void ClearResults() 
		{
			foreach(TreeNode node in changedNodes) 
			{
				node.ImageIndex = IconClosedFolder;
			}
			changedNodes.Clear();
		}

		private void RunTest() 
		{
			if(testBrowser.SelectedNode != null) 
			{
				TreeNode node = testBrowser.SelectedNode;
				if(node.GetNodeCount(false) > 0)
					node.ExpandAll();
				runner.Run((ITest)testNameAssociation[node.Text]);
				testBrowser.SelectedNode = null;
				runButton.Enabled = false;
			}
		}

		private void CloseButtonClicked(object sender, System.EventArgs e) 
		{
			Close();	}

		private void CollapseButtonClicked(object sender, System.EventArgs e) 
		{ 
			testBrowser.CollapseAll(); 
		}

		public void AddError(ITest test, Exception t) 
		{
			testFailure = true;
			TreeNode node = (TreeNode)nodeTestAssociation[test];
			node.ImageIndex = IconError;
			changedNodes.Add(node);
		}
		public void AddFailure(ITest test, AssertionFailedError t) 
		{
			testFailure = true;
			TreeNode node = (TreeNode)nodeTestAssociation[test];
			node.ImageIndex = IconFailure;
			changedNodes.Add(node);
		}

		public void EndTest(ITest test) 
		{
			if(testFailure == false) 
			{
				TreeNode node = (TreeNode)nodeTestAssociation[test];
				node.ImageIndex = IconOk;
				changedNodes.Add(node);
			}
		}

		public void StartTest(ITest test) 
		{
			testFailure = false;
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		///    Required method for Designer support - do not modify
		///    the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.collapseButton = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.buttonPanel = new System.Windows.Forms.Panel();
			this.testBrowser = new System.Windows.Forms.TreeView();
			this.buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// collapseButton
			// 
			this.collapseButton.Location = new System.Drawing.Point(97, 8);
			this.collapseButton.Name = "collapseButton";
			this.collapseButton.Size = new System.Drawing.Size(103, 32);
			this.collapseButton.TabIndex = 1;
			this.collapseButton.Text = "Collapse All";
			this.collapseButton.Click += new System.EventHandler(this.CollapseButtonClicked);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.closeButton.Location = new System.Drawing.Point(384, 8);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(80, 32);
			this.closeButton.TabIndex = 2;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.CloseButtonClicked);
			// 
			// runButton
			// 
			this.runButton.Enabled = false;
			this.runButton.Location = new System.Drawing.Point(10, 8);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(77, 32);
			this.runButton.TabIndex = 0;
			this.runButton.Text = "Run";
			this.runButton.Click += new System.EventHandler(this.RunButtonClicked);
			// 
			// buttonPanel
			// 
			this.buttonPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.collapseButton,
																					  this.closeButton,
																					  this.runButton});
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.Location = new System.Drawing.Point(0, 292);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Size = new System.Drawing.Size(480, 48);
			this.buttonPanel.TabIndex = 2;
			// 
			// testBrowser
			// 
			this.testBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testBrowser.HideSelection = false;
			this.testBrowser.ImageList = this.imageList;
			this.testBrowser.Name = "testBrowser";
			this.testBrowser.Size = new System.Drawing.Size(480, 292);
			this.testBrowser.TabIndex = 0;
			this.testBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect);
			// 
			// ShowTests
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(480, 340);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.testBrowser,
																		  this.buttonPanel});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ShowTests";
			this.Text = "Show Tests";
			this.buttonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
		}
	}
}
#endif