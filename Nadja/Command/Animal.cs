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
        //-craft command
        [Group("animals")]
        [Alias("a")]
        public class Animals : ModuleBase<SocketCommandContext>
        {
            // Default thing to display it nothing is inputed after -meme
            [Command]
            public async Task AnimalDefaultAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle($"Type {Bot.prefix}animals <animal> to get the info you want");
                builder.AddField("List of animals :",
                    "meat crow" + Environment.NewLine +
                    "egg crow" + Environment.NewLine +
                    "bat" + Environment.NewLine +
                    "pen dog" + Environment.NewLine +
                    "lighter dog" + Environment.NewLine +
                    "bear" + Environment.NewLine +
                    "gorilla" + Environment.NewLine +
                    "wickeline" + Environment.NewLine +
                    "meiji");
                builder.WithColor(Color.LightGrey);
                await ReplyAsync(embed: builder.Build());
            }

            // water crow info
            [Command("meat crow")]
            public async Task AnimalMCrowAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Meat Crow");
                builder.AddField("Locations", "Forest" + Environment.NewLine +
                    "Trail" + Environment.NewLine +
                    "Pond", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "110 secs", true);
                builder.AddField("Items", "Bird Meat" + Environment.NewLine +
                    "Feather" + Environment.NewLine +
                    "1 random food", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }
            // bread crow info
            [Command("egg crow")]
            public async Task AnimalBCrowAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Egg Crow");
                builder.AddField("Locations", "Dock" + Environment.NewLine +
                    "Beach" + Environment.NewLine +
                    "Lighthouse", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "110 secs", true);
                builder.AddField("Items", "Bird Eggs" + Environment.NewLine +
                    "Feather" + Environment.NewLine +
                    "1 random D armor supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            // bat info
            [Command("bat")]
            public async Task AnimalBatAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Bat");
                builder.AddField("Locations", "Tunnel" + Environment.NewLine +
                    "Temple" + Environment.NewLine +
                    "Chapel", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "110 secs", true);
                builder.AddField("Items", "Water" + Environment.NewLine +
                    "Bread" + Environment.NewLine +
                    "1 random A weapon supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            // lighter dog info
            [Command("lighter dog")]
            public async Task AnimalLDogAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Lighter Dog");
                builder.AddField("Locations", "Town Hall" + Environment.NewLine +
                    "Hotel" + Environment.NewLine +
                    "Uptown", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "140 secs", true);
                builder.AddField("Items", "Lighter" + Environment.NewLine +
                    "Leather" + Environment.NewLine +
                    "1 random A weapon supply" + Environment.NewLine +
                    "1 random D armor supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            // pen dog info
            [Command("pen dog")]
            public async Task AnimalPDogAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Pen Dog");
                builder.AddField("Locations", "School" + Environment.NewLine +
                    "Slum" + Environment.NewLine +
                    "Alley", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "140 secs", true);
                builder.AddField("Items", "Fountain Pen" + Environment.NewLine +
                    "Leather" + Environment.NewLine +
                    "1 random A weapon supply" + Environment.NewLine +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            // gorilla info
            [Command("gorilla")]
            public async Task AnimalGorillaAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Gorilla");
                builder.AddField("Locations", "Factory" + Environment.NewLine +
                    "Fire Station" + Environment.NewLine +
                    "Archery Range", false);
                builder.AddField("First Apparition", "360 secs", true);
                builder.AddField("Respawn Time", "170 secs", true);
                builder.AddField("Items", "Burdock" + Environment.NewLine +
                    "Fertilizer" + Environment.NewLine +
                    "1 random D armor supply" + Environment.NewLine +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            // gorilla info
            [Command("bear")]
            public async Task AnimalBearAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Bear");
                builder.AddField("Locations", "Hospital" + Environment.NewLine +
                    "Cemetary" + Environment.NewLine +
                    "Well", false);
                builder.AddField("First Apparition", "360 secs", true);
                builder.AddField("Respawn Time", "170 secs", true);
                builder.AddField("Items", "Garlic" + Environment.NewLine +
                    "Flower" + Environment.NewLine +
                    "1 random A weapon supply" + Environment.NewLine +
                    "1 random D armor supply" + Environment.NewLine +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            // wickeline info
            [Command("wickeline")]
            public async Task AnimalWickAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Dr. Wickeline");
                builder.AddField("Locations", "Random open area", false);
                builder.AddField("First Apparition", "540 secs", true);
                builder.AddField("Respawn Time", "None", true);
                builder.AddField("Items", "Holy Blood" + Environment.NewLine +
                    "First Aid Box" + Environment.NewLine +
                    "1 random A weapon supply" + Environment.NewLine +
                    "1 random D armor supply" + Environment.NewLine +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            // meiji info
            [Command("meiji")]
            public async Task AnimalMeijiAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Dr. Meiji");
                builder.AddField("Locations", "Research Center", false);
                builder.AddField("First Apparition", "None", true);
                builder.AddField("Respawn Time", "None", true);
                builder.AddField("Items", "Bull Intestine" + Environment.NewLine +
                    "Cigarettes" + Environment.NewLine +
                    "Heartbeat Sensor", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }
        }

    }
}
