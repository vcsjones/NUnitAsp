#region Copyright (c) 2003 Brian Knowles, Jim Little
/********************************************************************************************************************
'
' Copyright (c) 2003, Brian Knowles, Jim Little
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
using System.DirectoryServices;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Threading;

using NUnit.Framework;
using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.AspTester;

namespace NUnit.Extensions.Asp.Test
{
	public class HttpClientCredentialsTest : NUnitAspTestCase
	{
		protected override void SetUp()
		{
			base.SetUp();
            SetFolderToNTLMAuthentication("Credentials");
		}

		public void TestNoCredentials()
		{
            string errorMessage = "Unauthorised Access status '401' was expected";
			try
			{
				Browser.GetPage(BaseUrl + "Credentials/CredentialsTest.aspx");
                Fail(errorMessage);
            }
			catch (HttpClient.BadStatusException e)
			{
				Assert(errorMessage, e.Message.IndexOf("(status code: 401)") > 0);
			}

		}

		public void TestCredentials()
		{
			LabelTester userId = new LabelTester("userId", CurrentWebForm);

            Browser.Credentials = CredentialCache.DefaultCredentials;
			Browser.GetPage(BaseUrl + "Credentials/CredentialsTest.aspx");

			Assertion.AssertEquals("userId", WindowsIdentity.GetCurrent().Name, userId.Text);
		}


		private void SetFolderToNTLMAuthentication(string folderName)
		{
			DirectoryEntry baseFolder = new DirectoryEntry("IIS://localhost/W3SVC/1/Root" + BasePath);

			DirectoryEntry targetFolder;
			string folderClass = "IIsWebDirectory";
			try
			{
				targetFolder = baseFolder.Children.Find(folderName, folderClass);
			}
			catch (DirectoryNotFoundException)
			{
				targetFolder = baseFolder.Children.Add(folderName, folderClass);
			}

			SetEntryProperty(targetFolder, "AuthAnonymous", false);
			SetEntryProperty(targetFolder, "AuthBasic", false);
			SetEntryProperty(targetFolder, "AuthNTLM", true);
            targetFolder.CommitChanges();
		}

		private void SetEntryProperty(DirectoryEntry entry, string propertyName, object newValue)
		{
			entry.Properties[propertyName][0] = newValue;
		}
	}
}
