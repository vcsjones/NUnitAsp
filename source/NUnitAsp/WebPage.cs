#region Copyright (c) 2002, 2003, Brian Knowles, Jim Little
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, Brian Knowles, Jim Little
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
#endregion

using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Text.RegularExpressions;

using Sgml;

namespace NUnit.Extensions.Asp
{
	internal class WebPage
	{
		private string pageText;
		private XmlDocument document = null;
		private NameValueCollection formVariables = new NameValueCollection();

		internal WebPage(string htmlPage)
		{
			pageText = htmlPage;
		}

		internal XmlDocument Document
		{
			get
			{
				if (document == null) ParsePageText();
				return document;
			}
		}

		internal string FormVariables
		{
			get
			{
				string joiner = "";
				string result = "";
				foreach (string key in formVariables.Keys)
				{
					result += joiner + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(formVariables.Get(key));
					joiner = "&";
				}
				return result;
			}
		}

		private SgmlDtd ParseDtd(XmlNameTable nt)
		{
			string name = string.Format("{0}.{1}.Html.dtd",
				typeof(WebPage).Namespace, typeof(SgmlDtd).Namespace);

			Stream stream = typeof(SgmlDtd).Assembly.GetManifestResourceStream(name);
			StreamReader reader = new StreamReader(stream);
			return SgmlDtd.Parse(null, "HTML", null, reader, null, null, nt);
		}

		private void ParsePageText()
		{
			SgmlReader reader = new SgmlReader();
			try 
			{
				reader.InputStream = new StringReader(pageText);
				reader.Dtd = ParseDtd(reader.NameTable);
				reader.ErrorLog = Console.Error;
				reader.DocType = "HTML";

				document = new XhtmlDocument(reader.NameTable);
				document.Load(reader);
				ParseDefaultFormVariables();
			}
			catch (XmlException e)
			{
				Console.WriteLine("vvvvvv The following HTML could not be parsed by NUnitAsp vvvvvv");
				Console.WriteLine(pageText);
				Console.WriteLine("^^^^^^ The preceding HTML could not be parsed by NUnitAsp ^^^^^^");
				throw new ParseException("Could not parse HTML.  See standard out for the HTML and use a validator (such as the one at validator.w3.org) to troubleshoot.  Parser error was: " + e.Message);
			}
			finally
			{
				reader.Close();
			}
		}
		private void ParseDefaultFormVariables() 
		{
			formVariables = new NameValueCollection();
			ParseFormHiddenValues();
			ParseFormTextValues("//input[@type='file']");
			ParseFormTextValues("//input[@type='password']");
			ParseFormTextValues("//input[@type='text']");
			ParseFormTextValues("//input[@type='radio'][@checked]");
			ParseFormCheckBoxValues();
			ParseFormTextAreaValues();
			ParseFormSelectValues();
		}

		private void ParseFormHiddenValues() 
		{
			XmlNodeList nodes = Document.SelectNodes("//input[@type='hidden']");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				XmlAttribute name = item.Attributes["name"];
				XmlAttribute aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) 
				{
					SetFormVariable(name.Value, aValue.Value);
				}
			}
		}

		private void ParseFormTextValues(string expression) 
		{
			XmlAttribute name;
			XmlAttribute aValue;
			XmlNodeList nodes = Document.SelectNodes(expression);
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				aValue = item.Attributes["value"];
				if ((name != null) && (aValue != null)) 
				{
					SetFormVariable(name.Value, aValue.Value);
				}
			}
		}

		private void ParseFormCheckBoxValues() 
		{
			XmlAttribute name;
			XmlNodeList nodes = Document.SelectNodes("//input[@type='checkbox'][@checked]");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				if (name != null) 
				{
					SetFormVariable(name.Value, "checked");
				}
			}
		}

		private void ParseFormTextAreaValues() 
		{
			XmlAttribute name;
			XmlNodeList nodes = Document.SelectNodes("//textarea");
			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				if (name != null) 
				{
					SetFormVariable(name.Value, item.InnerText.Trim());
				}
			}
		}

		private void ParseFormSelectValues() 
		{
			string expression = "//select";
			XmlAttribute name;
			XmlNode aValue = null;
			XmlNodeList nodes = Document.SelectNodes(expression);

			if (nodes == null) return;

			foreach (XmlNode item in nodes) 
			{
				name = item.Attributes["name"];
				if (name == null) 
				{
					throw new XmlException("A select form element on the page does not have a name.", null);
				}

				// Look for the option that is selected
				foreach (XmlNode child in item.ChildNodes) 
				{
					if (child.Attributes["selected"] != null) 
					{
						aValue = child.Attributes["value"];
					}
				}

				// If there is no value then we will just set it as empty
				if (aValue == null) 
				{
					SetFormVariable(name.Value, String.Empty);
				}
				else 
				{
					SetFormVariable(name.Value, aValue.Value);
				}
			}
		}	

		public void SetFormVariable(string name, string value) 
		{
			formVariables.Remove(name);
			formVariables.Add(name, value);
		}	

		public void ClearFormVariable(string name)
		{
			formVariables.Remove(name);
		}

		public override string ToString()
		{
			return pageText;
		}


		private class XhtmlDocument : XmlDocument
		{
			private readonly Hashtable byHtmlId = new Hashtable();

			public XhtmlDocument(XmlNameTable nt) : base(nt)
			{
			}

			public override void Load(XmlReader reader)
			{
				XmlNodeChangedEventHandler insertHandler = 
					new XmlNodeChangedEventHandler(XhtmlDocument_NodeInserted);

				byHtmlId.Clear();
				NodeInserted += insertHandler;
				try
				{
					base.Load(reader);
				}
				finally
				{
					NodeInserted -= insertHandler;
				}
			}

			public override XmlElement GetElementById(string htmlId)
			{
				return (XmlElement)byHtmlId[htmlId];
			}

			private void XhtmlDocument_NodeInserted(object sender, XmlNodeChangedEventArgs e)
			{
				if (e.Node.NodeType != XmlNodeType.Element) return;

				XmlAttribute id = e.Node.Attributes["id"];
				if (id != null)
				{
					byHtmlId[id.Value] = e.Node;
				}
			}
		}
	}
}
