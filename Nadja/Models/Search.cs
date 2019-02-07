using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nadja.Models
{
    public class Search
    {
        public string DiscordID;
        public double Time;

        public Search(string anID, double aTime)
        {
            DiscordID = anID;
            Time = aTime;
        }
    }
}