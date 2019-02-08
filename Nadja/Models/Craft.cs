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

        public void DisplayCraft(EmbedBuilder builder, bool first)
        {
            if(first)
                builder.AddField(ItemCrafted.Name, ItemsNeeded[0].Name + " + " + ItemsNeeded[1].Name, false);
            int count = 0;
            foreach (Item item in ItemsNeeded)
            {
                bool inline = true;
                string aString = item.DisplayLocationItem();
                if(item.Craft != null)
                {
                    if (aString != "")
                    {
                        aString += "OR ";
                    }
                    for (int j = 0; j < item.Craft.ItemsNeeded.Count; j++)
                    {
                        aString += item.Craft.ItemsNeeded[j].Name;
                        if (j != item.Craft.ItemsNeeded.Count - 1)
                            aString += " + ";
                    }
                }
                if (aString == "")
                {
                    aString = "Looks like you can't find this item";
                }

                if (count % 2 == 1)
                    inline = false;
                builder.AddField(item.Name, aString, inline);

                if (item.Craft != null)
                {
                    item.DisplayItem(builder, false);
                }
                count++;

            }
        }
    }
}
