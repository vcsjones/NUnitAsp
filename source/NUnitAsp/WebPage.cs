#region Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2002, 2003, Brian Knowles, Jim Shore
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
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

using Sgml;

namespace NUnit.Extensions.Asp
{
	internal class WebPage
	{
		private string pageText;
		private XmlDocument document = null;
		public Hashtable formVariables = null;

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

		internal FormVariables VariablesFor(string formHtmlId)
		{
			if (formVariables == null) ParsePageText();
			FormVariables result = (FormVariables)formVariables[formHtmlId];
			WebAssert.NotNull(result, "form ID '" + formHtmlId + "' not found on current page");
			return result;
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
				reader.InputStream = new StringReader(FixHtmlToAvoidParseErrors(pageText));
				reader.Dtd = ParseDtd(reader.NameTable);
				reader.ErrorLog = Console.Error;
				reader.DocType = "HTML";

				document = new XhtmlDocument(reader.NameTable);
				try 
				{
					document.Load(reader);
				}
				catch (WebException e)
				{
					throw new DoctypeDtdException(e);
				}

				ParseForms();
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

		private string FixHtmlToAvoidParseErrors(string html)
		{
			return Regex.Replace(html, "<style>\\s+<!--", "<style><!--");
		}

		private void ParseForms()
		{
			formVariables = new Hashtable();
			foreach (XmlElement formElement in Document.SelectNodes("//form"))
			{
				ParseInitialFormValues(formElement);
			}
		}

		private void ParseInitialFormValues(XmlElement formElement) 
		{
			string id = formElement.GetAttribute("id");
			if (id == null) return;

			formVariables[id] = new FormVariables();
			ParseFormElementValues(id, "//input[@type='file']", "@name", "");
			ParseFormElementValues(id, "//input[@type='password']", "@name", "");
			ParseFormElementValues(id, "//input[@type='text']", "@name", "");
			ParseFormElementValues(id, "//input[@type='hidden']", "@name", "");
			ParseFormElementValues(id, "//input[@type='radio'][@checked]", "@name", "on");
			ParseFormElementValues(id, "//input[@type='checkbox'][@checked]", "@name", "on");
			ParseFormElementValues(id, "//textarea", "@name", null);
			ParseFormElementValues(id, "//select/option[@selected]", "../@name", null);
		}

		private void ParseFormElementValues(string formId, string elementExpr, string nameExpr, string defaultValue)
		{
			foreach (XmlElement element in Document.SelectNodes("//form[@id='" + formId + "']" + elementExpr)) 
			{
				XmlAttribute name = (XmlAttribute)element.SelectSingleNode(nameExpr);
				string value = element.GetAttribute("value");

				if (name == null) continue;
				if (value == null || value == "") 
				{
					if (defaultValue != null)
					{
						value = defaultValue;
					}
					else
					{
						// Last chance value for <option> and <textarea> element
						value = element.InnerText.Trim();
					}
				}
				VariablesFor(formId).Add(name.Value, value);
			}
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

	/// <summary>
	/// Problems with the DOCTYPE DTD; probably that it was incorrect.  Correct it.
	/// </summary>
	public class DoctypeDtdException : ApplicationException
	{
		internal DoctypeDtdException(WebException e) : base(GetMessage(e))
		{
		}

		private static string GetMessage(WebException e)
		{
			return "Problems with DOCTYPE DTD: <" + e.Message + ">.  Your DOCTYPE is probably " +
				"incorrect.  If you're not sure what the DOCTYPE should be, use <!DOCTYPE HTML " +
				"PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\" >, Visual Studio .NET's default.";
		}
	}
}
