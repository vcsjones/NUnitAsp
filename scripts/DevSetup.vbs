' DevSetup.vbs
' Sets up the required virtual directories to run NUnitAsp on IIS.
' Inspired by: Peter A. Bromberg, Ph.D.'s article at http://www.eggheadcafe.com/articles/20010609.asp
'
' To avoid annoying popups and have echo's sent to the console, run with cscript.exe.
' Better yet, run c:\Inetpub\AdminScripts\adsutil.vbs once to set your default script handler to cscript.exe

Option Explicit
Const NUNIT_ASP = "NUnitAsp"
Const SCRIPTS_DIR = "scripts"
Const ASPNET_USER = "ASPNET"
Const ANON_USER_PERMISSION = "R"
Const ASPNET_USER_PERMISSION = "F"

' Set some variables and constants we will use...
Dim strNUnitAspServerPath ' Path to NUnitAsp virtual dir
Dim strServerRoot ' Path to web server
Dim oArgs, ArgNum

Set oArgs = WScript.Arguments
ArgNum = 0
While ArgNum < oArgs.Count

	Select Case LCase(oArgs(ArgNum))
		Case "--website","-w":
			ArgNum = ArgNum + 1
			strServerRoot = oArgs(ArgNum)
		Case "--help","-?","-h"
			Call DisplayUsage
	End Select	

	ArgNum = ArgNum + 1
Wend

' Default to the default web site on localhost.
If strServerRoot = "" Then strServerRoot = "IIS://localhost/W3SVC/1/Root" 

' Build the NUnitAsp server path
strNUnitAspServerPath = strServerRoot & "/" & NUNIT_ASP

' Setup base NUnitAsp vir dir and sub vir dir's.
call setupVirtualDirAndFilePermissions(strServerRoot, NUNIT_ASP, "web")
call setupVirtualDirAndFilePermissions(strNUnitAspServerPath, "VBSamplePages", "sample\VBSamplePages")
call setupVirtualDirAndFilePermissions(strNUnitAspServerPath, "NUnitAspTestPages", "source\NUnitAspTestPages")


Sub setupVirtualDirAndFilePermissions(strServerPath, strVirtualDirName, strVirtualDirPath)
	Dim strFileSystemPath
	Dim strNUnitAspHomeDir
	
	strNUnitAspHomeDir = Left(WScript.ScriptFullName, InStr(WScript.ScriptFullName, SCRIPTS_DIR)-1)
	strFileSystemPath = strNUnitAspHomeDir & strVirtualDirPath
	
	call createApplication(strServerPath, strVirtualDirName, strFileSystemPath)
	call setDirectoryPermissions(strFileSystemPath, getIISIUSRName(), ANON_USER_PERMISSION)
	call setDirectoryPermissions(strFileSystemPath, ASPNET_USER, ASPNET_USER_PERMISSION)
End Sub

Sub createApplication(strSitePath, strVirtualDirectoryName, strVirtualDirectoryPath)
	Dim	objIIS 'ADSI IIS Object
	Dim objVirtualDirectory 'ADSI IIS Virtual Directory Object
	
	Dim strRes
	
	Wscript.echo(vbCRlf & "Adding virtual directory '" & strVirtualDirectoryName & "'" & vbCRlf & " to the '" & strSitePath & "' server." & vbCRlf )

	' Does this IIS application already exist in the metabase?
	On Error Resume Next
	Set objIIS = GetObject(strSitePath & "/" & strVirtualDirectoryName)

	If Err.Number = 0 Then
		Wscript.echo(strSitePath & "/" & strVirtualDirectoryName & vbCRlf)
		Wscript.echo ("An application with this name already exists. " & vbCRlf)
		Exit Sub
	End If
	Set objIIS = Nothing
	On Error GoTo 0

	'Now use IIS administration objects to create the IIS application in the metabase. 
	'Create the IIS application
	Set objIIS = GetObject(strSitePath)

	'Using IIS Administration object , turn on script/execute permissions and define the virtual directory as an 'in-process application. 
	Set objVirtualDirectory = objIIS.Create("IISWebVirtualDir", strVirtualDirectoryName)
	objVirtualDirectory.Path = strVirtualDirectoryPath
	objVirtualDirectory.AccessScript = True
	objVirtualDirectory.AccessWrite = False 
	objVirtualDirectory.AccessRead = True
	objVirtualDirectory.AccessExecute = False
	objVirtualDirectory.EnableDirBrowsing = True
	objVirtualDirectory.AuthAnonymous =True
	objVirtualDirectory.AnonymousUserName=getIISIUSRName()
	objVirtualDirectory.AnonymousPasswordSync=True
	objVirtualDirectory.AppFriendlyName=strVirtualDirectoryName
	objVirtualDirectory.AppCreate2(2) ' sets application protection to pooled
	objVirtualDirectory.SetInfo 
	
	strRes = "Web Application Created Sucessfully" & vbCRlf
	strRes = strRes & "Mapped virtual directory: " & strSitePath & "/" & strVirtualDirectoryName & vbCRlf
	strRes = strRes & "to path: " & strVirtualDirectoryPath & vbCRlf
	wscript.echo strRes
End Sub

Sub setDirectoryPermissions(strPath, strUserName, strPermission)
	' strPermission values
	'N  None
	'R  Read
	'W  Write
	'C  Change (write)
	'F  Full control
	
	Dim objWSH 'Windows Scripting Host Object
	Dim strACLCommand 'Command Line string to set ACLs
	Dim objRTC 'Return

	'Set Change Permissions for the owner using CACLS.exe
	' need to "|" pipe the "Y" yes answer to the command "Are you sure?" prompt for this to work (see KB: Q135268 )
	WScript.Echo "Adding '" & strPermission & "' permission for " & strUserName & " to " & strPath
	strACLCommand = "cmd /c echo y| CACLS " 
	strACLCommand = strACLCommand & strPath
	strACLCommand = strACLCommand & " /E /T /g " & strUserName & ":" & strPermission ' /T propagates to children, /E edits instead of replaces.

	Set objWSH = CreateObject("WScript.Shell")
	objRTC = objWSH.Run (strACLCommand , 0, True)
	Set objWSH = Nothing
	
	If objRTC > 0 Then 
		WScript.Echo "ERROR: Permission not added. CODE: " & objRTC & vbCRlf
	Else
		WScript.Echo "Permission added." & vbCRlf
	End If
End Sub

Sub DisplayUsage()
	WScript.Echo "Usage: DevSetup [--website|-w WEBSITE1]"
	WScript.Echo "                [--help|-?|-h]"	

	WScript.Echo ""
	WScript.Echo "The web site defaults to 'IIS://localhost/W3SVC/1/Root' if one is not provided."
	WScript.Echo ""
	WScript.Echo "Example : DevSetup -w IIS://localhost/W3SVC/2/Root"
	
	WScript.Quit
End Sub

Function getIISIUSRName()
	Dim MachineName ' computer name
	Dim Network
	
	' Get the Computer name using Wscript.Network and assign to IUSR to create IIS IUSR account name
	Set Network = WScript.CreateObject("WScript.Network")
	MachineName=Network.ComputerName
	getIISIUSRName = "IUSR_" & MachineName
	Set Network = Nothing
End Function
