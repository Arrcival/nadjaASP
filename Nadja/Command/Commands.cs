using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nadja.Models;
using Nadja;

namespace Nadja.Command
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly string VERSION = "2.3";

        //Help function
        [Command("help")]
        public async Task HelpAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Get the command list on the website !")
                .WithDescription($"Prefix : **{Bot.prefix}** \n");


            builder.WithUrl("https://nadja.azurewebsites.net");

            builder.WithColor(Color.DarkPurple);
            builder.WithFooter("Bot made by Arrcival, V" + VERSION);

            await ReplyAsync(embed: builder.Build());

        }

        [Command("info")]
        public async Task WhoAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Nadja BS bot V" + VERSION)
                .WithDescription("Developed by Arrcival");



            builder.AddField("Changelog", "" +
                "2.2 : Fixed profile for points\n" +
                "Added p (user) for other players\n" +
                "Fixed many database old items\n" +
                "The database is way more faster and efficient\n" +
                "Fixed some graphic issues\n" + 
                "Optimized some old code\n" +
                "2.3 : Removed ans, now you can answer without using ans\n" +
                "Added daily update that remove 5% of your points each day");

            builder.WithColor(Color.DarkBlue);

            await ReplyAsync(embed: builder.Build());
        }

        [Command("say"), RequireOwner]
        public async Task SayAsync(string channel, [Remainder] string text)
        {
            var server = Context.Client.Guilds.SingleOrDefault(g => g.Name == "Black Survival");
            ITextChannel channelsay;
            if (server != null)
            {
                channelsay = server.TextChannels.Single(tc => tc.Name == channel);
                //if (server.TextChannels.All(tc => tc.Name != "sh-comp-logger"))
                //    channel = await server.CreateTextChannelAsync("sh-comp-logger");
                //else channel = server.TextChannels.Single(tc => tc.Name == "sh-comp-logger");
                if (channel != null) await channelsay.SendMessageAsync(text);
            }
        }
        



    }
}
