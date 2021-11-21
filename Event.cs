using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Xml.Linq;

namespace EventMonitor
{
    #test
    class Event
    {
        public static void OnEntryWritten(object source, EntryWrittenEventArgs e)
        {
            if (e.Entry.Source == "Microsoft-Windows-Winlogon")
            {
                if (e.Entry.EventID == 7001 || e.Entry.EventID == 7002)
                {
                    string eventMsg;
                    if (e.Entry.EventID == 7001)
                    {
                        eventMsg = GetUsername(e.Entry.Index) + " logon";
                        WrtieToLog(eventMsg, 1111, EventLogEntryType.Information);
                    }
                    if (e.Entry.EventID == 7002)
                    {
                        eventMsg = GetUsername(e.Entry.Index) + " logoff";
                        WrtieToLog(eventMsg, 1112, EventLogEntryType.Information);
                    }
                }
            }                    
        }
        private static void WrtieToLog(string eventMsg, int eventID, EventLogEntryType severety)
        {
            try
            {
                string sourceLog = "EventMonitor";
                EventLog systemEventLog = new EventLog("Application");
                if (!EventLog.SourceExists(sourceLog))
                {
                    EventLog.CreateEventSource(sourceLog, "Application");
                }
                systemEventLog.Source = sourceLog;
                systemEventLog.WriteEntry(eventMsg, severety, eventID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetUsername(int eventIndex)
        {
            string query = "*[System/EventRecordID=" + eventIndex + "]";
            EventLogQuery eventsQuery = new EventLogQuery("System", PathType.LogName, query);
            EventLogReader logReader = new EventLogReader(eventsQuery);
            EventRecord eventRecord = logReader.ReadEvent();
            XDocument xml = XDocument.Parse(eventRecord.ToXml());
            XNamespace ns = "http://schemas.microsoft.com/win/2004/08/events/event";
            string sid = xml.Descendants(ns + "Data").Last().Value;
            string account = new System.Security.Principal.SecurityIdentifier(sid).Translate(typeof(System.Security.Principal.NTAccount)).ToString();
            var shortAc = account.Split((@"\").ToCharArray());
            string userName = shortAc[1].ToLower();
            return userName;
        }
    }
}
