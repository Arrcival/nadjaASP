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
        private readonly string VERSION = "2.6.2";

        //Help function
        [Command("help")]
        [Alias("h")]
        public async Task HelpAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Get the command list on the website !")
                .WithDescription($"Prefix : **{Bot.prefix}** \n");


            builder.WithUrl("https://nadja.azurewebsites.net/Home/Commands");

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


            builder.WithUrl("https://nadja.azurewebsites.net/Home/Changelog");

            builder.WithColor(Color.DarkBlue);

            await ReplyAsync(embed: builder.Build());
        }

        [Command("slangs")]
        public async Task SlangsAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Get the slang list on the website !");


            builder.WithUrl("https://nadja.azurewebsites.net/Home/Slangs");

            builder.WithColor(Color.Magenta);

            await ReplyAsync(embed: builder.Build());
        }

        [Command("wiki")]
        public async Task WikiAsync()
        {
            await ReplyAsync("https://blacksurvival.gamepedia.com/Black_Survival_Wiki");
        }

        

        [Command("mentoring")]
        public async Task MentorAsync()
        {            
            await ReplyAsync("Go to <#451026647601119263> and select the book emote to get access for the mentoring section.");
        }

        [Command("say"), RequireOwner]
        public async Task SayAsync(string channel, [Remainder] string text)
        {
            var server = Context.Client.Guilds.SingleOrDefault(g => g.Id == Context.Guild.Id);
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
