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
        public static List<string> Queue = new List<string>();

        [Command("queue")]
        public async Task QueueAsync()
        {
            EmbedBuilder builder = BuildQueue();

            await ReplyAsync(embed: builder.Build());
        }

        [Command("queue"), RequireUserPermission(GuildPermission.DeafenMembers)]
        public async Task QueueAsync([Remainder] string name)
        {
            Queue.Add(name);
            await ReplyAsync($"**{name}** queued !");
        }
        

        public string Queuing(string name)
        {
            Queue.Add(name);
            string str = $"**{name}** queued !";
            if (Queue.Count == 1)
                str += "\nAnd it's already your turn !";
            return str;

        }

        [Command("next"), RequireUserPermission(GuildPermission.DeafenMembers)]
        public async Task NextAsync()
        {
            if(Queue.Count > 0)
            {
                string str = $"Thanks **{Queue[0]}** for singing !";
                if(Queue.Count > 1)
                {
                    str += $"\nNext : {Queue[1]} !";
                }
                await ReplyAsync(str);
                Queue.Remove(Queue[0]);
            } else
            {
                await ReplyAsync($"The queue is empty.");
            }
        }

        public EmbedBuilder BuildQueue()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Karaoke Queue");
            if (Queue.Count == 0)
            {
                builder.WithDescription("The queue is currently empty... \nIt might be sav turn !");
            }
            else
            {
                string str = $"Currently singing : **{Queue[0]}**";
                for (int i = 1; i < Queue.Count; i++)
                {
                    str += $"\n{Queue[i]}";
                }
                str += $"\nSav";
                builder.WithDescription(str);
            }

            builder.WithColor(Color.Teal);

            return builder;
        }
    }
}
