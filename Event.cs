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
                string eventProvider = "System";
                string logName = eventProvider;
                int lastEntry = 0;
                EventLog log = new EventLog(logName);
                lastEntry = log.Entries.Count - 1;
                int eventID = log.Entries[lastEntry].EventID;
                string eventSource = log.Entries[lastEntry].Source;
                if (eventID == 7001 | eventID == 7002 | eventSource == "Service Control Manager") //ПОМЕНЯТЬ НА WINLOGON ПЕРЕД РЕЛИЗОМ!!
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
                        string hostName = System.Environment.MachineName;
                        string eventMsg = "";
                        if (eventID == 7001)
                        {
                            eventMsg = shortAc[1] + " logon";
                        }
                        else
                        {
                            eventMsg = shortAc[1] + " logoff";
                        }
                        string sourceLog = "EventMonitor";
                        EventLog systemEventLog = new EventLog("Application");
                        if (!EventLog.SourceExists(sourceLog))
                        {
                            EventLog.CreateEventSource(sourceLog, "Application");
                        }
                        systemEventLog.Source = sourceLog;
                        systemEventLog.WriteEntry(eventMsg, EventLogEntryType.Information, 1111);
                    }
                    catch (EventLogNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }

                log.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
