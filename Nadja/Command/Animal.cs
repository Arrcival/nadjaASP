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
        public class Animals : ModuleBase<SocketCommandContext>
        {
            // Default thing to display it nothing is inputed after -meme
            [Command]
            public async Task AnimalDefaultAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle($"Type $animals <animal> to get the info you want");
                builder.AddField("List of animals :",
                    "meat crow \n" +
                    "egg crow \n" +
                    "bat \n" +
                    "pen dog \n" +
                    "lighter dog \n" +
                    "bear \n" +
                    "gorilla \n" +
                    "wickeline \n" +
                    "meiji \n");
                builder.WithColor(Color.LightGrey);
                await ReplyAsync("", false, builder.Build());
            }

            // water crow info
            [Command("meat crow")]
            public async Task AnimalWCrowAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Meat Crow");
                builder.AddField("Locations", "Forest \n" +
                    "Trail \n" +
                    "Pond", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "110 secs", true);
                builder.AddField("Items", "Bird Meat \n" +
                    "Feather \n" +
                    "1 random food", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
            }
            // bread crow info
            [Command("egg crow")]
            public async Task AnimalBCrowAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Egg Crow");
                builder.AddField("Locations", "Dock \n" +
                    "Beach \n" +
                    "Lighthouse", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "110 secs", true);
                builder.AddField("Items", "Bird Eggs \n" +
                    "Feather \n" +
                    "1 random D armor supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
            }

            // bat info
            [Command("bat")]
            public async Task AnimalBatAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Bat");
                builder.AddField("Locations", "Tunnel \n" +
                    "Temple \n" +
                    "Chapel", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "110 secs", true);
                builder.AddField("Items", "Water \n" +
                    "Bread \n" +
                    "1 random A weapon supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
            }

            // lighter dog info
            [Command("lighter dog")]
            public async Task AnimalLDogAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Lighter Dog");
                builder.AddField("Locations", "Town Hall \n" +
                    "Hotel \n" +
                    "Uptown", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "140 secs", true);
                builder.AddField("Items", "Lighter \n" +
                    "Leather \n" +
                    "1 random A weapon supply \n" +
                    "1 random D armor supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
            }

            // pen dog info
            [Command("pen dog")]
            public async Task AnimalPDogAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Pen Dog");
                builder.AddField("Locations", "School \n" +
                    "Slum \n" +
                    "Alley", false);
                builder.AddField("First Apparition", "180 secs", true);
                builder.AddField("Respawn Time", "140 secs", true);
                builder.AddField("Items", "Fountain Pen \n" +
                    "Leather \n" +
                    "1 random A weapon supply \n" +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
            }

            // gorilla info
            [Command("gorilla")]
            public async Task AnimalGorillaAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Gorilla");
                builder.AddField("Locations", "Factory \n" +
                    "Fire Station \n" +
                    "Archery Range \n", false);
                builder.AddField("First Apparition", "360 secs", true);
                builder.AddField("Respawn Time", "170 secs", true);
                builder.AddField("Items", "Burdock \n" +
                    "Fertilizer \n" +
                    "1 random D armor supply \n" +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
            }

            // gorilla info
            [Command("bear")]
            public async Task AnimalBearAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Info of Bear");
                builder.AddField("Locations", "Hospital \n" +
                    "Cemetary \n" +
                    "Well", false);
                builder.AddField("First Apparition", "360 secs", true);
                builder.AddField("Respawn Time", "170 secs", true);
                builder.AddField("Items", "Garlic \n" +
                    "Flower \n" +
                    "1 random A weapon supply \n" +
                    "1 random D armor supply \n" +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
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
                builder.AddField("Items", "Holy Blood \n" +
                    "First Aid Box \n" +
                    "1 random A weapon supply \n" +
                    "1 random D armor supply \n" +
                    "1 random icebox supply", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
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
                builder.AddField("Items", "Bull Intestine \n" +
                    "Cigarettes \n" +
                    "Heartbeat Sensor", false);
                builder.WithColor(Color.Blue);
                await ReplyAsync("", false, builder.Build());
            }
        }

    }
}
