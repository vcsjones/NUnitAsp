namespace NUnit.GUI
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Reflection;
	using System.Security;
	using System.Windows.Forms;
	using NUnit.Framework;
	using NUnit.Runner;

	/// <summary>
	///    Main form that displays the status, progress and results of
	///    a test. Allows users to interactively select test to be run.
	/// </summary>
	internal class TestRunnerForm : System.Windows.Forms.Form//, ITestRunner
	{
		#region Instance Variables
		/// <summary>
		///    Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox detailTextBox;
		private System.Windows.Forms.OpenFileDialog FindModuleDialog;
		private System.Windows.Forms.StatusBarPanel statusTime;
		private System.Windows.Forms.StatusBarPanel statusFailCount;
		private System.Windows.Forms.StatusBarPanel statusErrorCount;
		private System.Windows.Forms.StatusBarPanel statusRunCount;
		private System.Windows.Forms.StatusBarPanel statusTestCount;
		private System.Windows.Forms.StatusBarPanel statusMain;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.TextBox StdOutText;
		private System.Windows.Forms.TextBox StdErrText;
		private System.Windows.Forms.ListBox TestErrorsText;
		private System.Windows.Forms.TabPage TestErrorsTabPage;
		private System.Windows.Forms.TabPage StdErrTabPage;
		private System.Windows.Forms.TabPage StdOutTabPage;
		private System.Windows.Forms.TabControl OutputTab;
		private System.Windows.Forms.Label typeNameLabel;
		private System.Windows.Forms.ComboBox TypeNameSelector;
		private System.Windows.Forms.TextBox AssemblyFileNameText;
		private System.Windows.Forms.Label assemblyNameLabel;
		private System.Windows.Forms.Button RunButton;
		private System.Windows.Forms.Button BrowseAssemblyButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem testBrowserItem;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MainMenu mainMenu1;

		//private TestResult result;
		//private ShowTests testBrowser = null;
		private System.Windows.Forms.Splitter splitter1;
		private NUnit.GUI.ProgressBar runTestProgressBar;
		public String initialTestToRun = "";
		private DateTime fStartTime;
		//private UnloadingLoader loader = new UnloadingLoader();
		private readonly Color fErrorColor = Color.Red;
		private readonly Color fStartColor = Color.Lime;
		private RemoteRunner fRunner = new RemoteRunner();
		private int fRunCount;
		private int fErrorCount;
		private int fFailureCount;
		#endregion
		
		#region Constructors, Desctructors, Disposable
		public TestRunnerForm()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			fRunner.TestStarted+=new RemoteTestEventHandler(RemoteTestStarted);
			fRunner.TestEnded+=new RemoteTestEventHandler(RemoteTestEnded);
			fRunner.TestErred+=new RemoteTestErrorHandler(RemoteTestErred);
			fRunner.TestFailed+=new RemoteTestErrorHandler(RemoteTestFailed);
			fRunner.RunStarted+= new RemoteTestEventHandler(RemoteRunStarted);
			fRunner.RunEnded+= new RemoteTestEventHandler(RemoteRunEnded);
			Console.SetError(new TextBoxWriter(StdErrText));
			Console.SetOut(new TextBoxWriter(StdOutText));
			ClearStatus();
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
		#endregion

		#region GUI Event Handlers
		protected void SelectAndDisplayDetail(object sender, System.EventArgs e)
		{
			setDetailText((TestFailureDescriptor)TestErrorsText.SelectedItem);
		}

		protected void TypeName_Click(object sender, System.EventArgs e)
		{
			ClearStatus();
		}

		protected void BrowseAssemblyButton_Click(object sender, System.EventArgs e)
		{
			BrowseForAssembly();
		}

		protected void AssemblyFileNameText_TextChanged(object sender, EventArgs e)
		{
			bool flag=false;
			PopulateTestFixureList();
			if (TypeNameSelector.Items.Count > 0)
			{
				flag=true;
			}
			SetAssemblyStatus(flag);
			ClearStatus();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			//HACK:
			this.SuspendLayout();
			OutputTab.CreateControl();
			OutputTab.SelectedIndex=2;
			OutputTab.SelectedIndex=1;
			OutputTab.SelectedIndex=0;
			this.ResumeLayout();
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			if (initialTestToRun.Length > 0)
			{
				string assemblyQualifiedName = initialTestToRun;
				initialTestToRun=string.Empty;
				int comma = assemblyQualifiedName.IndexOf(',');
				if(comma != -1)
				{
					AssemblyFileNameText.Text = assemblyQualifiedName.Substring(comma+1);
					TypeNameSelector.Text = assemblyQualifiedName.Substring(0, comma);
					Run(assemblyQualifiedName);
				}
				else
				{
					AssemblyFileNameText.Text = assemblyQualifiedName;
				}
			}
		}

		private void ExitClicked(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ShowAbout(object sender, System.EventArgs e)
		{
			new AboutBox().ShowDialog(this);
		}

		private void ShowTestBrowser(object sender, System.EventArgs e)
		{
			//BUGBUG: enable test browsing
			ShowTests testBrowser = new ShowTests(QualifiedTestFixtureName
				//,this.loader
				,this.fRunner);
			testBrowser.ShowDialog(this);
		}
		protected void RunButton_Click(object sender, System.EventArgs e)
		{
			Run(QualifiedTestFixtureName);
		}
		#endregion

		#region Test Events
		private void RemoteTestFailed(object sender, RemoteTestErrorArgs e)
		{
			fFailureCount++;
			runTestProgressBar.ForeColor = fErrorColor;
			TestErrorsText.Items.Insert(TestErrorsText.Items.Count, e.TestError);
		}

		private void RemoteTestErred(object sender, RemoteTestErrorArgs e)
		{
			fErrorCount++;
			runTestProgressBar.ForeColor = fErrorColor;
			TestErrorsText.Items.Insert(TestErrorsText.Items.Count, e.TestError);
		}

		private void RemoteTestStarted(object sender, RemoteTestEventArgs e)
		{
			statusMain.Text = "Running test: " + e.Test.ToString();
		}

		private void RemoteTestEnded(object sender, RemoteTestEventArgs e)
		{
			fRunCount++;
			runTestProgressBar.PerformStep();
			statusMain.Text = "Finished Running test: " + e.Test.ToString();
			DisplaySummary();
		}
		
		private void RemoteRunStarted(object sender, RemoteTestEventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			this.ClearStatus();
			fStartTime = DateTime.Now;
			runTestProgressBar.Maximum = e.Test.CountTestCases;
			statusTestCount.Text="Tests: " + e.Test.CountTestCases;
		}

		private void RemoteRunEnded(object sender, RemoteTestEventArgs e)
		{
			statusMain.Text = "Finished";
			Cursor = Cursors.Default;
			if(this.TestErrorsText.Items.Count>0)
			this.TestErrorsText.SelectedIndex=0;
		}
		#endregion

		#region UI Helpers
		/// <summary>
		///    Resets the screen components to initial values
		/// </summary>
		private void ClearStatus()
		{
			runTestProgressBar.Value= 0;
			runTestProgressBar.ForeColor = fStartColor;
			StdErrText.Clear();
			StdOutText.Clear();
			TestErrorsText.Items.Clear();
			TestErrorsText.ResetText();
			detailTextBox.Text = "";

			statusMain.Text = "Ready";
			statusRunCount.Text = "Run";
			statusErrorCount.Text = "Error";
			statusFailCount.Text = "Fail";
			statusTime.Text = "Time";
			statusTestCount.Text = "Tests";
			fRunCount=0;
			fErrorCount=0;
			fFailureCount=0;
			//TestErrorsTabPage.Text = "Errors and Failures";
			this.Update();
		}
		/// <summary>
		/// Updates the UI display with the test results
		/// </summary>
		private void DisplaySummary()
		{
			statusRunCount.Text="Run: "+fRunCount.ToString();
			statusFailCount.Text="Failed: "+fFailureCount.ToString();
			statusErrorCount.Text="Errors: "+fErrorCount.ToString();
			statusTime.Text="Time: "+(DateTime.Now-this.fStartTime).ToString();
		}

		private void setDetailText(TestFailureDescriptor test)
		{
			if (test != null)
				detailTextBox.Text = BaseTestRunner.FilterStack(
					test.ExceptionText);
		}
		
		private void BrowseForAssembly()
		{
			FindModuleDialog.FileName=AssemblyFileNameText.Text;
			if(FindModuleDialog.ShowDialog()==DialogResult.OK)
			{
				AssemblyFileNameText.Text=FindModuleDialog.FileName;
			}
		}
		
		protected virtual void SetAssemblyStatus(bool flag)
		{
			TypeNameSelector.Enabled=flag;
			typeNameLabel.Enabled = flag;
			testBrowserItem.Enabled = flag;
			RunButton.Enabled=flag;
			this.testBrowserItem.Enabled=flag;
		}

		private String QualifiedTestFixtureName
		{
			get
			{
				return TypeNameSelector.Text + ","
					+ AssemblyFileNameText.Text;
			}
		}

		private void PopulateTestFixureList()
		{
			string assemblyFileName = AssemblyFileNameText.Text; 
			TypeNameSelector.Items.Clear();
			String typeName = string.Empty;
			if(assemblyFileName.Length > 0
				&& File.Exists(assemblyFileName))
			{
				TypeNameSelector.Items.AddRange(
					this.fRunner.GetAssemblyTestClasses(
					AssemblyFileNameText.Text));
				if (TypeNameSelector.Items.Count > 0)
				{
					typeName = TypeNameSelector.Items[0].ToString();
					foreach(String s in TypeNameSelector.Items)
					{
						if(s.EndsWith("AllTests"))
						{
							typeName = s;
						}
					}
				}
			}	
			TypeNameSelector.Text = typeName;
		}
		#endregion

		#region TestRunner Methods
		/// <summary>
		/// Runs actual test
		/// </summary>
		public void Run(String assemblyQualifiedTestClassName)
		{
			try
			{
				this.fRunner.Run(assemblyQualifiedTestClassName);
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine(ex.ToString());
			}
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		///    Required method for Designer support - do not modify
		///    the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TestRunnerForm));
            this.detailTextBox = new System.Windows.Forms.TextBox();
            this.FindModuleDialog = new System.Windows.Forms.OpenFileDialog();
            this.TypeNameSelector = new System.Windows.Forms.ComboBox();
            this.statusTime = new System.Windows.Forms.StatusBarPanel();
            this.statusFailCount = new System.Windows.Forms.StatusBarPanel();
            this.typeNameLabel = new System.Windows.Forms.Label();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusMain = new System.Windows.Forms.StatusBarPanel();
            this.statusTestCount = new System.Windows.Forms.StatusBarPanel();
            this.statusRunCount = new System.Windows.Forms.StatusBarPanel();
            this.statusErrorCount = new System.Windows.Forms.StatusBarPanel();
            this.StdOutTabPage = new System.Windows.Forms.TabPage();
            this.StdOutText = new System.Windows.Forms.TextBox();
            this.TestErrorsText = new System.Windows.Forms.ListBox();
            this.BrowseAssemblyButton = new System.Windows.Forms.Button();
            this.OutputTab = new System.Windows.Forms.TabControl();
            this.TestErrorsTabPage = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.StdErrTabPage = new System.Windows.Forms.TabPage();
            this.StdErrText = new System.Windows.Forms.TextBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.testBrowserItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.assemblyNameLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AssemblyFileNameText = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.runTestProgressBar = new NUnit.GUI.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.statusTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusFailCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusTestCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusRunCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusErrorCount)).BeginInit();
            this.StdOutTabPage.SuspendLayout();
            this.OutputTab.SuspendLayout();
            this.TestErrorsTabPage.SuspendLayout();
            this.StdErrTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailTextBox
            // 
            this.detailTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.detailTextBox.Font = new System.Drawing.Font("Courier New", 9F);
            this.detailTextBox.Location = new System.Drawing.Point(0, 192);
            this.detailTextBox.Multiline = true;
            this.detailTextBox.Name = "detailTextBox";
            this.detailTextBox.ReadOnly = true;
            this.detailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detailTextBox.Size = new System.Drawing.Size(656, 68);
            this.detailTextBox.TabIndex = 7;
            this.detailTextBox.Text = "textBox1";
            // 
            // FindModuleDialog
            // 
            this.FindModuleDialog.Filter = "Dll\'s|*.dll|Executables|*.exe|All Files|*.*";
            this.FindModuleDialog.ReadOnlyChecked = true;
            this.FindModuleDialog.Title = "Select Assembly";
            // 
            // TypeNameSelector
            // 
            this.TypeNameSelector.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right);
            this.TypeNameSelector.DropDownWidth = 384;
            this.TypeNameSelector.Enabled = false;
            this.TypeNameSelector.Location = new System.Drawing.Point(117, 48);
            this.TypeNameSelector.Name = "TypeNameSelector";
            this.TypeNameSelector.Size = new System.Drawing.Size(445, 21);
            this.TypeNameSelector.TabIndex = 3;
            this.TypeNameSelector.SelectedIndexChanged += new System.EventHandler(this.TypeName_Click);
            // 
            // statusTime
            // 
            this.statusTime.MinWidth = 150;
            this.statusTime.ToolTipText = "Elapsed time since suite started";
            this.statusTime.Width = 150;
            // 
            // statusFailCount
            // 
            this.statusFailCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusFailCount.MinWidth = 70;
            this.statusFailCount.ToolTipText = "Number of failures encountered";
            this.statusFailCount.Width = 70;
            // 
            // typeNameLabel
            // 
            this.typeNameLabel.Enabled = false;
            this.typeNameLabel.Location = new System.Drawing.Point(15, 53);
            this.typeNameLabel.Name = "typeNameLabel";
            this.typeNameLabel.Size = new System.Drawing.Size(85, 23);
            this.typeNameLabel.TabIndex = 0;
            this.typeNameLabel.Text = "Test Fixture";
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 407);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                         this.statusMain,
                                                                                         this.statusTestCount,
                                                                                         this.statusRunCount,
                                                                                         this.statusErrorCount,
                                                                                         this.statusFailCount,
                                                                                         this.statusTime});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(664, 23);
            this.statusBar.TabIndex = 0;
            // 
            // statusMain
            // 
            this.statusMain.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusMain.Width = 218;
            // 
            // statusTestCount
            // 
            this.statusTestCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusTestCount.MinWidth = 70;
            this.statusTestCount.ToolTipText = "Number of tests in suite";
            this.statusTestCount.Width = 70;
            // 
            // statusRunCount
            // 
            this.statusRunCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusRunCount.MinWidth = 70;
            this.statusRunCount.ToolTipText = "Number of tests completed";
            this.statusRunCount.Width = 70;
            // 
            // statusErrorCount
            // 
            this.statusErrorCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusErrorCount.MinWidth = 70;
            this.statusErrorCount.ToolTipText = "Number of errors encountered";
            this.statusErrorCount.Width = 70;
            // 
            // StdOutTabPage
            // 
            this.StdOutTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                        this.StdOutText});
            this.StdOutTabPage.Location = new System.Drawing.Point(4, 22);
            this.StdOutTabPage.Name = "StdOutTabPage";
            this.StdOutTabPage.Size = new System.Drawing.Size(657, 261);
            this.StdOutTabPage.TabIndex = 2;
            this.StdOutTabPage.Text = "Standard Output";
            // 
            // StdOutText
            // 
            this.StdOutText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StdOutText.Font = new System.Drawing.Font("Courier New", 9F);
            this.StdOutText.Multiline = true;
            this.StdOutText.Name = "StdOutText";
            this.StdOutText.ReadOnly = true;
            this.StdOutText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.StdOutText.Size = new System.Drawing.Size(657, 261);
            this.StdOutText.TabIndex = 0;
            this.StdOutText.Text = "StdOutText Text";
            // 
            // TestErrorsText
            // 
            this.TestErrorsText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestErrorsText.Font = new System.Drawing.Font("Courier New", 9F);
            this.TestErrorsText.HorizontalExtent = 2000;
            this.TestErrorsText.HorizontalScrollbar = true;
            this.TestErrorsText.IntegralHeight = false;
            this.TestErrorsText.ItemHeight = 15;
            this.TestErrorsText.Name = "TestErrorsText";
            this.TestErrorsText.Size = new System.Drawing.Size(656, 184);
            this.TestErrorsText.TabIndex = 6;
            this.TestErrorsText.SelectedIndexChanged += new System.EventHandler(this.SelectAndDisplayDetail);
            // 
            // BrowseAssemblyButton
            // 
            this.BrowseAssemblyButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.BrowseAssemblyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseAssemblyButton.Location = new System.Drawing.Point(569, 8);
            this.BrowseAssemblyButton.Name = "BrowseAssemblyButton";
            this.BrowseAssemblyButton.Size = new System.Drawing.Size(75, 28);
            this.BrowseAssemblyButton.TabIndex = 2;
            this.BrowseAssemblyButton.Text = "&Browse...";
            this.BrowseAssemblyButton.Click += new System.EventHandler(this.BrowseAssemblyButton_Click);
            // 
            // OutputTab
            // 
            this.OutputTab.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.TestErrorsTabPage,
                                                                                    this.StdOutTabPage,
                                                                                    this.StdErrTabPage});
            this.OutputTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputTab.Location = new System.Drawing.Point(0, 121);
            this.OutputTab.Name = "OutputTab";
            this.OutputTab.SelectedIndex = 0;
            this.OutputTab.Size = new System.Drawing.Size(664, 286);
            this.OutputTab.TabIndex = 5;
            // 
            // TestErrorsTabPage
            // 
            this.TestErrorsTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                            this.TestErrorsText,
                                                                                            this.splitter1,
                                                                                            this.detailTextBox});
            this.TestErrorsTabPage.Location = new System.Drawing.Point(4, 22);
            this.TestErrorsTabPage.Name = "TestErrorsTabPage";
            this.TestErrorsTabPage.Size = new System.Drawing.Size(656, 260);
            this.TestErrorsTabPage.TabIndex = 0;
            this.TestErrorsTabPage.Text = "Errors and Failures";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 184);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(656, 8);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // StdErrTabPage
            // 
            this.StdErrTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                        this.StdErrText});
            this.StdErrTabPage.Location = new System.Drawing.Point(4, 22);
            this.StdErrTabPage.Name = "StdErrTabPage";
            this.StdErrTabPage.Size = new System.Drawing.Size(657, 261);
            this.StdErrTabPage.TabIndex = 1;
            this.StdErrTabPage.Text = "Standard Error";
            // 
            // StdErrText
            // 
            this.StdErrText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StdErrText.Font = new System.Drawing.Font("Courier New", 9F);
            this.StdErrText.Multiline = true;
            this.StdErrText.Name = "StdErrText";
            this.StdErrText.ReadOnly = true;
            this.StdErrText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StdErrText.Size = new System.Drawing.Size(657, 261);
            this.StdErrText.TabIndex = 0;
            this.StdErrText.Text = "StdErrText Text";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem2,
                                                                                      this.testBrowserItem,
                                                                                      this.menuItem4,
                                                                                      this.menuItem5});
            this.menuItem1.Text = "&NUnit";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.menuItem2.ShowShortcut = false;
            this.menuItem2.Text = "&About NUnit...";
            this.menuItem2.Click += new System.EventHandler(this.ShowAbout);
            // 
            // testBrowserItem
            // 
            this.testBrowserItem.Enabled = false;
            this.testBrowserItem.Index = 1;
            this.testBrowserItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.testBrowserItem.ShowShortcut = false;
            this.testBrowserItem.Text = "&Show Test Browser";
            this.testBrowserItem.Click += new System.EventHandler(this.ShowTestBrowser);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.menuItem5.ShowShortcut = false;
            this.menuItem5.Text = "E&xit";
            this.menuItem5.Click += new System.EventHandler(this.ExitClicked);
            // 
            // assemblyNameLabel
            // 
            this.assemblyNameLabel.Location = new System.Drawing.Point(15, 15);
            this.assemblyNameLabel.Name = "assemblyNameLabel";
            this.assemblyNameLabel.Size = new System.Drawing.Size(85, 23);
            this.assemblyNameLabel.TabIndex = 0;
            this.assemblyNameLabel.Text = "Assembly File";
            // 
            // panel1
            // 
            this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                 this.AssemblyFileNameText,
                                                                                 this.assemblyNameLabel,
                                                                                 this.BrowseAssemblyButton,
                                                                                 this.RunButton,
                                                                                 this.typeNameLabel,
                                                                                 this.TypeNameSelector,
                                                                                 this.runTestProgressBar});
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 121);
            this.panel1.TabIndex = 0;
            // 
            // AssemblyFileNameText
            // 
            this.AssemblyFileNameText.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right);
            this.AssemblyFileNameText.Location = new System.Drawing.Point(117, 11);
            this.AssemblyFileNameText.Name = "AssemblyFileNameText";
            this.AssemblyFileNameText.Size = new System.Drawing.Size(445, 20);
            this.AssemblyFileNameText.TabIndex = 1;
            this.AssemblyFileNameText.Text = "";
            this.AssemblyFileNameText.TextChanged += new System.EventHandler(this.AssemblyFileNameText_TextChanged);
            // 
            // RunButton
            // 
            this.RunButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.RunButton.Enabled = false;
            this.RunButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.RunButton.Location = new System.Drawing.Point(569, 45);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 28);
            this.RunButton.TabIndex = 4;
            this.RunButton.Text = "&Run";
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // runTestProgressBar
            // 
            this.runTestProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right);
            this.runTestProgressBar.CausesValidation = false;
            this.runTestProgressBar.Enabled = false;
            this.runTestProgressBar.ForeColor = System.Drawing.Color.Lime;
            this.runTestProgressBar.Location = new System.Drawing.Point(15, 86);
            this.runTestProgressBar.Maximum = 100;
            this.runTestProgressBar.Minimum = 0;
            this.runTestProgressBar.Name = "runTestProgressBar";
            this.runTestProgressBar.Size = new System.Drawing.Size(634, 23);
            this.runTestProgressBar.Step = 1;
            this.runTestProgressBar.TabIndex = 0;
            this.runTestProgressBar.TabStop = false;
            this.runTestProgressBar.Value = 0;
            // 
            // TestRunnerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(664, 430);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.OutputTab,
                                                                          this.statusBar,
                                                                          this.panel1});
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(370, 440);
            this.Name = "TestRunnerForm";
            this.Text = "Run Test Suite";
            this.TextChanged += new System.EventHandler(this.AssemblyFileNameText_TextChanged);
            ((System.ComponentModel.ISupportInitialize)(this.statusTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusFailCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusTestCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusRunCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusErrorCount)).EndInit();
            this.StdOutTabPage.ResumeLayout(false);
            this.OutputTab.ResumeLayout(false);
            this.TestErrorsTabPage.ResumeLayout(false);
            this.StdErrTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
	}
}