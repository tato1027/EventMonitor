using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EventMonitor
{
    class Event
    {
        public static void OnEntryWritten(object source, EntryWrittenEventArgs e)
        {
            try
            {
                //testing...
                string eventProvider = "System";
                string logName = eventProvider;
                int lastEntry = 0;
                EventLog log = new EventLog(logName);
                lastEntry = log.Entries.Count - 1;
                int eventID = log.Entries[lastEntry].EventID;
                string eventSource = log.Entries[lastEntry].Source;
                if (eventID == 7001 | eventID == 7002 | eventSource == "Service Control Manager") //ПОМЕНЯТЬ НА WINLOGON!!
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
                        string hostName = log.Entries[lastEntry].MachineName;
                        string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
                        string time = log.Entries[lastEntry].TimeGenerated.ToString();

                        //Write to file
                        using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(@"D:\Logon.log", true))
                        {
                            file.WriteLine(account + "/" + time + "/" + ipAddress + "/" + hostName + "/");
                        }
                        //Console.WriteLine(time);
                        //Console.WriteLine(account);
                        //Console.WriteLine(hostName);
                        //Console.WriteLine(ipAddress);
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
