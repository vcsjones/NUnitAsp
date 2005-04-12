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
	/// <summary>
	/// A collection of HTML form variables.  These form variables are sent to the server
	/// when you submit or postback a form.  They consist of a name/value pair and are not
	/// required to be unique.  You may include duplicate names or even duplicate name/value
	/// pairs in this collection.  When writing custom testers, <see cref="ReplaceAll"/>
	/// and <see cref="RemoveAll"/> are typically the most appropriate methods to use.
	/// </summary>
	public class FormVariables
	{
		private ArrayList variables = new ArrayList();
		
		/// <summary>
		/// Add a name/value pair.  Add doesn't overwrite any existing pairs with the
		/// same name, so in most cases, <see cref="ReplaceAll"/> is more appropriate.
		/// </summary>
		public void Add(string name, string value)
		{
			variables.Add(new DictionaryEntry(name, value));
		}

		/// <summary>
		/// Remove a specific name/value pair.  If the pair is in the collection more
		/// than once, this method will only remove one of them.
		/// </summary>
		public void Remove(string name, string value)
		{
			int i = IndexOf(name, value);
			WebAssert.True(i != -1, String.Format("Couldn't find form variable '{0}={1}' to remove in {2}", name, value, ToString()));

			variables.RemoveAt(i);
		}

		/// <summary>
		/// Remove all form variables with a particular name.
		/// </summary>
		public void RemoveAll(string name)
		{
			ArrayList indexes = AllIndexesOf(name);
			for (int i = 0; i < indexes.Count; i++)
			{
				int index = (int)indexes[i];
				variables.RemoveAt(index - i);
				// we subtract i because the variables array gets one smaller on each iteration
			}
		}

		/// <summary>
		/// Replace a specific name/value pair.
		/// </summary>
		public void Replace(string name, string oldValue, string newValue)
		{
			Remove(name, oldValue);
			Add(name, newValue);
		}

		/// <summary>
		/// Replace all form variables with a particular name.  This is the same
		/// as calling <see cref="RemoveAll"/> and then <see cref="Add"/>.
		/// </summary>
		public void ReplaceAll(string name, string newValue)
		{
			RemoveAll(name);
			Add(name, newValue);
		}

		/// <summary>
		/// Check if a name/value pair is in the collection.  It could exist
		/// more than once.
		/// </summary>
		public bool Contains(string name, string value)
		{
			return (IndexOf(name, value) != -1);
		}

		/// <summary>
		/// Check if a particular name has any values.  There could be more
		/// than one name/value pair with the specified name.
		/// </summary>
		public bool ContainsAny(string name)
		{
			return AllIndexesOf(name).Count > 0;
		}

		/// <summary>
		/// Returns the value of the name/value pair with the specified name.
		/// Throws an exception if there aren't any pairs with that name or
		/// if there's more than one.  Use <see cref="AllValuesOf"/> to handle
		/// cases where there isn't exactly one name/value pair with the
		/// requested name.
		/// </summary>
		public string ValueOf(string name)
		{
			string[] result = AllValuesOf(name);
			WebAssert.AreEqual(1, result.Length, "number of '" + name + "' variables");
			return result[0];
		}

		/// <summary>
		/// Returns all the values associated with the specified name.  Returns
		/// an empty (zero-length) array if there aren't any.  <see cref="ValueOf"/>
		/// is more convenient when there's exactly one name/value pair with the
		/// specified name.
		/// </summary>
		public string[] AllValuesOf(string name)
		{
			ArrayList indexes = AllIndexesOf(name);
			string[] result = new string[indexes.Count];
			for (int i = 0; i < indexes.Count; i++)
			{
				int index = (int)indexes[i];
				DictionaryEntry entry = (DictionaryEntry)variables[index];
				result[i] = (string)entry.Value;
			}
			return result;
		}

		/// <summary>
		/// Serializes all the name/value pairs into a string.  The format of the
		/// string matches the HTTP protocol specification for transmitting form
		/// variables to a server.  This can be handy for checking the form variables
		/// in your web page.  Generally you won't be sending it to a server because
		/// <see cref="HttpClient"/> handles that for you.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string result = "";
			string joiner = "";

			foreach(DictionaryEntry entry in variables)
			{
				result += String.Format("{0}{1}={2}",
					joiner,
					HttpUtility.UrlEncode((string)entry.Key),
					HttpUtility.UrlEncode((string)entry.Value));
				joiner = "&";
			}
			return result;
		}

		private int IndexOf(string name, string value)
		{
			for (int i = 0; i < variables.Count; i++)
			{
				DictionaryEntry entry = (DictionaryEntry)variables[i];
				if (entry.Key.Equals(name) && entry.Value.Equals(value))
				{
					return i;
				}
			}
			return -1;
		}

		private ArrayList AllIndexesOf(string name)
		{
			ArrayList result = new ArrayList();
			for (int i = 0; i < variables.Count; i++)
			{
				DictionaryEntry entry = (DictionaryEntry)variables[i];
				if (entry.Key.Equals(name))
				{
					result.Add(i);
				}
			}
			return result;
		}
	}
}
