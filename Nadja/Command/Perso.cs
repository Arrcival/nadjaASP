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
                    "Indécis : 1 (combo nombre de gens x5)" + Environment.NewLine +
                    "Poteau : 2" + Environment.NewLine +
                    "Piéton : 2" + Environment.NewLine +
                    "Anglais : 2" + Environment.NewLine +
                    "Train : 3 (combo ciseaux x2)" + Environment.NewLine +
                    "Serpent : 4" + Environment.NewLine +
                    "180 no scope : 5" + Environment.NewLine +
                    "Demi-Orange : 5" + Environment.NewLine +
                    "Poisson : 5" + Environment.NewLine +
                    "Autotamponneuse : 6" + Environment.NewLine +
                    "Gandalf : 7" + Environment.NewLine +
                    "Rugbymen : 8" + Environment.NewLine +
                    "CRS : 10 (combo contexte jusqu'a x5)";
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
                    str += $"{civil.Nom} : {civil.Points} points" + Environment.NewLine;
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

        [Command("uinfo")]
        public async Task TaskAsync(IGuildUser user)
        {
            await ReplyAsync($"Name : {user.Nickname}" + Environment.NewLine + 
                $"Discord Name : {user.Username}" + Environment.NewLine +
                $"ID : {user.Id}" + Environment.NewLine +
                $"Mention : {user.Mention}" + Environment.NewLine +
                $"User : {user.ToString()}");
        }

    }
}
