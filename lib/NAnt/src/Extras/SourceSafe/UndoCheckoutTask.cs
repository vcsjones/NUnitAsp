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
// Robert Jefferies (robert.jefferies@usbank.com)

using System;
using SourceSafeTypeLib;
using SourceForge.NAnt.Attributes;

namespace SourceForge.NAnt.Extras.SourceSafe {
    /// <summary>
    /// Task is used to undo a checkout from SourceSafe
    /// <code>
    ///		<![CDATA[
    ///		<vssundocheckout dbpath="C:\DBlocation\srcsafe.ini" user="id" password="mypassword" path="$/ProjectName" localpath="c:\ProjectName" recursive="true" />
    ///		]]>
    /// </code>
    /// </summary>
    [TaskName("vssundocheckout")]
    public sealed class UndoCheckoutTask : BaseTask {
        
        string _recursive = Boolean.TrueString;
        string _localpath = ""; 

        /// <summary>
        /// The absolute path to the local working directory. This is required if you wish to 
        /// have your local file replaced with the latest version from SourceSafe.
        /// </summary>
        [TaskAttribute("localpath", Required=false)]
        public string LocalPath {
            get { return _localpath; }
            set { _localpath = value; }
        }

        /// <summary>
        /// Determines whether to perform a recursive UndoCheckOut. 
        /// Default value is true when omitted.
        /// </summary>
        [TaskAttribute("recursive")]
        [BooleanValidator]
        public string Recursive {
            get { return _recursive; }
            set { _recursive = value; }
        }

        protected override void ExecuteTask() {
            Open();
            
            int flags = (Convert.ToBoolean(_recursive) ? Convert.ToInt32(RecursiveFlag) : 0);

            try {
                Log.WriteLineIf(base.Verbose,LogPrefix + "localpath : " + _localpath);
                Item.UndoCheckout(_localpath,flags);
            }
            catch (Exception e) {
                throw new BuildException("UndoCheckout failed", Location, e);
            }

            Log.WriteLine(LogPrefix + "UndoCheckOut " + Path);
        }		
    }
}
