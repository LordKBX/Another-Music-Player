
#define MyAppName "Another Music Player"
#define MyAppPublisher "LordKBX WorkShop"
#define MyAppURL "https://github.com/LordKBX/Another-Music-Player"
#define MyAppExeName "AnotherMusicPlayer"
#define RunTimeName "Install Runtime .NET CORE 3.1.3"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{9F636870-C49A-477A-AF5B-C6D0383D7696}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
DisableWelcomePage=no
; The [Icons] "quicklaunchicon" entry uses {userappdata} but its [Tasks] entry has a proper IsAdminInstallMode Check.
UsedUserAreasWarning=no
LicenseFile=C:\Users\KevBo\source\repos\MediaPlayer\LICENSE
; Uncomment the following line to run in non administrative install mode (install for current user only.)
PrivilegesRequired=admin
OutputDir=C:\Users\KevBo\source\repos\MediaPlayer\Installers
OutputBaseFilename={#MyAppExeName}-{#MyAppVersion}-{#BuildVersion}
Compression=lzma                                              
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"   

#include ReadReg(HKEY_LOCAL_MACHINE,'Software\Sherlock Software\InnoTools\Downloader','ScriptPath','');

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]
Source: ".\Release\{#BuildVersion}\AnotherMusicPlayer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\Release\{#BuildVersion}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]     
Filename: "{tmp}\setup-runtime.exe"; Parameters: "/install /quiet"; Flags: skipifdoesntexist shellexec waituntilterminated
;Filename: "{tmp}\setup-runtime-x64.exe"; Parameters: "/install /quiet"; Flags: skipifdoesntexist shellexec waituntilterminated
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code] 
const
  SHCONTCH_NOPROGRESSBOX = 4;
  SHCONTCH_RESPONDYESTOALL = 16;

procedure UnZip(ZipPath, TargetPath: string); 
var
  Shell: Variant;
  ZipFile: Variant;
  TargetFolder: Variant;
begin
  Shell := CreateOleObject('Shell.Application');

  ZipFile := Shell.NameSpace(ZipPath);
  if VarIsClear(ZipFile) then
    RaiseException(Format('ZIP file "%s" does not exist or cannot be opened', [ZipPath]));

  TargetFolder := Shell.NameSpace(TargetPath);
  if VarIsClear(TargetFolder) then
    RaiseException(Format('Target path "%s" does not exist', [TargetPath]));

  TargetFolder.CopyHere(ZipFile.Items, SHCONTCH_NOPROGRESSBOX or SHCONTCH_RESPONDYESTOALL);
end;

procedure InitializeWizard();
begin
  WizardForm.WelcomeLabel1.Visible := True;   
  WizardForm.WelcomeLabel2.Visible := True;
  
  itd_init;

  if not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\\Classes\\Installer\\Dependencies\\{#MyRegKey}') then
  begin
    if ('{#BuildVersion}' = 'X86') then
      itd_addfile('http://download.visualstudio.microsoft.com/download/pr/7cd5c874-5d11-4e72-81f0-4a005d956708/0eb310169770c893407169fc3abaac4f/windowsdesktop-runtime-3.1.3-win-x86.exe',expandconstant('{tmp}\setup-runtime.exe'))
    else
      itd_addfile('http://download.visualstudio.microsoft.com/download/pr/5954c748-86a1-4823-9e7d-d35f6039317a/169e82cbf6fdeb678c5558c5d0a83834/windowsdesktop-runtime-3.1.3-win-x64.exe',expandconstant('{tmp}\setup-runtime.exe'))
    ;
  end;

  if ('{#BuildVersion}' = 'X86') then
    itd_addfile('http://sd-36502.dedibox.fr:9092/AnotherMusicPlayer/ffmpeg-win32-static.zip',expandconstant('{tmp}\ffmpeg-win32-static.zip'))
  else
    itd_addfile('http://sd-36502.dedibox.fr:9092/AnotherMusicPlayer/ffmpeg-win64-static.zip',expandconstant('{tmp}\ffmpeg-win64-static.zip'))
  ;
  //Start the download after the "Ready to install" screen is shown
  itd_downloadafter(wpReady);

end;   
     
procedure CurStepChanged(CurStep: TSetupStep);
begin
 if CurStep=ssInstall then begin //Lets install those files that were downloaded for us
  CreateDir(expandconstant('{sd}\Users\{username}\AppData\Local\{#MyAppExeName}'))

  if ('{#BuildVersion}' = 'X86') then 
    UnZip(expandconstant('{tmp}\ffmpeg-win32-static.zip'), expandconstant('{sd}\Users\{username}\AppData\Local\{#MyAppExeName}\'))
  else  
    UnZip(expandconstant('{tmp}\ffmpeg-win64-static.zip'), expandconstant('{sd}\Users\{username}\AppData\Local\{#MyAppExeName}\'))
  ;
  //UnZip(expandconstant('{tmp}\ffmpeg-static.zip'), expandconstant('{sd}\Users\{username}\AppData\Local\{#MyAppExeName}\'));
 end;
end;