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
        //Help function
        [Command("help")]
        public async Task HelpAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Get the command list on the website !")
                .WithDescription($"Prefix : **{Bot.prefix}** \n");


            builder.WithUrl("https://nadja.azurewebsites.net");

            builder.WithColor(Color.DarkPurple);
            builder.WithFooter("Bot made by Arrcival");

            await ReplyAsync("", false, builder.Build());

        }

        [Command("info")]
        public async Task WhoAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Nadja BS bot Version 2.0.0")
                .WithDescription("Developed by Arrcival");



            builder.AddField("Changelog", "" +
                "Dank, this is a version 2 ! \n" +
                "Removed teehee (for real)\n");

            builder.WithColor(Color.DarkBlue);

            await ReplyAsync("", false, builder.Build());
        }


    }
}
