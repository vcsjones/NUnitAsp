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
// Jason Reimer, Diversant Inc. (jason.reimer@diversant.net)

using System;
using SourceSafeTypeLib;
using SourceForge.NAnt.Attributes;

namespace SourceForge.NAnt.Extras.SourceSafe {
	/// <summary>
	/// Used to checkin files into Visual Source Safe.
	/// </summary>
	[TaskName("vsscheckin")]
	public sealed class CheckinTask : BaseTask {
		
		string _comment = "";
		string _localpath = "";
		string _recursive = Boolean.TrueString;
		string _writable = Boolean.FalseString;

		/// <summary>
		/// The comment for the new version.
		/// </summary>
		[TaskAttribute("comment")]
		public string Comment 
		{ 
			get { return _comment; }
			set { _comment = value; }
		}

		/// <summary>
		/// The absolute path to the local working directory. Required.
		/// </summary>
		[TaskAttribute("localpath", Required=true)]
		public string LocalPath 
		{ 
			get { return _localpath; }
			set { _localpath = value; }
		}

		/// <summary>
		/// Determines whether to perform a recursive checkin. 
		/// Default value is true when omitted.
		/// </summary>
		[TaskAttribute("recursive")]
		[BooleanValidator()]
		public string Recursive
		{ 
			get { return _recursive; }
			set { _recursive = value; }
		}

		/// <summary>
		/// Determines whether to leave the file(s) as writable. 
		/// Default value is false when omitted.
		/// </summary>
		[TaskAttribute("writable")]
		[BooleanValidator()]
		public string Writable
		{ 
			get { return _writable; }
			set { _writable = value; }
		}

		protected override void ExecuteTask() {
			Open();

			/* -- Allowed flag categories --
			 * RECURS, USERO, CMPMETHOD, TIMESTAMP, EOL, REPLACE, FORCE, 
			 * KEEPCHECK, DEL, and UPD 
			 */
			int flags = (Convert.ToBoolean(_recursive) ? Convert.ToInt32(RecursiveFlag) : 0) | 
						(Convert.ToBoolean(_writable) ? Convert.ToInt32(VSSFlags.VSSFLAG_USERRONO) : Convert.ToInt32(VSSFlags.VSSFLAG_USERROYES));

			try {
				Item.Checkin(_comment, _localpath, flags);
			}
			catch (Exception e) {
				throw new BuildException("check-in failed.", Location, e);
			}

			Log.WriteLine(LogPrefix + "Checked in " + Path);
		}

	}
}
