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
	/// Used to apply a label to a Visual Source Safe item.
	/// </summary>
	[TaskName("vsslabel")]
	public sealed class LabelTask : BaseTask {
		
		string _comment = "";
		string _label = "";

		/// <summary>
		/// The label comment.
		/// </summary>
		[TaskAttribute("comment")]
		public string Comment 
		{
			get { return _comment; }
			set { _comment = value; }
		}
		
		/// <summary>
		/// The value of the label. Required.
		/// </summary>
		[TaskAttribute("label", Required=true)]
		public string Label 
		{
			get { return _label; }
			set { _label = value; }
		}
		
		protected override void ExecuteTask() {
			Open();

			try {
				Item.Label(_label, _comment);
			}
			catch (Exception e) {
				throw new BuildException("label failed", Location, e);
			}

			Log.WriteLine(LogPrefix + "Applied label \"" + _label + "\" to " + Path);
		}
	}
}
