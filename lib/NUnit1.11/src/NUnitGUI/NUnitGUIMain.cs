namespace NUnit.GUI
{
	using System;
	using System.Collections;
	using System.Windows.Forms;
	using System.Reflection;
	using NUnit.Framework;
	using NUnit.Runner;
  
	public class Top
	{
		public static void Main(string[] args) 
		{
			NUnit.GUI.TestRunnerForm mainForm = new NUnit.GUI.TestRunnerForm();
			if (args.Length > 0)
				mainForm.initialTestToRun = args[0];
			Application.Run(mainForm);
		}
	}
}