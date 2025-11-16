; Inno Setup Script for Torrent Rationer
; Requires Inno Setup 6.0 or later (https://jrsoftware.org/isinfo.php)

#define MyAppName "Torrent Rationer"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Torrent Rationer Team"
#define MyAppExeName "TorrentRationer.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
AppId={{A7B3C4D5-E6F7-4890-A1B2-C3D4E5F67890}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=installer
OutputBaseFilename=TorrentRationer-Setup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
; Show info about VC++ Redistributable requirement
InfoBeforeFile=README.md

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "addtopath"; Description: "Add to system PATH"; GroupDescription: "Additional options:"; Flags: unchecked

[Files]
Source: "publish\win-x64\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\win-x64\TorrentRationer.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "SKIASHARP_FIX.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "TROUBLESHOOTING.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Uninstall {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{group}\README"; Filename: "{app}\README.md"
Name: "{group}\Troubleshooting Guide"; Filename: "{app}\TROUBLESHOOTING.md"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function VCRedistNeedsInstall: Boolean;
begin
  // Check if vcruntime140.dll exists in System32
  Result := not (FileExists(ExpandConstant('{sys}\vcruntime140.dll')) and 
                 FileExists(ExpandConstant('{sys}\msvcp140.dll')));
end;

function InitializeSetup: Boolean;
var
  ErrCode: Integer;
begin
  Result := True;
  
  // Check if VC++ Redistributable is installed
  if VCRedistNeedsInstall then
  begin
    if MsgBox('This application requires Microsoft Visual C++ Redistributable (x64).' + #13#10 + #13#10 +
              'The installer will now open a web page where you can download it.' + #13#10 +
              'After installing the redistributable, please run this installer again.' + #13#10 + #13#10 +
              'Would you like to open the download page now?', 
              mbConfirmation, MB_YESNO) = IDYES then
    begin
      ShellExec('open', 'https://aka.ms/vs/17/release/vc_redist.x64.exe', '', '', SW_SHOW, ewNoWait, ErrCode);
    end;
    
    MsgBox('Please install Visual C++ Redistributable and then run this installer again.' + #13#10 + #13#10 +
           'Download link: https://aka.ms/vs/17/release/vc_redist.x64.exe', 
           mbInformation, MB_OK);
    Result := False;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
    Path: string;
begin
    if CurStep = ssPostInstall then
    begin
        if WizardIsTaskSelected('addtopath') then
        begin
            if RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', Path) then
            begin
                if Pos(ExpandConstant('{app}'), Path) = 0 then
                begin
                    Path := Path + ';' + ExpandConstant('{app}');
                    RegWriteStringValue(HKEY_LOCAL_MACHINE, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', Path);
                end;
            end;
        end;
    end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
    Path: string;
    AppPath: string;
begin
    if CurUninstallStep = usPostUninstall then
    begin
        AppPath := ExpandConstant('{app}');
        if RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', Path) then
        begin
            if Pos(AppPath, Path) > 0 then
            begin
                StringChangeEx(Path, ';' + AppPath, '', True);
                StringChangeEx(Path, AppPath + ';', '', True);
                StringChangeEx(Path, AppPath, '', True);
                RegWriteStringValue(HKEY_LOCAL_MACHINE, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', Path);
            end;
        end;
    end;
end;


