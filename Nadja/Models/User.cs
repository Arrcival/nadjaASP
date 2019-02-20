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
                double allCommon = allSearches[0];
                double allUncommon = allSearches[1];
                double allRare = allSearches[2];
                double allEpic = allSearches[3];
                double allLegendaries = allSearches[4];

                double userTotal = Common + Uncommon + Rare + Epic + CountLegendaries();
                double allTotal = allSearches.Sum();

                double coeff = 0;
                coeff += Math.Pow(allLegendaries / allTotal, -1) * CountLegendaries();
                coeff += Math.Pow(allEpic / allTotal, -1) * Epic;
                coeff += Math.Pow(allRare / allTotal, -1) * Rare;
                coeff += Math.Pow(allUncommon / allTotal, -1) * Uncommon;
                coeff += Math.Pow(allCommon / allTotal, -1) * Common;

                double total = (coeff - Math.Pow(userTotal / 10, 1.1)) / userTotal;


                return Math.Round(total * 1000, 2);
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
