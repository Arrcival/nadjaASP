using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Nadja.Command
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        //Help function
        [Command("help")]
        public async Task HelpAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Command list")
                .WithDescription($"Prefix : **$** \n");

            string stringBS = "craft + <Item> : Show the craft of the <Item> \n" +
                "item + <Item> : Get basic infos of the <Item> \n" +
                "what + <Location> : Show every items in the <Location> \n" +
                "animal : Get info about animals \n" +
                "--------- \n" +
                "quiz : Craft quiz ! \n" +
                "ans + <answer> : Answer command \n" +
                "ranks : Top10 no-life people \n" +
                "--------- \n" +
                "loot : Copy in-game loot, with user parameters, helpful for mentors \n" +
                "--------- \n" +
                "search : Search for an item ? \n" +
                "toplucky : Look at your luck coefficient \n" +
                "topunlucky : Look at top10 luckiest people \n" +
                "rename : To rename yourself \n" +
                "profile : To look at your profile \n";

            string stringMisc = "help : Show help page \n" +
                "info : Get informations about me + Changelog \n" +
                "And other secret stuff..";

            builder.AddField("BS related", stringBS);
            builder.AddField("Non BS related", stringMisc);

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
                "Dank, this is a version 2! \n" +
                "Removed teehee (for real)\n");

            builder.WithColor(Color.DarkBlue);

            await ReplyAsync("", false, builder.Build());
        }


    }
}
