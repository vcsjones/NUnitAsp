using System;
using System.Xml;

namespace NUnit.Extensions.Asp
{
	/// <summary>
	/// Base class for all NUnitAsp controls.  To create your own tester 
	/// classes, you should usually extend ControlTester instead.
	///
	/// Not intended for third-party use.  The API for this class will change 
	/// in future releases.  
	/// </summary>
	public abstract class Control
	{
		public abstract bool HasChildElement(string aspId);
		internal abstract XmlElement GetChildElement(string htmlId);
		internal abstract string GetChildElementHtmlId(string aspId);
		internal abstract void EnterInputValue(string name, string value);
		internal abstract void Submit();
		public abstract string Description
		{
			get;
		}
		internal abstract HttpClient Browser
		{
			get;
		}

		internal class AttributeMissingException : ApplicationException
		{
			internal AttributeMissingException(string name, string containerDescription) : base("Expected attribute '" + name + "' on " + containerDescription)
			{
			}

		}
	}
}
