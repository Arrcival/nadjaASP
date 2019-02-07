using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Nadja.Models
{
    public class Craft
    {
        public int ID { get; set; }
        public int Amount { get; set; }

        public Item ItemCrafted { get; set; }

        public List<Item> ItemsNeeded { get; set; }

        public void DisplayCraft(EmbedBuilder builder)
        {
            foreach (Item item in ItemsNeeded)
            {
                builder.AddField("------------------------------------", item.Name);

                string aString = item.DisplayLocationItem();
                if (item.Crafts != null)
                {
                    if (aString != "")
                    {
                        aString += "OR ";
                    }
                    for (int j = 0; j < item.Crafts[0].ItemsNeeded.Count; j++)
                    {
                        aString += item.Crafts[0].ItemsNeeded[j].Name;
                        if (j != item.Crafts[0].ItemsNeeded.Count - 1)
                            aString += " + ";
                    }
                }
                if (aString == "")
                {
                    aString = "Looks like you can't find this item";
                }
                builder.AddField(item.Name, aString, true);

                
            }

            // Call this to add and display the craft on the current builder
            // Must be recursive ?
        }
    }
}
