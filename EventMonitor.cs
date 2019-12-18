using System.Diagnostics;
using System.ServiceProcess;

namespace EventMonitor
{
    public partial class EventMonitor : ServiceBase
    {
        public EventMonitor()
        {
            InitializeComponent();
        }

        public void OnDebug()   // Start debug configuration
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            string eventProvider = "System";
            EventLog log = new EventLog(eventProvider);
            log.EntryWritten += new EntryWrittenEventHandler(Event.OnEntryWritten);
            log.EnableRaisingEvents = true;
        }

        protected override void OnStop()
        {
        }
    }
}
