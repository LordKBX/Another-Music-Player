; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Another Music Player"
#define MyAppVersion "0.1.2.1"
#define MyAppPublisher "LordKBX WorkShop"
#define MyAppURL "https://github.com/LordKBX/Another-Music-Player"
#define MyAppExeName "AnotherMusicPlayer.exe"
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
OutputBaseFilename=AnotherMusicPlayer-{#MyAppVersion}
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
Source: "C:\Users\KevBo\source\repos\MediaPlayer\Release\AnotherMusicPlayer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\KevBo\source\repos\MediaPlayer\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]     
Filename: "{tmp}\setup-runtime.exe"; Description: "Run installer .NET Core 3.1.3(do not skip if download additional elements page was show)"; Parameters: "/install /quiet"; Flags: skipifdoesntexist shellexec waituntilterminated
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

  if not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\\Classes\\Installer\\Dependencies\\dotnet_apphost_pack_24.76.28628_x64_x86') then
  begin
    itd_init;   
    { if IsWin64 then
      
      else      }
      itd_addfile('http://download.visualstudio.microsoft.com/download/pr/7cd5c874-5d11-4e72-81f0-4a005d956708/0eb310169770c893407169fc3abaac4f/windowsdesktop-runtime-3.1.3-win-x86.exe',expandconstant('{tmp}\setup-runtime.exe'))
    { ;      }

    //Start the download after the "Ready to install" screen is shown
    itd_downloadafter(wpReady);
  end;
end;   