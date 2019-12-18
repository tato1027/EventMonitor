using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Xml.Linq;

namespace EventMonitor
{
    class Event
    {
        public static void OnEntryWritten(object source, EntryWrittenEventArgs e)
        {
            try
            {
                string logName = "System";
                int lastEntry = 0;
                EventLog log = new EventLog(logName);
                lastEntry = log.Entries.Count - 1;
                int eventID = log.Entries[lastEntry].EventID;
                string eventSource = log.Entries[lastEntry].Source;
                if ((eventID == 7001 | eventID == 7002) & eventSource == "Winlogon") 
                {
                    int eventRecordID = log.Entries[lastEntry].Index;
                    string query = "*[System/EventRecordID=" + eventRecordID + "]";
                    EventLogQuery eventsQuery = new EventLogQuery("System", PathType.LogName, query);
                    try
                    {
                        EventLogReader logReader = new EventLogReader(eventsQuery);
                        EventRecord eventRecord = logReader.ReadEvent();
                        XDocument xml = XDocument.Parse(eventRecord.ToXml());
                        XNamespace ns = "http://schemas.microsoft.com/win/2004/08/events/event";
                        string sid = xml.Descendants(ns + "Data").Last().Value;
                        string account = new System.Security.Principal.SecurityIdentifier(sid).Translate(typeof(System.Security.Principal.NTAccount)).ToString();
                        var shortAc = account.Split((@"\").ToCharArray());
                        string eventMsg = "";
                        if (eventID == 7001)
                        {
                            eventMsg = (shortAc[1]).ToLower() + " logon";
                        }
                        else
                        {
                            eventMsg = shortAc[1].ToLower() + " logoff";
                        }
                        WrtieToLog(eventMsg, 1111, EventLogEntryType.Information);                        
                    }
                    catch (EventLogNotFoundException ex)
                    {
                        WrtieToLog(ex.Message.ToString(), 1112, EventLogEntryType.Error);
                        throw;
                    }
                }
                log.Close();
            }
            catch (Exception ex)
            {
                WrtieToLog(ex.Message.ToString(), 1113, EventLogEntryType.Error);
                throw;
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
    }
}
