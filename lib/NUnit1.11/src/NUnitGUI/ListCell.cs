namespace NUnit.GUI 
{
	using System;
	using NUnit.Framework;

	[Obsolete("Use TestDescriptor")]
	class ListCell 
	{
		private string fTestName;
		private Exception fTestException;
    
		public ListCell(string testName, Exception testException) 
		{
			this.fTestName = testName;
			this.fTestException = testException;
		}
    
		public override String ToString() 
		{
			String cell = fTestName;
			String message = fTestException.Message;
			if(message != null)
				return cell + " : " + message;
    
			return cell;
		}
    
		public Exception Exception 
		{
			get { return this.fTestException; }
		}
	}
}