
#define MyAppName "Another Music Player"
#define MyAppPublisher "LordKBX WorkShop"
#define MyAppURL "https://github.com/LordKBX/Another-Music-Player"
#define MyAppExeName "AnotherMusicPlayer"
#define RunTimeName "Install Runtime .NET CORE 3.1.3"

#ifndef MyInstallerVersion
#define MyInstallerVersion "1.0.0"
#endif

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{9F636870-C49A-477A-AF5B-C6D0383D7696}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyInstallerVersion}

;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
DisableWelcomePage=no
; The [Icons] "quicklaunchicon" entry uses {userappdata} but its [Tasks] entry has a proper IsAdminInstallMode Check.
;UsedUserAreasWarning=no
LicenseFile=D:\CODES\VS\MediaPlayer\LICENSE
; Uncomment the following line to run in non administrative install mode (install for current user only.)
PrivilegesRequired=admin
OutputDir=D:\CODES\VS\MediaPlayer\Installers
OutputBaseFilename={#MyAppExeName}-{#MyAppVersion}-{#BuildVersion}-ib({#MyInstallerVersion})
Compression=lzma                                              
SolidCompression=yes
;WizardStyle=modern                      

#include "C:\Program Files (x86)\Inno Download Plugin\idp.iss"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"   

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]
Source: ".\Release\{#BuildVersion}\AnotherMusicPlayer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\Release\{#BuildVersion}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "unzip.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonstartup}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]     
Filename: "{tmp}\setup-runtime.exe"; Parameters: "/install /quiet"; Flags: skipifdoesntexist shellexec waituntilterminated
Filename: "{tmp}\unzip.exe"; Parameters: "{tmp}\ffmpeg.zip -d {sd}\ProgramData\{#MyAppExeName}"
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]    
procedure InitializeWizard();
begin
  WizardForm.WelcomeLabel1.Visible := True;   
  WizardForm.WelcomeLabel2.Visible := True;

  if ('{#BuildVersion}' = 'X86') then
    if not DirExists(ExpandConstant('{commonpf32}\dotnet\shared\Microsoft.NETCore.App\3.1.3')) then 
      idpAddFileSize('http://sd-36502.dedibox.fr/AnotherMusicPlayer/windowsdesktop-runtime-3.1.3-win-x86.exe',expandconstant('{tmp}\setup-runtime.exe'), 48625808)
    ;
  ;     

  if ('{#BuildVersion}' = 'X64') then
    if not DirExists(ExpandConstant('{commonpf64}\dotnet\shared\Microsoft.NETCore.App\3.1.3')) then 
      idpAddFileSize('http://sd-36502.dedibox.fr/AnotherMusicPlayer/windowsdesktop-runtime-3.1.3-win-x64.exe',expandconstant('{tmp}\setup-runtime.exe'), 54449000)
    ;
  ;
  
  // find relasese at https://www.videohelp.com/software/ffmpeg/old-versions
  if not FileExists(ExpandConstant('{sd}\ProgramData\{#MyAppExeName}\ffmpeg.exe')) then 
    if ('{#BuildVersion}' = 'X86') then
      idpAddFileSize('http://sd-36502.dedibox.fr/AnotherMusicPlayer/ffmpeg-win32-static.zip',expandconstant('{tmp}\ffmpeg.zip'), 20997449)
    else
      idpAddFileSize('http://sd-36502.dedibox.fr/AnotherMusicPlayer/ffmpeg-win64-static.zip',expandconstant('{tmp}\ffmpeg.zip'), 39247024)
      ;
    ;
  idpDownloadAfter(wpReady);
end;   
     
procedure CurStepChanged(CurStep: TSetupStep);
begin
 if CurStep=ssInstall then begin //Lets install those files that were downloaded for us
  CreateDir(expandconstant('{sd}\ProgramData\{#MyAppExeName}'))
 end;
end;