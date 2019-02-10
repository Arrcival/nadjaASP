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
        private static List<Quiz> Games = new List<Quiz>();

        [Command("quiz"), RequireContext(ContextType.Guild)]
        public async Task QuizAsync()
        {
            bool found = false;
            EmbedBuilder builder = new EmbedBuilder();
            Quiz game = FindGame(Context.Guild.Id.ToString(), Context.Channel.Id.ToString());
            if(game != null)
            {
                found = true;
                if (game.TimedOut())
                {
                    builder.AddField("Sorry, but the game is over.", $"Answer was {game.hiddenItem.Name}");
                    Games.Remove(game);
                } else
                {
                    game.PrintQuiz(builder);
                }
                await ReplyAsync("", false, builder.Build());

            } else 
            {
                game = new Quiz(Context.Guild.Id.ToString(), Context.Channel.Id.ToString());
                Games.Add(game);
                game.PrintQuiz(builder);
                await ReplyAsync("", false, builder.Build());
            }
        }


        [Command("ans"), RequireContext(ContextType.Guild)]
        public async Task AnswerAsync()
        {
            Quiz game = FindGame(Context.Guild.Id.ToString(), Context.Channel.Id.ToString());
            if(game != null)
            {
                EmbedBuilder builder = new EmbedBuilder();
                if (game.TimedOut())
                {
                    builder.AddField("Sorry, but the game is over.", $"Answer was {game.hiddenItem.Name}");
                    Games.Remove(game);
                }
                else
                {
                    game.PrintQuiz(builder);
                }
                await ReplyAsync("", false, builder.Build());
            }

        }

        [Command("ans"), RequireContext(ContextType.Guild)]
        public async Task AnswerAsync([Remainder] string answer)
        {
            Quiz game = FindGame(Context.Guild.Id.ToString(), Context.Channel.Id.ToString());

            if (game != null)
            {
                Helper.GameResult result = game.Guess(answer);
                EmbedBuilder builder = new EmbedBuilder();
                if (result == Helper.GameResult.Victory)
                {
                    ServerUser serverUser = Dal.GetServerUser(Context.User.Id.ToString(), Context.Guild.Id.ToString());
                    if(serverUser == null)
                    {
                        User user = Dal.GetUser(Context.User.Id.ToString());
                        if (user == null)
                        {
                            Dal.CreateUser(Context.User.Id.ToString(), Context.User.Username);
                            user = Dal.GetUser(Context.User.Id.ToString());
                        }
                        Dal.CreateServerUser(user, Context.Guild.Id.ToString());
                        serverUser = Dal.GetServerUser(Context.User.Id.ToString(), Context.Guild.Id.ToString());
                    }
                    int points = game.GetPoints();
                    serverUser.AddPoints(points);
                    serverUser.UpdateQuiz();

                    builder.AddField($"{Context.User.Username} won in {game.ElapsedTime()}s !", $"Answer was {game.hiddenItem.Name}, ({points}pts)");
                    builder.WithColor(Color.Magenta);
                    Games.Remove(game);
                    await ReplyAsync("", false, builder.Build());
                }
                else if (result == Helper.GameResult.Timeout)
                {
                    builder.AddField("Sorry, but the game is over.", $"Answer was {game.hiddenItem.Name}");
                    Games.Remove(game);
                    await ReplyAsync("", false, builder.Build());
                }
            }


        }

        private Quiz FindGame(string idServer, string idChannel)
        {
            foreach (Quiz game in Games)
                if (game.idServer == Context.Guild.Id.ToString() && game.idChannel == Context.Channel.Id.ToString())
                    return game;
            return null;

        }
    }
}
