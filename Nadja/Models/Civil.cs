using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nadja.Models
{
    public class Civil
    {
        public int Points { get; set; }
        public string Nom { get; set; }

        public Civil(string name, int points)
        {
            Nom = name;
            Points = points;
        }
    }
}