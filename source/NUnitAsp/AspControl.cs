/********************************************************************************************************************
'
' Copyright (c) 2002, Brian Knowles, Jim Little
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/

using System;
using NUnit.Framework;
using System.Xml;

namespace NUnit.Extensions.Asp
{

	public abstract class AspControl : Control
	{

		private string aspId;
		private Control container;

		internal AspControl(string aspId, Control container)
		{
			this.aspId = aspId;
			this.container = container;
		}

		protected AspControl()
		{
		}

		public bool Visible
		{
			get
			{
				return container.HasElement(aspId);
			}
		}

		protected string GetOptionalAttributeValue(string name)
		{
			return Element.GetAttribute(name);
		}

		protected string GetAttributeValue(string name) 
		{
			XmlAttribute attribute = Element.GetAttributeNode(name);
			string message = string.Format("Expected attribute '{0}' in {1}", name, HtmlIdAndDescription);
			Assertion.AssertNotNull(message, attribute);
			return attribute.Value;
		}

		internal override XmlElement GetElement(string aspId)
		{
			return container.GetElement(aspId);
		}

		public override bool HasElement(string aspId)
		{
			return container.HasElement(aspId);
		}

		internal virtual XmlElement Element
		{
			get 
			{
				return container.GetElement(aspId);
			}
		}

		public string HtmlIdAndDescription
		{
			get
			{
				return string.Format("{0} ({1})", HtmlId, Description);
			}
		}

		public override string Description 
		{
			get 
			{
				string controlType = this.GetType().Name;
				return string.Format("{0} '{1}' in {2}", controlType, aspId, container.Description);
			}
		}

		public string AspId
		{
			get
			{
				return aspId;
			}
		}

		public virtual string HtmlId
		{
			get
			{
				return GetOptionalAttributeValue("id");
			}
		}

		internal override void EnterInputValue(string name, string value)
		{
			container.EnterInputValue(name, value);
		}

		internal override void Submit()
		{
			container.Submit();
		}

	}

}
