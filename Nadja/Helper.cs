using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nadja.Models;
using Discord;

namespace Nadja
{
    public static class Helper
    {
        public static readonly double PointPercentKeptEachDay = 0.95;

        public static List<Models.Game> Games = new List<Models.Game>();

        public enum Rarity { Common, Uncommon, Rare, Epic }
        public enum GameResult { Victory, Timeout, None}
        public static Random rng = new Random();

        public static DateTime Origin = new DateTime(2000, 1, 1);

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

        public static List<ServerUser> GetRanking(List<ServerUser> serverUsers, int amount = 10)
        {
            if (serverUsers.Count < amount)
                amount = serverUsers.Count;

            List<ServerUser> tempList = new List<ServerUser>();
            List<ServerUser> usersSorted = SortByPoints(serverUsers);
            
            for (int i = 0; i < amount; i++)
                tempList.Add(usersSorted[i]);

            return tempList;

        }

        public static List<ServerUser> GetLuckRanking(List<ServerUser> serverUsers, int amount = 10)
        {
            if (serverUsers.Count < amount)
                amount = serverUsers.Count;

            List<ServerUser> tempList = new List<ServerUser>();
            List<ServerUser> usersSorted = SortByLuck(serverUsers);

            for (int i = 0; i < amount; i++)
                tempList.Add(usersSorted[i]);

            return tempList;
        }

        public static List<ServerUser> SortByPoints(List<ServerUser> everyUsers)
        {
            List<ServerUser> newList = new List<ServerUser>();
            if (everyUsers.Count == 0)
                return newList;

            int count = everyUsers.Count;

            newList.Add(everyUsers[0]);
            everyUsers.Remove(everyUsers[0]);

            while (newList.Count < count)
            {
                int i = 0;
                bool found = false;
                while (!found)
                {
                    if (everyUsers[0].Points > newList[i].Points)
                    {
                        i++;
                        if (i == newList.Count)
                        {
                            found = true;
                        }
                    }
                    else
                    {
                        found = true;
                    }
                }
                newList.Insert(i, everyUsers[0]);
                everyUsers.Remove(everyUsers[0]);
            }

            newList.Reverse();
            return newList;
        }



        public static List<ServerUser> SortByLuck(List<ServerUser> everyUsers)
        {
            List<ServerUser> newList = new List<ServerUser>();
            if (everyUsers.Count == 0)
                return newList;

            int count = everyUsers.Count;

            newList.Add(everyUsers[0]);
            everyUsers.Remove(everyUsers[0]);
            while (newList.Count < count)
            {
                int i = 0;
                bool found = false;
                while (!found)
                {
                    if (everyUsers[0].GetLuck() > newList[i].GetLuck())
                    {
                        i++;
                        if (i == newList.Count)
                        {
                            found = true;
                        }
                    }
                    else
                    {
                        found = true;
                    }
                }
                newList.Insert(i, everyUsers[0]);
                everyUsers.Remove(everyUsers[0]);
            }

            newList.Reverse();
            return newList;
        }

        public static double GetCurrentTime()
        {
            return (DateTime.Now.ToUniversalTime() - Origin).TotalSeconds;
        }

        public static void AddItemFound(Rarity rarity, User user)
        {
            user.LastTimeSearch = GetCurrentTime();
            switch (rarity)
            {
                case Rarity.Common:
                    user.Common += 1;
                    break;
                case Rarity.Uncommon:
                    user.Uncommon += 1;
                    break;
                case Rarity.Rare:
                    user.Rare += 1;
                    break;
                case Rarity.Epic:
                    user.Epic += 1;
                    break;
                default:
                    break;
            }

            Dal.UpdateUserSearch(user);
        }

        public static EmbedBuilder ResolveQuiz(Models.Game game, GameResult result, string idUser, string idServer, string name)
        {
            EmbedBuilder builder = new EmbedBuilder();
            if (result == GameResult.Victory)
            {
                Dal.DoConnection();
                ServerUser serverUser = GetServerUser(idUser, idServer, name);
                int points = game.GetPoints();
                serverUser.AddPoints(points);
                serverUser.UpdateQuiz();
                Dal.CloseConnection();

                builder.AddField($"**{serverUser.DiscordName}** won in *{game.ElapsedTime()}*s !", $"Answer was **{game.hiddenItem.Name}** (*{points}*pts)");
                builder.WithColor(Color.Magenta);
                Games.Remove(game);
                return builder;
            }
            else if (result == GameResult.Timeout)
            {
                builder.AddField("Time out !", $"Answer was **{game.hiddenItem.Name}**");
                Games.Remove(game);
                return builder;
            }
            return null;
        }

        public static ServerUser GetServerUser(string idUser, string idServer, string name)
        {
            ServerUser serverUser = Dal.GetServerUser(idUser, idServer);
            if (serverUser == null)
            {
                User user = Dal.GetUser(idUser);
                if (user == null)
                {
                    Dal.CreateUser(idUser, name);
                    user = Dal.GetUser(idUser);
                }
                Dal.CreateServerUser(user, idServer);
                serverUser = Dal.GetServerUser(idUser, idServer);
            }
            return serverUser;
        }
    }
}
