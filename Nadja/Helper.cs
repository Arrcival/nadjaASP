using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nadja.Models;

namespace Nadja
{
    public static class Helper
    {
        public static Random rng = new Random();

        public static string DiscordPingDelimiter(string testString)
        {
            char delimiter = '!';
            char delimiter2 = '>';
            if (testString.Contains("<@!"))
            {
                testString = testString.Split(delimiter)[1];
                testString = testString.Split(delimiter2)[0];
            }
            return testString;
        }

        public static string GetRank(List<ServerUser> serverUsers, ServerUser serverUser)
        {
            int rank = 1;
            foreach (ServerUser randomUser in serverUsers)
                if (randomUser.Points > serverUser.Points)
                    rank++;

            return GetRank(rank);
        }

        public static string GetRank(int rank)
        {
            if (rank <= 25)
            {
                string visual;
                int numero = rank % 5;
                if (rank <= 5)
                    visual = "Dragon";
                else if (rank <= 10)
                    visual = "Bear";
                else if (rank <= 15)
                    visual = "Lion";
                else if (rank <= 20)
                    visual = "Wolf";
                else
                    visual = "Fox";
                switch (numero)
                {
                    case (1):
                        visual += " I"; break;
                    case (2):
                        visual += " II"; break;
                    case (3):
                        visual += " III"; break;
                    case (4):
                        visual += " IV"; break;
                    case (0):
                        visual += " V"; break;
                    default:
                        visual += " ?"; break;
                }
                return visual;
            }
            else
                return "Not ranked";
        }

        public static List<ServerUser> GetRanking(List<ServerUser> serverUsers)
        {
            List<ServerUser> tempList = new List<ServerUser>();
            int maxTable = 10;
            if (serverUsers.Count < 10)
                maxTable = serverUsers.Count;
            for (int i = 1; i <= maxTable; i++)
            {
                ServerUser maxUser = serverUsers[0];
                foreach (ServerUser serverUser in serverUsers)
                    if (serverUser.Points > maxUser.Points)
                        maxUser = serverUser;

                tempList.Add(maxUser);
                serverUsers.Remove(maxUser);
            }

            return tempList;

        }
    }
}
