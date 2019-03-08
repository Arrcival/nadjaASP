using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Nadja.Models
{
    public class Game
    {

        public string idServer;

        public string idChannel;

        private readonly int maxTime = 18000;

        private Stopwatch stopwatch;

        private Craft guessCraft;

        private int hidden;

        public Item hiddenItem;

        public double ElapsedTime()
        {
            return Math.Round((double)stopwatch.ElapsedMilliseconds / 1000, 1);
        }

        public double RemainingTime()
        {
            return Math.Round((double)(maxTime - stopwatch.ElapsedMilliseconds)/1000, 1);
        }

        public int GetPoints()
        {
            double temp = maxTime - stopwatch.ElapsedMilliseconds;
            double temp2 = temp / maxTime;
            double temp3 = temp2 * 40;
            int temp4 = 10 + (int)temp3;
            return temp4;
        }

        public bool TimedOut()
        {
            if (stopwatch.ElapsedMilliseconds > maxTime)
                return true;
            else
                return false;
        }

        public Game(string idServer, string idChannel)
        {
            stopwatch = new Stopwatch();
            this.idServer = idServer;
            this.idChannel = idChannel;

            guessCraft = Dal.GetRandomCraft();

            hidden = Helper.rng.Next(0, 3);

            switch (hidden)
            {
                case (0):
                    hiddenItem = guessCraft.ItemCrafted;
                    break;
                case (1):
                    hiddenItem = guessCraft.ItemsNeeded[0];
                    break;
                case (2):
                    hiddenItem = guessCraft.ItemsNeeded[1];
                    break;

            }
            stopwatch.Start();
        }

        public Helper.GameResult Guess(string answer)
        {
            if (stopwatch.ElapsedMilliseconds < maxTime)
            {
                if (hiddenItem.IsNamed(answer))
                {
                    stopwatch.Stop();
                    return Helper.GameResult.Victory;
                } else
                {
                    return Helper.GameResult.None;
                }
            }
            else
                return Helper.GameResult.Timeout;
        }

        public void PrintQuiz(EmbedBuilder builder)
        {
            string aString = $"{RemainingTime()} seconds to answer, ({Bot.prefix}ans <answer> to answer)";

            switch(hidden)
            {
                case (0):
                    builder.AddField($"? = {guessCraft.ItemsNeeded[0].Name} + {guessCraft.ItemsNeeded[1].Name}", aString);
                    break;

                case (1):
                    builder.AddField($"{guessCraft.ItemCrafted.Name} = ? + {guessCraft.ItemsNeeded[1].Name}", aString);
                    break;

                case (2):
                    builder.AddField($"{guessCraft.ItemCrafted.Name} = {guessCraft.ItemsNeeded[0].Name} + ?", aString);
                    break;
            }

            builder.WithColor(Color.DarkBlue);
        }


    }
}
