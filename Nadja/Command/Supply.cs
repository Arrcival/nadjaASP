using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Discord;
using Discord.Commands;

namespace Nadja.Command
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        //Group command
        [Group("supply")]
        [Alias("sp")]
        public class Supply : ModuleBase<SocketCommandContext>
        {
            // Default thing to display it nothing is inputed after -meme
            [Command]
            public async Task SupplyDefaultAsync()
            {
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("If you had the 3 supplies, you would get :");
                string supply = "";
                supply += ASupply[Helper.rng.Next(ASupply.Count)] + "\n";
                supply += DSupply[Helper.rng.Next(DSupply.Count)] + "\n";
                supply += Icebox[Helper.rng.Next(Icebox.Count)] + "\n";
                builder.WithDescription(supply);
                builder.WithColor(Color.Purple);
                await ReplyAsync(embed: builder.Build());
            }

            // -meme jp
            [Command("a")]
            public async Task ASupplyAsync()
            {
                double stat = Math.Round((double)1 / ASupply.Count * 100, 2);
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle($"The list of every A supply items ({stat}%)");
                string supply = ASupply[0];
                for(int i = 1; i < ASupply.Count; i++)
                    supply += ", " + ASupply[i];
                builder.WithDescription(supply);
                builder.WithColor(Color.Green);
                await ReplyAsync(embed: builder.Build());
            }
            [Command("d")]
            public async Task DSupplyAsync()
            {
                double stat = Math.Round((double)1 / DSupply.Count * 100, 2);
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle($"The list of every D supply items ({stat}%)");
                string supply = DSupply[0];
                for (int i = 1; i < DSupply.Count; i++)
                    supply += ", " + DSupply[i];
                builder.WithDescription(supply);
                builder.WithColor(Color.LightOrange);
                await ReplyAsync(embed: builder.Build());
            }
            [Command("ice")]
            public async Task IceSupplyAsync()
            {
                double stat = Math.Round((double)1 / Icebox.Count * 100, 2);
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle($"The list of every Icebox items ({stat}%)");
                string supply = Icebox[0];
                for (int i = 1; i < Icebox.Count; i++)
                    supply += ", " + Icebox[i];
                builder.WithDescription(supply);
                builder.WithColor(Color.Blue);
                await ReplyAsync(embed: builder.Build());
            }

            public List<string> ASupply = new List<string>
            {
            "Shamshir", "Hatchet", "Sickle", "Pickaxe", "Scissors", "Katana", "Long Sword", "Knife", "Kitchen Knife", "Razor", "Box Cutter", "Long Spear",
            "Harpoon", "Short Spear", "Fountain Pen", "Needle", "Claw Hammer", "Nightstick", "Chain Rod", "Tonfa", "Long Rod", "Flag Pole", "Stone Axe",
            "Hammer", "Bat", "Whip", "Steel Chain", "Steel Pipe", "Bamboo", "Frying Pan", "Spear Handle", "Short Rod", "Scrolls of DongYi", "Branch",
            "Ripped Scroll - 2", "Anatomy Model", "Scarf", "Ripped Scroll - 1", "Fan", "Dart", "Playing Cards", "Stallion Medal", "Throwing Dagger", "Cooking Pot",
            "Stone", "Iron Ball", "Gemstone", "Baseball Set", "Mudfish", "Carp", "Plastic Botlte", "Can", "White Chalk", "Glass Marble", "Glass Bottle",
            "Plate", "Glass Cup", "Colt Python", "IM-10", "SGOTY870", "F92S", "V-PP", "Northern Revolver", "Southern Revolver", "Long Rifle", "Long Crossbow",
            "Pellet Bow", "Wooden Bow", "Composite Bow", "Heavy Crossbow", "Short Crossbow", "Bow", "Yumi Bow", "Iron Knuckle", "Claw", "Knuckle", "Cotton Work Glove",
            "Piano Wire", "Bullets", "Arrows"
            };

            private List<string> DSupply = new List<string>
            {
            "Motorcycle Helmet","Safety Helmet","Fire Helmet","Hairband","Hiking Hat","Glasses","Monk's Robe","Bunker Jacket","Fabric Armor",
            "Full Body Swimsuit","Windbreaker","Doctor's Gown","Cassock","Wrist Band","Bracer","Arm Warmers","Watch","Reserve Armband","Bracelet",
            "Thimble","Feather Boots","Boots","Hiking Shoes","Wooden Shoes","Tights","Running Shoes","Sniping Scope","Belt","Doll","Quiver",
            "Magazine","Ribbon","Flower","Binoculars","Cross"
            };

            private List<string> Icebox = new List<string>
            {
            "Grilled Eel","Herb Medicine","Fresh Sashimi","Holy Water","Gimbap","Acupuncture","Turtle Soup","Herb","First Aid Kit","Honey Medicine",
            "Scorched Rice Soup","Oriental Concoction","Pill","Bandage","Bird Meat","Chocolate","Cookie","Antiseptic","Band-Aid","Ramen","Orange",
            "Bird Egg","Potato","Garlic","Tuna","Burdock","Oriental Grass","Soy Sauce","Curry Powder","Sweet Potato","Flower Liquor","Whiskey",
            "Herbal Liquor","Oriental Raisin Tea","Soju","Honey Water","Kaoliang Liquor","1.5L Water Bottle","Ice Water","Bacchus","Honey",
            "Liver Supplement Pill","Coffee","Tobacco Leaves","Carbonated Water","Sports Drink","Ice","Whetstone","Nail","Silencer","Gold",
            "Glass Pieces","Flint","Blueprint","Iron Sheet","Ash","Electronic Parts","Oilcloth","Turtle Shell","Steel","Ruby","Laser Pointer",
            "Thick Paper","Feather","Lighter","Boiling Water","Scrap Metal","Rubber","Leather","Wire","Iron Ore","Oil","Gunpowder","Glue",
            "Fertilizer","Battery","Cloth","Alcohol","Box"
            };
        }






    }





}