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
        private readonly int cooldownTime = 1;
        private int defaultQty = 10;
        private float defaultRarity = 1;



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
                        Dal.AddItemFound(Helper.Rarity.Epic, user);
                    }
                    else
                    {
                        if (rareItems.Contains(itemSelected.Name))
                        {
                            builder.WithTitle($":blue_heart: {Context.User.Username} just found {itemSelected.Name} in {location.Name} !! A rare item !! :blue_heart:")
                            .WithColor(Color.Blue);
                            Dal.AddItemFound(Helper.Rarity.Rare, user);
                        }
                        else
                        {
                            if (uncommonItems.Contains(itemSelected.Name))
                            {

                                builder.WithTitle($":green_heart: {Context.User.Username} just found {itemSelected.Name} in {location.Name} ! An uncommon item :green_heart:")
                                .WithColor(Color.Green);
                                Dal.AddItemFound(Helper.Rarity.Uncommon, user);
                            }
                            else
                            {

                                builder.WithTitle($"{Context.User.Username} just found {itemSelected.Name} in {location.Name} ...")
                                .WithColor(Color.LightGrey);
                                Dal.AddItemFound(Helper.Rarity.Common, user);
                            }
                        }
                    }

                }

                int totalSearches = user.GetTotalSearchs();
                double afterLuck = user.GetLuck();
                double luckModifier = Math.Round(afterLuck - beforeLuck, 6);
                string footer = "";

                if (luckModifier > 0)
                    footer += "+";

                footer += $"{luckModifier}";

                if (totalSearches % 100 == 0)
                {
                    int totalLegendaries = Dal.GetEveryLegendaries().Count;
                    footer += $" / {totalSearches}th search! {totalLegendaries} legendaries found in total.";
                }

                builder.WithFooter(footer);
                await ReplyAsync("", false, builder.Build());





            }

        }
        private List<string> randomItems = new List<string> { "Tree of Life", "Arcane Stone", "Holy Blood", "Mithril", "Jewel Sword", "Meteorite", "Muramasa", "Ogre Skin", "Water", "Bullets", "Tights", "Cookie", "Arrows", "Heartbeat Sensor" };

        private List<string> legendaryItems = new List<string> { "Tree of Life", "Arcane Stone", "Holy Blood", "Mithril", "Jewel Sword", "Meteorite", "Muramasa", "Ogre Skin", "Heartbeat Sensor" };

        private List<string> epicItems = new List<string> { "Holy Grail", "Masamune", "Fresh Sashimi", "Burdock", "Sweet Potato", "Soy Sauce", "Laptop (broken screen)", "Anatomy Model", "Playing Cards" };

        private List<string> rareItems = new List<string>{"Tuna", "Ramen", "Garlic", "Potato", "Fountain Pen", "Flower", "Holy Water", "Stallion Medal", "Bacchus",
            "Cooking Pot", "Kitchen Knife", "Glass Cup", "Carp", "Mudfish", "Saury" , "Cross", "Cassock"};

        private List<string> uncommonItems = new List<string>{ "Lighter", "Honey", "Ice", "Pill", "Alcohol", "Coffee", "Orange", "Chocolate", "Thick Paper",
            "TV", "Cookie",  "Curry Powder", "Fabric Armor", "Glass Cup", "Ripped Scroll - 1", "Ripped Scroll - 2",  "Buddhist Scripture", "Wooden Fish", "Whetstone", "Ingram MAC-10"};

    }
}
