using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static Stopwatch stopwatch = new Stopwatch();
        private static List<Search> searches = new List<Search>();
        private readonly int cooldownTime = 30000;



        //-search command
        [Command("search")]
        public async Task SearchAsync()
        {

            if (!stopwatch.IsRunning)
                stopwatch.Start();

            string idUser = Context.User.Id.ToString();
            string nameUser = Context.User.Username;

            bool valid = true;

            foreach(Search search in searches)
            {
                if(search.DiscordID == idUser)
                {
                    if(stopwatch.ElapsedMilliseconds >= search.Time + cooldownTime)
                    {
                        valid = true;
                        searches.Remove(search);
                        break;
                    } else
                    {
                        valid = false;
                        await ReplyAsync($"**{Context.User.Username}**, you have to wait **{(int)((cooldownTime - (stopwatch.ElapsedMilliseconds - search.Time)) / 1000)}** secs before using this command again.", false);
                    }
                }
            }

            if(valid)
            {
                searches.Add(new Search(idUser, stopwatch.ElapsedMilliseconds));

                Dal.DoConnection();
                User user = Dal.GetUser(idUser);
                if (user == null)
                {
                    Dal.CreateUser(idUser, nameUser);
                    user = Dal.GetUser(idUser);
                }

                double beforeLuck = user.GetLuck();
                EmbedBuilder builder = new EmbedBuilder();
                int dice = Helper.rng.Next(1, 1001); // 0.1% to loot a legendary item
                List<Legendary> legendariesList = Dal.GetEveryLegendaries();

                if(dice <= 1)
                {
                    int itemFound = Helper.rng.Next(0, legendariesList.Count);
                    Legendary legendary = legendariesList[itemFound];
                    builder.WithTitle($":clap: :yellow_heart: :clap: {Context.User.Username} just found {legendary.Name} !!! A legendary item !!!!! :clap: :yellow_heart: :clap:")
                            .WithColor(Color.Gold);

                    Dal.AddLegendary(user, legendary);
                }
                else
                {
                    Location location = Dal.GetLocationFromInt(Helper.rng.Next(1, 22));
                    int totalItemAmount = location.GetTotalItemsInArea();
                    int idItemSelected = Helper.rng.Next(0, totalItemAmount);

                    List<Item> everyItems = location.GetEveryItems();
                    int amount = Helper.rng.Next(location.GetEveryItems().Count);


                    Item itemSelected = everyItems[amount];

                    if (epicItems.Contains(itemSelected.Name))
                    {
                        builder.WithTitle($":purple_heart:  {Context.User.Username} just found {itemSelected.Name} in {location.Name} !!! An epic item !!! :purple_heart: ")
                            .WithColor(Color.Purple);
                        Helper.AddItemFound(Helper.Rarity.Epic, user);
                    }
                    else
                    {
                        if (rareItems.Contains(itemSelected.Name))
                        {
                            builder.WithTitle($":blue_heart: {Context.User.Username} just found {itemSelected.Name} in {location.Name} !! A rare item !! :blue_heart:")
                            .WithColor(Color.Blue);
                            Helper.AddItemFound(Helper.Rarity.Rare, user);
                        }
                        else
                        {
                            if (uncommonItems.Contains(itemSelected.Name))
                            {

                                builder.WithTitle($":green_heart: {Context.User.Username} just found {itemSelected.Name} in {location.Name} ! An uncommon item :green_heart:")
                                .WithColor(Color.Green);
                                Helper.AddItemFound(Helper.Rarity.Uncommon, user);
                            }
                            else
                            {

                                builder.WithTitle($"{Context.User.Username} just found {itemSelected.Name} in {location.Name} ...")
                                .WithColor(Color.LightGrey);
                                Helper.AddItemFound(Helper.Rarity.Common, user);
                            }
                        }
                    }

                }

                List<double> allSearches = Dal.GetEverySearches();
                int totalSearches = (int)allSearches.Sum();
                double afterLuck = user.GetLuck();
                double luckModifier = Math.Round(afterLuck - beforeLuck, 6);
                string footer = "";

                if (luckModifier > 0)
                    footer += "+";

                footer += $"{luckModifier}";

                if (totalSearches % 100 == 0)
                {
                    int totalLegendaries = Dal.GetEveryPossess();
                    footer += $" / {totalSearches}th search! {totalLegendaries} legendaries found in total.";
                }

                builder.WithFooter(footer);
                await ReplyAsync(embed: builder.Build());


                Dal.CloseConnection();



            }

        }

        [Command("loot")]
        public async Task LootDefaultAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField("To use command loot :", "-loot <location> [1 < quantity (default 10) < 25] [0 < rare item % (default 1) < 100]");
            builder.WithColor(Color.DarkRed);
            await ReplyAsync(embed: builder.Build());

        }

        [Command("loot")]
        public async Task LootDefaultAsync(string place)
        {
            Dal.DoConnection();
            Location location = Dal.GetLocation(place);
            EmbedBuilder builder = new EmbedBuilder();
            Dal.CloseConnection();

            if (location != null)
                DoLoot(builder, location);
            else
                builder.WithTitle("Check the name of your location.");

            await ReplyAsync(embed: builder.Build());
        }

        [Command("loot")]
        public async Task LootQtyAsync(string place, int qty)
        {
            Dal.DoConnection();
            Location location = Dal.GetLocation(place);
            Dal.CloseConnection();

            EmbedBuilder builder = new EmbedBuilder();

            if (location != null)
            {
                if (qty > 0 && qty <= 25)
                    DoLoot(builder, location, qty);
                else
                    builder.WithTitle("Quantity have to be between 1 and 25.");

            }
            else
                builder.WithTitle("Check the name of your location.");

            await ReplyAsync(embed: builder.Build());
        }

        [Command("loot")]
        public async Task LootQtyRtyAsync(string place, int qty, float rare)
        {
            Dal.DoConnection();
            Location location = Dal.GetLocation(place);
            EmbedBuilder builder = new EmbedBuilder();
            Dal.CloseConnection();

            if (location != null)
            {
                if (qty > 0 && qty <= 25)
                    if (rare >= 0 && rare <= 100)
                        DoLoot(builder, location, qty, rare);
                    else
                        builder.WithTitle("Rarity have to be between 0 and 100.");
                else
                    builder.WithTitle("Quantity have to be between 1 and 25.");
            }
            else
            {
                builder.WithTitle("Check the name of your location.");
            }

            await ReplyAsync(embed: builder.Build());
        }

        public void DoLoot(EmbedBuilder builder, Location location, int quantity = 10, float rarity = 1)
        {
            string items = "";
            List<int> itemAlreadyPicked = new List<int>();
            for (int i = 0; i < quantity; i++)
            {

                if (Helper.rng.NextDouble() * 100 <= 100 - rarity)
                {
                    int idItemSelected = Helper.rng.Next(1, location.GetTotalItemsInArea() + 1);
                    while (itemAlreadyPicked.Contains(idItemSelected))
                    {
                        idItemSelected = Helper.rng.Next(1, location.GetTotalItemsInArea() + 1);
                    }

                    Item itemSelected = new Item();

                    int totalItemAmount = location.GetTotalItemsInArea();

                    List<Item> everyItems = location.GetEveryItems();
                    int amount = Helper.rng.Next(location.GetEveryItems().Count);

                    itemSelected = everyItems[amount];

                    itemAlreadyPicked.Add(idItemSelected);

                    items += itemSelected.Name + " \n";

                }
                else
                    items += randomItems[Helper.rng.Next(0, randomItems.Count)] + " \n";
            }

            
            builder.AddField($"{quantity} items in {location.Name} (Rare item : {rarity}%)", items);
            
            builder.WithColor(Color.DarkRed);
        }


        private List<string> randomItems = new List<string> { "Tree of Life", "Arcane Stone", "Holy Blood", "Mithril", "Jewel Sword", "Meteorite", "Muramasa", "Ogre Skin", "Water", "Bullets", "Tights", "Cookie", "Arrows", "Heartbeat Sensor" };

        private List<string> legendaryItems = new List<string> { "Tree of Life", "Arcane Stone", "Holy Blood", "Mithril", "Jewel Sword", "Meteorite", "Muramasa", "Ogre Skin", "Heartbeat Sensor" };

        private List<string> epicItems = new List<string> { "Holy Grail", "Masamune", "Fresh Sashimi", "Burdock", "Sweet Potato", "Soy Sauce", "Laptop (broken screen)", "Anatomy Model", "Playing Cards" };

        private List<string> rareItems = new List<string>{"Tuna", "Ramen", "Garlic", "Potato", "Fountain Pen", "Flower", "Holy Water", "Stallion Medal", "Bacchus",
            "Cooking Pot", "Kitchen Knife", "Glass Cup", "Carp", "Mudfish", "Saury" , "Cross", "Cassock"};

        private List<string> uncommonItems = new List<string>{ "Lighter", "Honey", "Ice", "Pill", "Alcohol", "Coffee", "Orange", "Chocolate", "Thick Paper",
            "TV", "Cookie",  "Curry Powder", "Fabric Armor", "Glass Cup", "Ripped Scroll - 1", "Ripped Scroll - 2",  "Buddhist Scripture", "Wooden Fish", "Whetstone", "IM-10"};



        [Command("luck")]
        public async Task LuckAsync()
        {
            Dal.DoConnection();
            User user = Dal.GetUser(Context.User.Id.ToString());
            if(user != null)
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle($"{Context.User.Username}, your luck coefficient is {user.GetLuck()}")
                                .WithColor(Color.DarkGreen);
                await ReplyAsync("", false, builder.Build());
            }
            Dal.CloseConnection();

        }

        [Command("luck")]
        public async Task LuckPlayerAsync([Remainder] string name)
        {
            Dal.DoConnection();
            EmbedBuilder builder = new EmbedBuilder();
            name = Helper.DiscordPingDelimiter(name);

            string idUser = Dal.GetIdUser(name);

            if (idUser != null)
            {
                User user = Dal.GetUser(idUser);
                builder.WithTitle($"The luck coefficient of {user.DiscordName} is {user.GetLuck()}")
                    .WithColor(Color.DarkGreen);
            }
            else
            {
                User user = Dal.GetUser(name);
                if(user == null)
                {
                    builder.WithTitle($"The luck coefficient of {user.DiscordName} is {user.GetLuck()}")
                    .WithColor(Color.DarkGreen);
                } else
                {
                    builder.WithTitle($"{name} does not exists...")
                        .WithColor(Color.DarkGreen);

                }
            }
            Dal.CloseConnection();

            await ReplyAsync(embed: builder.Build());
        }

        [Command("legendaries")]
        public async Task LegendariesAsync()
        {
            Dal.DoConnection();
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"{Dal.GetEveryPossess()} have been found in total.")
                .WithColor(Color.DarkGreen);
            await ReplyAsync(embed: builder.Build());

            Dal.CloseConnection();

        }

        // Top lucky players ?
        [Command("luckranks")]
        public async Task LuckRanksAsync()
        {
            Dal.DoConnection();
            List<ServerUser> everyUsers = Dal.GetEveryUser(Context.Guild.Id.ToString());

            EmbedBuilder builder = new EmbedBuilder();
            string aString = "";

            int max = 10;
            if (everyUsers.Count < 10)
                max = everyUsers.Count;

            for(int i = 1; i <= max; i++)
            {
                double maxLuck = -1;
                ServerUser tempUser = null;

                foreach(ServerUser user in everyUsers)
                {
                    if (user.GetLuck() > maxLuck)
                    {
                        tempUser = user;
                        maxLuck = user.GetLuck();
                    }
                }

                if(tempUser != null)
                    aString += $"#{i} : {tempUser.DiscordName} with {tempUser.GetLuck()}\n";

                everyUsers.Remove(tempUser);
            }

            builder.AddField($"Top 10 luckiest players", aString);
            builder.WithColor(Color.DarkGreen);
            
            Dal.CloseConnection();

            await ReplyAsync(embed: builder.Build());
        }


        [Command("search rc")]
        public async Task SearchRCAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"{Context.User.Username} just found nothing in the Research Center...")
                .WithColor(Color.LightGrey);
            await ReplyAsync("", false, builder.Build());

        }

        [Command("search research center")]
        public async Task SearchResearchAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"{Context.User.Username} just found nothing in the Research Center...")
                .WithColor(Color.LightGrey);
            await ReplyAsync("", false, builder.Build());

        }

        [Command("search legendary")]
        public async Task SearchLegAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($":clap: :yellow_heart: :clap: {Context.User.Username} just found a super legendary item !!!!! No seriously you really think this command work ? :clap: :yellow_heart: :clap:")
                            .WithColor(Color.Gold);
            await ReplyAsync("", false, builder.Build());

        }







    }
}
