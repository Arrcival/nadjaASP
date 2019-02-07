using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadja.Models
{
    public class ServerUser : User
    {
        public int Points;
        public string ServerNameUser;
        public string ServerID;

        public ServerUser(User user) : base(user) { }

        public ServerUser(int id, string DiscordID, string discordName, int gems, int common, int uncommon, int rare, int epic, string serverNameUser, int points, string serverID, List<Legendary> legendaries) : base(id, DiscordID, discordName, gems, common, uncommon, rare, epic, legendaries)
        {
            Points = points;
            ServerNameUser = serverNameUser;
            ServerID = serverID;

        }

        public void UpdateQuiz()
        {
            Dal.UpdateUserQuiz(this);
        }



    }
}
