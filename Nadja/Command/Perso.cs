using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nadja.Models;

namespace Nadja.Command
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("list")]
        public async Task ListAsync()
        {
            if (Context.User.Id == 88011881498812416 || Context.User.Id == 241262144572751872)
            {
                EmbedBuilder builder = new EmbedBuilder();
                string str = "" +
                    "\nIndécis : 1 (combo nombre de gens x5)" +
                    "\nPoteau : 2" +
                    "\nPiéton : 2" +
                    "\nAnglais : 2" +
                    "\nTrain : 3 (combo ciseaux x2)" +
                    "\nSerpent : 4" +
                    "\n180 no scope : 5" +
                    "\nDemi-Orange : 5" +
                    "\nPoisson : 5" +
                    "\nAutotamponneuse : 6" +
                    "\nGandalf : 7" +
                    "\nRugbymen : 8" +
                    "\nCRS : 10 (combo contexte jusqu'a x5)" +
                    "";
                builder.AddField("Incivilité :", str);
                await ReplyAsync(embed: builder.Build());
            }
        }

        [Command("i")]
        public async Task IAsync()
        {
            if (Context.User.Id == 88011881498812416 || Context.User.Id == 241262144572751872)
            {
                Dal.DoConnection();
                EmbedBuilder builder = new EmbedBuilder();
                List<Civil> users = Dal.GetEveryCivilUsers().ToList();
                Dal.CloseConnection();
                string str = "";
                foreach (Civil civil in users)
                    str += $"{civil.Nom} : {civil.Points} points\n";
                builder.AddField("Ranks", str);
                await ReplyAsync(embed: builder.Build());
            }
        }

        [Command("i")]
        public async Task IParamAsync(int points)
        {
            if(Context.User.Id == 88011881498812416 || Context.User.Id == 241262144572751872)
            {
                Dal.DoConnection();
                Dal.GivePoints(Context.User.Id.ToString(), points);
                Dal.CloseConnection();
                await ReplyAsync($"**{points}** points ajoutés à {Context.User.Username}");
            }
        }
    }
}
