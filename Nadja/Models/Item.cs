using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Nadja.Models
{
    public class Item
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Slangs { get; set; }

        public List<Craft> Crafts { get; set; }

        public List<Found> Founds { get; set; }

        public bool IsNamed(string way)
        {
            if (Slangs.Contains(way))
                return true;

            return false;
        }

        public string DisplayLocationItem()
        {
            string result = "";
            List<Found> tempList = new List<Found>(Founds.ToArray());
            while(tempList.Count > 0)
            {
                string tempString = "";
                double maxPercentage = Math.Round(100 * (double)tempList[0].Amount / tempList[0].Location.GetTotalItemsInArea(), 2);
                Found tempFind = null;
                foreach(Found find in tempList)
                {
                    
                    double percentage = Math.Round(100 * (double)find.Amount / find.Location.GetTotalItemsInArea(), 2);
                    if (percentage >= maxPercentage)
                    {
                        tempFind = find;
                        tempString = $"{find.Location.Name} ({find.Amount} / {find.Location.GetTotalItemsInArea()}) = {percentage}%\n";
                        maxPercentage = percentage;
                    }
                    if (find.Location.Name == "Random")
                    {
                        tempFind = find;
                        tempString = $"Random\n";
                        break;
                    }
                }

                tempList.Remove(tempFind);
                result += tempString;
            }
            
            
            switch (Name)
            {
                case ("Bird Egg"):
                    result += "Egg Crow\n";
                    break;
                case ("Bird Meat"):
                    result += "Meat Crow\n";
                    break;
                case ("Flower"):
                    result += "Bear \n";
                    break;
                case ("Garlic"):
                    result += "Bear \n";
                    break;
                case ("Burdock"):
                    result += "Gorilla \n";
                    break;
                case ("Fertilizer"):
                    result += "Gorilla \n";
                    break;
                case ("First Aid Box"):
                    result += "Dr. Wickeline \n";
                    break;
                case ("Holy Blood"):
                    result += "Dr. Wickeline \n";
                    break;
                case ("Cigarettes"):
                    result += "Dr. Meiji \n";
                    break;
                case ("Bull Intestine"):
                    result += "Dr. Meiji \n";
                    break;
                case ("Heartbeat Sensor"):
                    result += "Dr. Meiji \n";
                    break;
                case ("Feather"):
                    result += "Water crow \n" +
                        "Bread crow \n";
                    break;
                case ("Leather"):
                    result += "Dog \n";
                    break;
                case ("Fountain Pen"):
                    result += "Pen dog \n";
                    break;
                case ("Lighter"):
                    result += "Lighter dog \n";
                    break;
            }

            return result;
        }



        public void DisplayOnlyLocationItem(EmbedBuilder builder)
        {
            string aString = DisplayLocationItem();
            if (aString == "")
                aString = "There is no way to find this item.";

            builder.AddField($"To find {Name} :", aString, true);
        }

        public void DisplayItem(EmbedBuilder builder, bool firstItem)
        {
            string aString = "";
            // If this the first item, just make (craft + craft) or with locations if exists
            if (firstItem)
            {
                aString = DisplayLocationItem();
                string tinyStr = "";
                if (Crafts != null)
                {
                    if (aString != "")
                    {
                        aString += "OR ";
                    }
                    for (int j = 0; j < Crafts[0].ItemsNeeded.Count; j++)
                    {
                        aString += Crafts[0].ItemsNeeded[j].Name;
                        if (j != Crafts[0].ItemsNeeded.Count - 1)
                            aString += " + ";
                    }
                    tinyStr = "(" + Crafts[0].Amount.ToString() + ")";
                }

                if (aString == "")
                {
                    aString = "Looks like you can't find this item";
                }
                builder.AddField($"{Name} {tinyStr}", aString);
            }

            if (Crafts != null)
            {
                // Displaying first item needed for craft
                foreach (Item item in Crafts[0].ItemsNeeded)
                {
                    aString = item.DisplayLocationItem();
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

                    if (item.Crafts != null)
                    {
                        builder.AddField("------------------------------------", item.Name + " (" + item.Crafts[0].Amount + ")");
                        item.DisplayItem(builder, false);
                    }
                }

                // If items for crafting the askingItem exists, display it too using this function

            }
        }
    }
}
