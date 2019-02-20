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

            await ReplyAsync(embed: builder.Build());

        }

        [Command("info")]
        public async Task WhoAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Nadja BS bot Version 2.1")
                .WithDescription("Developed by Arrcival");



            builder.AddField("Changelog", "" +
                "Added p, a lighter profile \n" +
                "Added back luckrank, to see who is the luckiest ! \n" +
                "Fixed many bugs or weird stuff\n" +
                "Fixed a lot of items in the database\n");

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
