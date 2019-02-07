using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadja.Models
{
    public class User
    {
        public int ID { get; set; }
        public string DiscordID { get; set; }
        public string DiscordName { get; set; }

        public int Gems { get; set; }

        public int Common { get; set; }
        public int Uncommon { get; set; }
        public int Rare { get; set; }
        public int Epic { get; set; }


        public List<Legendary> Legendaries { get; set; }

        public User(int id, string discordID, string discordName, int gems, int common, int uncommon, int rare, int epic, List<Legendary> legendaries)
        {
            ID = id;
            DiscordID = discordID;
            DiscordName = discordName;
            Gems = gems;
            Common = common;
            Uncommon = uncommon;
            Rare = rare;
            Epic = epic;
            Legendaries = legendaries;
        }

        public User(User user) : this(user.ID, user.DiscordID, user.DiscordName, user.Gems, user.Common, user.Uncommon, user.Rare, user.Epic, user.Legendaries) { }

        public int GetTotalSearchs()
        {
            return Common + Uncommon + Rare + Epic + CountLegendaries();
        }

        public double GetLuck()
        {
            if (GetTotalSearchs() > 0)
            {
                List<double> allSearches = Dal.GetEverySearches();
                double allAmount = allSearches[0] + allSearches[1] + allSearches[2] + allSearches[3] + allSearches[4];

                double coeff = 0;
                coeff += Math.Pow(((allSearches[0] + 1) / allAmount), -1) * Common;
                coeff += Math.Pow(((allSearches[1] + 1) / allAmount), -1) * Uncommon;
                coeff += Math.Pow(((allSearches[2] + 1) / allAmount), -1) * Rare;
                coeff += Math.Pow(((allSearches[3] + 1) / allAmount), -1) * Epic;
                coeff += Math.Pow(((allSearches[4] + 1) / allAmount), -1) * CountLegendaries();
                double total = (coeff - Math.Pow(GetTotalSearchs() / 10, 1.1)) / this.GetTotalSearchs();

                return Math.Round(total, 6);
            }
            else
            {
                return 0;
            }

        }

        public void Update()
        {
            Dal.UpdateUserSearch(this);
        }
        
        public int CountLegendaries()
        {
            if (Legendaries == null )
                return 0;
            else
                return Legendaries.Count;
        }
    }
}
