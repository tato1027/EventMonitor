using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EventMonitor
{
    static class Program
    {
        static void Main()
        {
#if DEBUG   // Debug
            EventMonitor eventMonitor = new EventMonitor();
            eventMonitor.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else       // Release
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new EventMonitor()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
