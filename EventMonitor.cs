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
            /////////////////////////////////////////////////// Debug
            //string query = "*[System/EventRecordID=5385]";
            //EventLogQuery eventsQuery = new EventLogQuery("System", PathType.LogName, query);

            //try
            //{
            //    EventLogReader logReader = new EventLogReader(eventsQuery);
            //    EventRecord eventRecord = logReader.ReadEvent();
            //    XDocument xml = XDocument.Parse(eventRecord.ToXml());
            //    XNamespace ns = "http://schemas.microsoft.com/win/2004/08/events/event";
            //    string test = xml.Descendants(ns + "Data").Last().Value;
            //    Console.WriteLine(test);
            //}
            //catch (EventLogNotFoundException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return;
            //}
            ///////////////////////////////////////////////
            
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
