#region Copyright (c) 2005, Brian Knowles, Jim Shore
/********************************************************************************************************************
'
' Copyright (c) 2005, Brian Knowles, Jim Shore
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
using System.Collections;
using System.Web;

namespace NUnit.Extensions.Asp
{
	public class FormVariables
	{
		private Hashtable variables = new Hashtable();
		private ArrayList newVars = new ArrayList();
		
		public void Add(string name, string value)
		{
			Add(null, name, value);
		}

		public void Add(object owner, string name, string value)
		{
			newVars.Add(new DictionaryEntry(new FormKey(owner, name), value));
		}

		public void Remove(string name, string value)
		{
			Remove(null, name, value);
		}

		public void Remove(object owner, string name, string value)
		{
			for (int i = 0; i < newVars.Count; i++)
			{
				DictionaryEntry entry = (DictionaryEntry)newVars[i];
			
				if (entry.Key.Equals(new FormKey(owner, name)) && entry.Value.Equals(value))
				{
					newVars.RemoveAt(i);
					return;
				}
			}
			WebAssert.Fail(String.Format("Couldn't find form variable '{0}={1}' to remove", name, value));
		}

		public void RemoveAll(string name)
		{
			RemoveAll(null, name);
		}

		public void RemoveAll(object owner, string name)
		{
			for (int i = 0; i < newVars.Count; i++)
			{
				DictionaryEntry entry = (DictionaryEntry)newVars[i];
			
				if (entry.Key.Equals(new FormKey(owner, name)))
				{
					newVars.RemoveAt(i);
				}
			}
		}

		public void ReplaceAll(string name, string newValue)
		{
			ReplaceAll(null, name, newValue);
		}

		public void ReplaceAll(object owner, string name, string newValue)
		{
			RemoveAll(owner, name);
			Add(owner, name, newValue);
		}

		public override string ToString()
		{
			string result = "";
			string joiner = "";

			foreach(DictionaryEntry entry in newVars)
			{
				result += String.Format("{0}{1}={2}",
					joiner,
					HttpUtility.UrlEncode(((FormKey)entry.Key).Name),
					HttpUtility.UrlEncode((string)entry.Value));
				joiner = "&";
			}
			return result;
		}

		public struct FormKey
		{
			public readonly object Owner;
			public readonly string Name;

			public FormKey(object owner, string name)
			{
				Owner = owner;
				Name = name;
			}
		}
	}
}
