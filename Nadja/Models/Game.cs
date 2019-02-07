using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadja.Models
{
    public class Game
    {

        public string idServer;

        private int maxTime = 18000;

        private Stopwatch stopwatch;

        private Craft guessCraft;

        private Item hiddenItem;


        public Game(string idServer)
        {

            guessCraft = Dal.GetRandomCraft();

            switch (Helper.rng.Next(0, 3))
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

        public int Guess(string answer)
        {
            if (stopwatch.ElapsedMilliseconds < maxTime)
            {
                if (hiddenItem.IsNamed(answer))
                {
                    //victory
                    return +1;
                }

                return 0;

            }
            else
            {
                return -1;
                //gameover
            }
        }

    }
}
