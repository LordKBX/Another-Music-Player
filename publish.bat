@echo off
dotnet publish D:\CODES\VS\MediaPlayer -p:PublishProfile=Win-X64
dotnet publish D:\CODES\VS\MediaPlayer -p:PublishProfile=Win-X86
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "D:\CODES\VS\MediaPlayer\Installer-x64.iss"
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "D:\CODES\VS\MediaPlayer\Installer-x86.iss"
pause