// NAnt - A .NET build tool
// Copyright (C) 2001 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Rob Jefferies (rob@houstonsigamchi.org)

using System;
using SourceSafeTypeLib;
using SourceForge.NAnt.Attributes;

namespace SourceForge.NAnt.Extras.SourceSafe {
    /// <summary>
    /// Used to add files to a Visual SourceSafe database.  If the file is currently
    /// in the SourceSafe database a message will be logged but files will continue to be added.
    /// <code>
    ///		<![CDATA[
    ///		<vssadd dbpath="C:\SourceSafeFolder\srcsafe.ini" user="user1" password="" path="$/Somefolder">
    ///			<fileset basedir="C:\SourceFolder\">	
    ///				<includes name="*.dll"/>
    ///			</fileset>
    ///		</vssadd>
    ///		]]>
    /// </code>
    /// This version does not support recursive adds.  Only adds in the root directory will be added to the
    /// SourceSafe database.
    /// </summary>
    [TaskName("vssadd")]
    public sealed class AddTask : BaseTask {

        string _comment = "";
        FileSet _fileset=new FileSet();

        /// <summary>
        /// Places a comment on all files added into the SourceSafe repository.  Not Required.
        /// </summary>
        [TaskAttribute("comment",Required=false)]
        public string Comment {
            get { return _comment;}
            set {_comment = value;}
        }

        /// <summary>
        /// List of files that should be added to SourceSafe.  Note: Recursive adds not supported.
        /// </summary>
        [FileSet("fileset")]
        public FileSet AddFileSet {
            get { return _fileset; }
        }

        protected override void ExecuteTask() {
            Open();
            
            const int FILE_ALREADY_ADDED = -2147166572;
          
            // Attempt to add each file to SourceSafe.  If file is already in SourceSafe
            // then log a message and continue otherwise throw an exception.
            foreach( string currentFile in _fileset.FileNames ) {
                try {
                    Item.Add(currentFile,_comment,0);
                    Log.WriteLine(LogPrefix + "Added File : " + currentFile);		
                } catch (System.Runtime.InteropServices.COMException e) {
                    if (e.ErrorCode  == FILE_ALREADY_ADDED) {
                        Log.WriteLine(LogPrefix + "File already added : " + currentFile);                                         
                        // just continue here 
                    } else {                                             
                        throw new BuildException("Adding files to SourceSafe failed.", Location, e);		                                             
                    }
                } 
                // All other exceptions
                catch (Exception e) {                                            
                  throw new BuildException("Adding files to SourceSafe failed.", Location, e);		                                                               
                }
            }
        }              
    }
}
