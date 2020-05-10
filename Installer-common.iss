
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
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
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
Source: "C:\Users\KevBo\source\repos\MediaPlayer\Release\{#BuildVersion}\AnotherMusicPlayer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\KevBo\source\repos\MediaPlayer\Release\{#BuildVersion}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
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
procedure InitializeWizard();
begin
  { Welcome page }
  { Hide the labels }
  WizardForm.WelcomeLabel1.Visible := True;   
  WizardForm.WelcomeLabel2.Visible := True;
  { 
  WizardForm.WelcomeLabel2.Font.Color := clRed;   
  WizardForm.WelcomeLabel2.Font.Size := 10;
  WizardForm.WelcomeLabel2.Caption := 'This application require the framework ".NET CORE 3.1", Go to'
    + #13#10 + ' https://dotnet.microsoft.com/download/dotnet-core/3.1'
    + #13#10 + ' Section "Desktop Runtime"';
   }

  if not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\\Classes\\Installer\\Dependencies\\{#MyRegKey}') then
  begin
    itd_init;
    if ('{#BuildVersion}' = 'X86') then
      itd_addfile('http://download.visualstudio.microsoft.com/download/pr/7cd5c874-5d11-4e72-81f0-4a005d956708/0eb310169770c893407169fc3abaac4f/windowsdesktop-runtime-3.1.3-win-x86.exe',expandconstant('{tmp}\setup-runtime.exe'))
    else
      itd_addfile('http://download.visualstudio.microsoft.com/download/pr/5954c748-86a1-4823-9e7d-d35f6039317a/169e82cbf6fdeb678c5558c5d0a83834/windowsdesktop-runtime-3.1.3-win-x64.exe',expandconstant('{tmp}\setup-runtime.exe'))
    ;
    //Start the download after the "Ready to install" screen is shown
    itd_downloadafter(wpReady);
  end;
end;   