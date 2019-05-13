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
        

        [Command("quiz"), RequireContext(ContextType.Guild)]
        [Alias("q")]
        public async Task QuizAsync()
        {
            Dal.DoConnection();
            EmbedBuilder builder = new EmbedBuilder();
            Models.Game game = FindGame(Context.Guild.Id.ToString(), Context.Channel.Id.ToString());
            if(game != null)
            {
                if (game.TimedOut())
                {
                    builder.AddField("Time out !", $"The answer was **{game.hiddenItem.Name}**");
                    Helper.Games.Remove(game);
                } else
                {
                    game.PrintQuiz(builder);
                }
                await ReplyAsync(embed: builder.Build());

            } else 
            {
                game = new Models.Game(Context.Guild.Id.ToString(), Context.Channel.Id.ToString());
                Helper.Games.Add(game);
                game.PrintQuiz(builder);
                await ReplyAsync(embed: builder.Build());
            }
            Dal.CloseConnection();
        }
        

        private Models.Game FindGame(string idServer, string idChannel)
        {
            foreach (Models.Game game in Helper.Games)
                if (game.idServer == Context.Guild.Id.ToString() && game.idChannel == Context.Channel.Id.ToString())
                    return game;
            return null;

        }

        public bool ChannelQuiz(string idChannel)
        {
            foreach (Models.Game game in Helper.Games)
                if (game.idChannel == idChannel)
                    return true;
            return false;
        }
        
    }
}
