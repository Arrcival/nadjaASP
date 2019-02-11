using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nadja.Models
{
    public class Log
    {
        public DateTime Time;
        public string User;
        public string Server;
        public string Channel;
        public string Command;
        public string Error;

        public Log(string user, string server, string channel, string alog, string error = "")
        {
            User = user;
            Server = server;
            Channel = channel;
            Command = alog;
            Error = error;
            Time = DateTime.Now;
        }
    }
}