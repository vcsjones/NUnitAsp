@echo off
set _defaultToolDir=c:\windows\Microsoft.NET\Framework\v1.0.3705
echo .

if not "%DotNetToolDir%"=="" goto :setPath
echo Could not find DotNetToolDir environment variable
echo .
echo set DotNetToolDir=%_defaultToolDir%
set DotNetToolDir=%_defaultToolDir%

:setPath
set _backupPath=%path%
set path=%path%;%FrameworkSDKDir%\bin

set _command=lib\nant\bin\nant %*
echo %_command%
echo .
call %_command%

set _command=
set path=%_backupPath%
set _backupPath=
set _defaultToolDir=