using System;
using System.Xml;

namespace NUnit.Extensions.Asp
{

	public abstract class Control
	{

		public abstract bool HasChildElement(string aspId);
		internal abstract XmlElement GetChildElement(string aspId);
		internal abstract void EnterInputValue(string name, string value);
		internal abstract void Submit();
		public abstract string Description
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
