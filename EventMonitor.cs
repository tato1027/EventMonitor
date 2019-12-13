using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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
