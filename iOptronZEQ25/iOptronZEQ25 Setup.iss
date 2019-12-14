;
; Script generated by the ASCOM Driver Installer Script Generator 6.4.0.0
; Generated by Alan Duffy on 2019-12-13 (UTC)
;
[Setup]
AppID={{40f36496-6602-4769-a9cb-96b22c5b134a}
AppName=ASCOM iOptronZEQ25 Telescope Driver
AppVerName=ASCOM iOptronZEQ25 Telescope Driver 6.4.0
AppVersion=6.4.0
AppPublisher=Alan Duffy <InukPhysiker@gmail.com>
AppPublisherURL=mailto:InukPhysiker@gmail.com
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
VersionInfoVersion=1.0.0
MinVersion=0,6.0
DefaultDirName="{commoncf}\ASCOM\Telescope\iOptronZEQ25"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="iOptronZEQ25 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Installer Generator\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Installer Generator\Resources\CreativeCommons.txt"
; {commoncf}\ASCOM\Uninstall\Telescope folder created by Platform, always
UninstallFilesDir="{commoncf}\ASCOM\Uninstall\Telescope\iOptronZEQ25"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{commoncf}\ASCOM\Uninstall\Telescope\iOptronZEQ25"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[Files]
Source: "C:\GitHub\ZEQ25\iOptronZEQ25\bin\Release\ASCOM.iOptronZEQ25.Server.exe"; DestDir: "{app}"
Source: "C:\GitHub\ZEQ25\Telescope\bin\Release\ASCOM.iOptronZEQ25.Telescope.dll"; DestDir: "{app}"

; AfterInstall: RegASCOM()

; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "C:\GitHub\ZEQ25\iOptronZEQ25\ReadMe.htm"; DestDir: "{app}"; Flags: isreadme
; TODO: Add other files needed by your driver here (add subfolders above)

; Only if driver is .NET
[Run]
; Only for .NET assembly/in-proc drivers
Filename: "{dotnet4032}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.iOptronZEQ25.Telescope.dll"""; Flags: runhidden 32bit
Filename: "{dotnet4064}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.iOptronZEQ25.Telescope.dll"""; Flags: runhidden 64bit; Check: IsWin64


; Only if driver is .NET
[UninstallRun]
; Only for .NET assembly/in-proc drivers
Filename: "{dotnet4032}\regasm.exe"; Parameters: "-u ""{app}\ASCOM.iOptronZEQ25.Telescope.dll"""; Flags: runhidden 32bit
; This helps to give a clean uninstall
Filename: "{dotnet4064}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.iOptronZEQ25.Telescope.dll"""; Flags: runhidden 64bit; Check: IsWin64
Filename: "{dotnet4064}\regasm.exe"; Parameters: "-u ""{app}\ASCOM.iOptronZEQ25.Telescope.dll"""; Flags: runhidden 64bit; Check: IsWin64


;Only if COM Local Server
[Run]
Filename: "{app}\ASCOM.iOptronZEQ25.Server.exe"; Parameters: "/regserver"




;Only if COM Local Server
[UninstallRun]
Filename: "{app}\ASCOM.iOptronZEQ25.Server.exe"; Parameters: "/unregserver"



;  DCOM setup for COM local Server, needed for TheSky
[Registry]
; TODO: If needed set this value to the Telescope CLSID of your driver (mind the leading/extra '{')
#define AppClsid "{{35f11bab-d72a-475d-abe3-af2f5a4598c1}"

; set the DCOM access control for TheSky on the Telescope interface
Root: HKCR; Subkey: CLSID\{#AppClsid}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid}
Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: string; ValueData: "ASCOM iOptronZEQ25 Telescope Driver"
Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid}
Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: dword; ValueName: AuthenticationLevel; ValueData: 1
; set the DCOM key for the executable as a whole
Root: HKCR; Subkey: AppId\ASCOM.iOptronZEQ25.Server.exe; ValueType: string; ValueName: AppID; ValueData: {#AppClsid}
; CAUTION! DO NOT EDIT - DELETING ENTIRE APPID TREE WILL BREAK WINDOWS!
Root: HKCR; Subkey: AppId\{#AppClsid}; Flags: uninsdeletekey
Root: HKCR; Subkey: AppId\ASCOM.iOptronZEQ25.Server.exe; Flags: uninsdeletekey

[Code]
const
   REQUIRED_PLATFORM_VERSION = 6.2;    // Set this to the minimum required ASCOM Platform version for this application

//
// Function to return the ASCOM Platform's version number as a double.
//
function PlatformVersion(): Double;
var
   PlatVerString : String;
begin
   Result := 0.0;  // Initialise the return value in case we can't read the registry
   try
      if RegQueryStringValue(HKEY_LOCAL_MACHINE_32, 'Software\ASCOM','PlatformVersion', PlatVerString) then 
      begin // Successfully read the value from the registry
         Result := StrToFloat(PlatVerString); // Create a double from the X.Y Platform version string
      end;
   except                                                                   
      ShowExceptionMessage;
      Result:= -1.0; // Indicate in the return value that an exception was generated
   end;
end;

//
// Before the installer UI appears, verify that the required ASCOM Platform version is installed.
//
function InitializeSetup(): Boolean;
var
   PlatformVersionNumber : double;
 begin
   Result := FALSE;  // Assume failure
   PlatformVersionNumber := PlatformVersion(); // Get the installed Platform version as a double
   If PlatformVersionNumber >= REQUIRED_PLATFORM_VERSION then	// Check whether we have the minimum required Platform or newer
      Result := TRUE
   else
      if PlatformVersionNumber = 0.0 then
         MsgBox('No ASCOM Platform is installed. Please install Platform ' + Format('%3.1f', [REQUIRED_PLATFORM_VERSION]) + ' or later from http://www.ascom-standards.org', mbCriticalError, MB_OK)
      else 
         MsgBox('ASCOM Platform ' + Format('%3.1f', [REQUIRED_PLATFORM_VERSION]) + ' or later is required, but Platform '+ Format('%3.1f', [PlatformVersionNumber]) + ' is installed. Please install the latest Platform before continuing; you will find it at http://www.ascom-standards.org', mbCriticalError, MB_OK);
end;

// Code to enable the installer to uninstall previous versions of itself when a new version is installed
procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  UninstallExe: String;
  UninstallRegistry: String;
begin
  if (CurStep = ssInstall) then // Install step has started
	begin
      // Create the correct registry location name, which is based on the AppId
      UninstallRegistry := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#SetupSetting("AppId")}' + '_is1');
      // Check whether an extry exists
      if RegQueryStringValue(HKLM, UninstallRegistry, 'UninstallString', UninstallExe) then
        begin // Entry exists and previous version is installed so run its uninstaller quietly after informing the user
          MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
          Exec(RemoveQuotes(UninstallExe), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
          sleep(1000);    //Give enough time for the install screen to be repainted before continuing
        end
  end;
end;

//
// Register and unregister the driver with the Chooser
// We already know that the Helper is available
//
procedure RegASCOM();
var
   P: Variant;
begin
   P := CreateOleObject('ASCOM.Utilities.Profile');
   P.DeviceType := 'Telescope';
   P.Register('ASCOM.iOptronZEQ25.Telescope', 'iOptronZEQ25');
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
   P: Variant;
begin
   if CurUninstallStep = usUninstall then
   begin
     P := CreateOleObject('ASCOM.Utilities.Profile');
     P.DeviceType := 'Telescope';
     P.Unregister('ASCOM.iOptronZEQ25.Telescope');
  end;
end;


