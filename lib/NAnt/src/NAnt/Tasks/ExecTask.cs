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
// Gerry Shaw (gerry_shaw@yahoo.com)

namespace SourceForge.NAnt.Tasks {

    using System;
    using System.IO;
    using SourceForge.NAnt.Attributes;

    /// <summary>Executes a system command.</summary>
    /// <example>
    ///   <para>Ping nant.sourceforge.net.</para>
    ///   <code><![CDATA[<exec program="ping" commandline="nant.sourceforge.net"/>]]></code>
    /// </example>
    [TaskName("exec")]
    public class ExecTask : ExternalProgramBase {
        
        string _program = null;        
        string _commandline = null;        
        string _baseDirectory = null;       
        int _timeout = Int32.MaxValue;
       
         /// <summary>The program to execute without command arguments.</summary>
        [TaskAttribute("program", Required=true)]
        public string FileName  { set { _program = value; } }                
                
        /// <summary>The command line arguments for the program.</summary>
        [TaskAttribute("commandline")]public string Arguments { set { _commandline = value; } }

        public override string ProgramFileName  { get { return _program; } }                
        public override string ProgramArguments { get { return _commandline; } }
        
        /// <summary>The directory in which the command will be executed.</summary>
        [TaskAttribute("basedir")]
        public override string BaseDirectory    { get { return Project.GetFullPath(_baseDirectory); } set { _baseDirectory = value; } }
                
        /// <summary>Stop the build if the command does not finish within the specified time.  Specified in milliseconds.  Default is no time out.</summary>
        [TaskAttribute("timeout")]
        [Int32Validator()]
        public override int TimeOut { get { return _timeout; } set { _timeout = value; }  }
        
        protected override void ExecuteTask() {
            Log.WriteLine(LogPrefix + "{0} {1}", ProgramFileName, GetCommandLine());
            base.ExecuteTask();
        }
    }
}
