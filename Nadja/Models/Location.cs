using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Nadja.Models
{
    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Found> Founds { get; set; }

        public Location(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int GetTotalItemsInArea()
        {
            int count = 0;
            for (int i = 0; i < Founds.Count; i++)
                count += Founds[i].Amount;
            return count;
        }

        public void DisplayItemsLocation(EmbedBuilder builder)
        {
            string aString = "There is no item in this location";
            if (Founds.Count > 0)
            {
                aString = "";
                for (int i = 0; i < Founds.Count; i++)
                {
                    aString += Founds[i].Item.Name + "(" + Founds[i].Amount + ")";
                    if (i != Founds.Count - 1)
                        aString += ", ";
                }
            }
            builder.AddField($"Items in the location {Name} :", aString);
        }
    }
}
