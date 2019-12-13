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
