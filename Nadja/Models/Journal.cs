using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nadja.Model
{
    public static class Journal
    {
        public static List<Log> Logs = new List<Log>();
        public static void AddLog(string user, string server, string channel, string log, string error = "")
        {
            Logs.Add(new Log(user, server, channel, log, error));
        }

        public static List<Log> GetLogs()
        {
            List<Log> logs = new List<Log>();
            for (int i = Logs.Count - 1; i >= 0; i--)
                logs.Add(Logs[i]);

            return logs;
        }
    }
}