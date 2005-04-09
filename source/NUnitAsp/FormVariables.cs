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
	public class FormVariables : Hashtable
	{
//		public override string ToString()
//		{
//			string result = "";
//			string joiner = "";
//			foreach (DictionaryEntry entry in this)
//			{
////				FormKey key = (FormKey)entry.Key;
////				result += String.Format("{0}{1}={2}",
////					joiner,
////					HttpUtility.UrlEncode((string)key.Name),
////					HttpUtility.UrlEncode((string)entry.Value));
////				joiner = "&";
//				result += String.Format("{0}{1}={2}",
//					joiner,
//					HttpUtility.UrlEncode((string)entry.Key),
//					HttpUtility.UrlEncode((string)entry.Value));
//				joiner = "&";
//			}
//			return result;
//		}
//
//		//delete me
//		public void Dump()
//		{
//			Console.WriteLine();
//			foreach (DictionaryEntry entry in this)
//			{
////				FormKey key = (FormKey)entry.Key;
//				Console.WriteLine("{0}: {1}", entry.Key, entry.Value);
//			}
//		}
//
//		public void SetFormVariable(object owner, string name, string value) 
//		{
////			if (owner == null) throw new ArgumentNullException("owner");
////			this[new FormKey(owner, name)] = value;
//			this[name] = value;
//Dump();
//Console.WriteLine("added {0}: {1}", name, value);
//		}	
//
//		public void ClearFormVariable(object owner, string name)
//		{
////			if (owner == null) throw new ArgumentNullException("owner");
////			this.Remove(new FormKey(owner, name));
//			this.Remove(name);
//Dump();
//Console.WriteLine("removed " + name);
//		}

		
		public void SetFormVariable(object owner, string name, string value) 
		{
			if (owner == null) throw new ArgumentNullException("owner");
			this[new FormKey(owner, name)] = value;
		}	

		public void ClearFormVariable(object owner, string name)
		{
			if (owner == null) throw new ArgumentNullException("owner");
			this.Remove(new FormKey(owner, name));
		}

		public override string ToString()
		{
			string result = "";
			string joiner = "";
			foreach (DictionaryEntry entry in this)
			{
				FormKey key = (FormKey)entry.Key;
				result += String.Format("{0}{1}={2}",
					joiner,
					HttpUtility.UrlEncode((string)key.Name),
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
