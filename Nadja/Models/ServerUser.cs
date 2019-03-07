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

        public ServerUser(User user) : base(user)
        {
            Points = 0;
            ServerNameUser = user.DiscordName;
            ServerID = "NONE";
        }

        public ServerUser(User user, int points, string serverID) : base(user) {
            Points = points;
            ServerNameUser = user.DiscordName;
            ServerID = serverID;
        }

        public ServerUser(int id, string DiscordID, string discordName, int gems, int common, int uncommon, int rare, int epic, string serverNameUser, int points, string serverID, List<Legendary> legendaries, double lastTime) : base(id, DiscordID, discordName, gems, common, uncommon, rare, epic, legendaries, lastTime)
        {
            Points = points;
            ServerNameUser = serverNameUser;
            ServerID = serverID;

        }

        public void UpdateQuiz()
        {
            Dal.UpdateUserQuiz(this);
        }

        public void AddPoints(int points)
        {
            Points += points;
        }

        public override string ToString()
        {
            return "SERVER " + base.ToString() + $" Points : {Points}";
        }


    }
}
