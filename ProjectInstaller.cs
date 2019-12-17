using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;

namespace EventMonitor
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        static void SetRecoveryOptions(string serviceName) // Add service recovery 
        {
            int exitCode;
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;                
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/60000", serviceName);
                process.Start();
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            if (exitCode != 0)
                throw new InvalidOperationException();
        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            SetRecoveryOptions("EventMonitor");
            new ServiceController(ServiceInstallerEventMonitor.ServiceName).Start();
        }
    }
}
