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

        //-craft command
        [Command("craft")]
        public async Task CraftAsync([Remainder] string name)
        {
            Dal.DoConnection();
            EmbedBuilder builder = new EmbedBuilder();
            int itemID = Dal.GetIDItem(name);

            if (itemID != -1)
            {
                Item itemAsked = Dal.GetItem(itemID, true);
                builder.AddField($"To find/craft : {itemAsked.Name}", $"Desc : { itemAsked.Description}");

                switch (itemAsked.ID)
                {
                    case (226): // Dart of Blood
                        builder.AddField($"To craft {itemAsked.Name}", "Dart of Souls + Stingburst");
                        builder.WithColor(Color.DarkRed);
                        await ReplyAsync("", false, builder.Build());
                        BuildItem("Dart of Souls", builder = new EmbedBuilder());

                        builder = new EmbedBuilder();
                        Item item = Dal.GetItem(Dal.GetIDItem("Stingburst"), true);
                        item.DisplayItem(builder, true);
                        break;
                    case (609): // Network PC
                        builder.AddField("To craft Network PC", "Cell Phone + Laptop");
                        builder.WithColor(Color.DarkRed);
                        await ReplyAsync("", false, builder.Build());
                        BuildItem("Cellphone", builder);

                        builder = new EmbedBuilder();
                        builder.AddField("To craft Laptop", "Ion battery + Laptop (dead battery)");

                        BuildItem("ion battery", builder);

                        builder = new EmbedBuilder();
                        item = Dal.GetItem(Dal.GetIDItem("Laptop (dead battery)"), true);
                        item.DisplayItem(builder, true);
                        break;
                    case (608): // Laptop
                        builder.AddField("To craft Laptop", "Ion battery + Laptop (dead battery)");
                        builder.WithColor(Color.DarkRed);
                        await ReplyAsync("", false, builder.Build());
                        BuildItem("Ion Battery", builder = new EmbedBuilder());

                        builder = new EmbedBuilder();
                        item = Dal.GetItem(Dal.GetIDItem("Laptop (dead battery)"), true);
                        item.DisplayItem(builder, true);
                        break;
                    default:
                        itemAsked.DisplayItem(builder, true);
                        break;
                }
                builder.WithFooter(DoFooterDesc(itemAsked));

            }
            else
            {
                builder.WithTitle("Check the name of your item.");
            }

            Dal.CloseConnection();
            builder.WithColor(Color.DarkRed);

            //This is the function sending builder (making great visual stuff)
            await ReplyAsync("", false, builder.Build());
        }


        //-where command
        [Command("item")]
        public async Task WhereAsync([Remainder] string name)
        {
            Dal.DoConnection();
            EmbedBuilder builder = new EmbedBuilder();
            int idItem = Dal.GetIDItem(name);
            Item itemAsked = null;

            if (idItem != -1)
            {
                itemAsked = Dal.GetItem(idItem, false);
                itemAsked.DisplayOnlyLocationItem(builder);
                if (itemAsked.Description != "")
                    builder.AddField($"Description :", $"- {itemAsked.Description}");

                builder.WithFooter(DoFooterDesc(itemAsked));
            }
            else
            {
                builder.WithTitle("Check the name of your item.");
            }

            Dal.CloseConnection();
            builder.WithColor(Color.DarkBlue);


            await ReplyAsync("", false, builder.Build());
        }

        //-what command
        [Command("what")]
        public async Task WhatAsync([Remainder] string name)
        {
            Dal.DoConnection();
            EmbedBuilder builder = new EmbedBuilder();

            Location locationAsked = Dal.GetLocation(name);

            if (locationAsked != null)
                locationAsked.DisplayItemsLocation(builder);
            else
                builder.WithTitle("Check the name of your location.");

            Dal.CloseConnection();
            builder.WithColor(Color.DarkGreen);
            await ReplyAsync("", false, builder.Build());
        }

        [Command("addslang"), RequireOwner]
        public async Task AddSlangAsync(string idItem, [Remainder] string slang)
        {
            if (idItem.Length > 9)
            {
                await ReplyAsync($"What are you trying to do ?", false);
            }
            else
            {
                Dal.DoConnection();
                Item item = Dal.GetItem(int.Parse(idItem), false);
                if (item != null)
                {
                    Dal.AddSlang(item.ID, slang);
                    await ReplyAsync($"Slang {slang} added for {item.Name}", false);
                }
                else
                    await ReplyAsync($"No item found to add a slang.", false);
                Dal.CloseConnection();
            }


        }




        private string DoFooterDesc(Item itemAsked)
        {
            string footer = "ID " + itemAsked.ID + " ";

            if (itemAsked.ID == 77)
                footer += "/ Kayla haven't done this blade once";
            if (itemAsked.ID == 261)
                footer += "/ Arrcival's easiest item to find";
            if (itemAsked.ID == 330)
                footer += "/ Kazu's favorite bow";

            return footer;
        }

        public async void BuildItem(string name, EmbedBuilder builder)
        {
            int idItem = Dal.GetIDItem(name);
            if (idItem != -1)
            {
                Item item = Dal.GetItem(idItem, false);
                item.DisplayItem(builder, true);
                builder.WithColor(Color.DarkRed);
                await ReplyAsync("", false, builder.Build());
            }
        }
    }
}
