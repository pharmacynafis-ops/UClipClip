[Setup]
AppName=UClipClip
AppVersion=1.0
DefaultDirName={pf}\UClipClip
DefaultGroupName=UClipClip
UninstallDisplayIcon={app}\UClipClip.exe
OutputDir=.
OutputBaseFilename=UClipClip_Setup
LicenseFile=LICENSE.txt

[Files]
Source: "bin\Release\net9.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion


[Icons]
Name: "{group}\UClipClip"; Filename: "{app}\UClipClip.exe"
Name: "{userstartup}\UClipClip"; Filename: "{app}\UClipClip.exe"

[Run]
Filename: "{app}\UClipClip.exe"; Description: "Launch UClipClip"; Flags: postinstall nowait skipifsilent