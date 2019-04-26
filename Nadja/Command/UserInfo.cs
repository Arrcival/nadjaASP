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
        [Command("profile"), RequireContext(ContextType.Guild)]
        [Alias("pr")]
        public async Task ProfileAsync()
        {
            Dal.DoConnection();
            ServerUser serverUser = Dal.GetServerUser(Context.User.Id.ToString(), Context.Guild.Id.ToString());


            EmbedBuilder builder = new EmbedBuilder();

            Construct(builder, serverUser, Context.User.Id.ToString());
            Dal.CloseConnection();

            await ReplyAsync("", false, builder.Build());
        }

        [Command("profile"), RequireContext(ContextType.Guild)]
        [Alias("pr")]
        public async Task ProfilePlayerAsync(IGuildUser user)
        {
            Dal.DoConnection();
            
            EmbedBuilder builder = new EmbedBuilder();
            ServerUser serverUser = null;
            
            serverUser = Dal.GetServerUser(user.Id.ToString(), Context.Guild.Id.ToString());

            Construct(builder, serverUser, user.Id.ToString(), false);
            Dal.CloseConnection();

            await ReplyAsync("", false, builder.Build());
        }

        [Command("p"), RequireContext(ContextType.Guild)]
        public async Task PrAsync()
        {
            string idUser = Context.User.Id.ToString();
            Dal.DoConnection();
            ServerUser serverUser = Dal.GetServerUser(idUser, Context.Guild.Id.ToString());
            EmbedBuilder builder = new EmbedBuilder();

            ConstructSmall(builder, serverUser, idUser);



            Dal.CloseConnection();

            await ReplyAsync("", false, builder.Build());
        }

        [Command("p"), RequireContext(ContextType.Guild)]
        public async Task PrOtherAsync(IGuildUser user)
        {
            Dal.DoConnection();
            

            EmbedBuilder builder = new EmbedBuilder();
            ServerUser serverUser = null;
            
            serverUser = Dal.GetServerUser(user.Id.ToString(), Context.Guild.Id.ToString());
            

            ConstructSmall(builder, serverUser, user.Id.ToString());
            
            Dal.CloseConnection();

            await ReplyAsync("", false, builder.Build());
        }





        [Command("ranks"), RequireContext(ContextType.Guild)]
        [Alias("r")]
        public async Task RanksAsync()
        {
            Dal.DoConnection();
            EmbedBuilder builder = new EmbedBuilder();
            List<ServerUser> listRanks = Dal.GetEveryUser(Context.Guild.Id.ToString());

            listRanks = Helper.GetRanking(listRanks);
            builder.WithTitle($"Top 10 players ({Context.Guild.Name})" + Environment.NewLine);
            string aString = "";
            for (int i = 0; i < listRanks.Count; i++)
            {
                if(listRanks[i].DiscordID == Context.User.Id.ToString())
                    aString += $"**{Helper.GetRank(i + 1)} : {listRanks[i].ServerNameUser}** with {listRanks[i].Points} points";
                else
                    aString += $"{Helper.GetRank(i + 1)} : {listRanks[i].ServerNameUser} with {listRanks[i].Points} points";

                if (i != 0)
                    aString += $" *({listRanks[i].Points - listRanks[i - 1].Points})*" + Environment.NewLine;
                else
                    aString += Environment.NewLine;
            }
            Dal.CloseConnection();

            builder.WithDescription(aString);

            builder.WithColor(Color.DarkTeal);
            await ReplyAsync("", false, builder.Build());
        }

        private void Construct(EmbedBuilder builder, ServerUser serverUser, string idUser, bool ownProfile = true)
        {
            if (serverUser == null)
            {
                serverUser = GetServerUser(idUser);
                if (serverUser == null)
                {
                    builder.WithTitle("This user doesn't exists.")
                        .WithColor(Color.DarkGrey);
                    return;
                }
            }
            else
            {
                if (ownProfile)
                    builder.WithImageUrl(Context.User.GetAvatarUrl());
            }

            List<ServerUser> everyServerUsers = Dal.GetEveryUser(Context.Guild.Id.ToString());

            string rank = Helper.GetRank(everyServerUsers, serverUser);
            
            if (serverUser.Points != 0)
            {
                builder.WithTitle($"Profile of {serverUser.DiscordName}")
                    .AddField("Rank", $"{rank}", true)
                    .AddField("Score", $"{serverUser.Points}", true);
            }
            else
            {
                builder.WithTitle($"Profile of {serverUser.DiscordName}")
                     .AddField("Rank", "Never played", true)
                     .AddField("Score", "Never played", true);
            }
            builder.AddField("Gems", $":gem: {serverUser.Gems}", false);
            
            if (serverUser.GetTotalSearchs() > 0)
            {
                builder.AddField("Items found :",
                    ":black_heart: Common items : " + serverUser.Common + Environment.NewLine +
                    ":green_heart: Uncommon items : " + serverUser.Uncommon + Environment.NewLine +
                    ":blue_heart: Rare items : " + serverUser.Rare + Environment.NewLine +
                    ":purple_heart: Epic items : " + serverUser.Epic + Environment.NewLine +
                    ":yellow_heart: Legendary items : " + serverUser.CountLegendaries(), true);
                
                string display = "";
                if (serverUser.CountLegendaries() <= 0)
                    display = "No legendary item...";
                else
                {
                    for (int i = 0; i < serverUser.CountLegendaries(); i++)
                        display += serverUser.Legendaries[i].Name + Environment.NewLine;
                }
                
                builder.AddField("Legendary items :", display, true);
                
                double luckCoeff = serverUser.GetLuck();
                DateTime lastSearch = Helper.Origin.AddSeconds(serverUser.LastTimeSearch);
                builder.WithFooter($"Luck coefficient : {luckCoeff} / Last search : {lastSearch.ToString()}");
            }
            
            builder.WithColor(Color.Gold);
        }

        private void ConstructSmall(EmbedBuilder builder, ServerUser serverUser, string idUser)
        {
            if (serverUser == null)
            {
                serverUser = GetServerUser(idUser);
                if (serverUser == null)
                {
                    builder.WithTitle("This user does not exists.")
                        .WithColor(Color.DarkGrey);
                    return;
                }
            }

            List<ServerUser> everyServerUsers = Dal.GetEveryUser(Context.Guild.Id.ToString());
            builder.WithTitle($"Profile of {serverUser.DiscordName}");
            builder.AddField($"Rank : {Helper.GetRank(everyServerUsers, serverUser)}", $"Points : {serverUser.Points}", true);
            builder.AddField($"Searches : {serverUser.GetTotalSearchs()}", $"Legendaries found : {serverUser.CountLegendaries()}", true);
            builder.WithFooter($"Luck coefficient : {serverUser.GetLuck()}");
            builder.WithColor(Color.DarkOrange);

    }

        private ServerUser GetServerUser(string idUser)
        {
            
            User user = Dal.GetUser(idUser);
            if (user == null)
                return null;
            else
                return new ServerUser(user);
        }


    }
}
