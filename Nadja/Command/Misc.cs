using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadja.Command
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        //Random one shot command (miscellnaoeus)

        [Command("hello")]
        public async Task HelloAsync()
        {
            await ReplyAsync("world!");
        }


        [Command("hey")]
        public async Task HeyAsync()
        {
            if (Context.User.Id == 326925427404832769)
            {
                await ReplyAsync($"Yes dark lord {Context.User.Mention}?");
            }
            else
            {
                await ReplyAsync($"I don't want to talk with you {Context.User.Mention}");
            }
        }

        [Command("slap")]
        public async Task SlapAsync()
        {
            await ReplyAsync("https://cdn.discordapp.com/attachments/384856749833715732/413346110023794699/lazu_slap.PNG");
        }

        [Command("nadja")]
        public async Task NadjaAsync()
        {
            await ReplyAsync("It's me, what do you want ?");
        }

        [Command("noctus")]
        public async Task NoctusAsync()
        {
            await ReplyAsync("Noctus waifu : https://cdn.discordapp.com/attachments/384924672308936705/405879171265593345/20180124_184859.jpg");
        }

        [Command("kazu")]
        public async Task KazuAsync()
        {
            await ReplyAsync("Kazu waifu : https://cdn.discordapp.com/attachments/384924672308936705/405878465317830666/IMG_20180124_184209.jpg");
        }

        [Command("cheese")]
        public async Task CheeseAsync()
        {
            await ReplyAsync(":cheese: :cheese: :cheese: :cheese: :cheese: :cheese: :cheese: :cheese: :cheese: :cheese: :cheese: :cheese:");
        }

        [Command("sheep")]
        public async Task SheepAsync()
        {
            await ReplyAsync("https://cdn.discordapp.com/attachments/386599542994239492/405879924071727114/jackie.png");
        }

        [Command("monika")]
        public async Task MonikaAsync()
        {
            await ReplyAsync("Just Monika");
        }

        [Command("arrchival")]
        public async Task ArrchivalAsync()
        {
            await ReplyAsync("https://www.google.com/search?q=how+to+read&ei=lyNpWtzcOomLgAbprrroBA&start=0&sa=N&biw=1920&bih=974");
        }

        [Command("french")]
        public async Task FrenchAsync()
        {
            await ReplyAsync(":flag_fr: :french_bread: :french_bread: VIVE :french_bread: LES :french_bread: BAGUETTES :french_bread: :french_bread: :flag_fr: ");
        }
        
        [Command("cupcake")]
        public async Task CupcakeAsync()
        {
            if (Context.User.Id != 107336587049713664)
            {
                await ReplyAsync("Hey, have you seen Cupcake purity? Me neither.");
            }
            else
            {
                await ReplyAsync("You devil.");
            }
        }

        [Command("trap")]
        public async Task TrapAsync()
        {
            if (Context.User.Id != 349333503583977473)
            {
                await ReplyAsync("Watch out, wraith is a trap.");
            }
            else
            {
                await ReplyAsync("Hello trap.");
            }
        }

        [Command("shitpost")]
        public async Task ShitpostAsync()
        {
            if (Context.User.Id != 288965819315257345)
            {
                await ReplyAsync("Dark is shitposter number one");
            }
            else
            {
                await ReplyAsync("Hello shitposter.");
            }
        }
        
        [Command("easter")]
        public async Task EasterAsync()
        {
            await ReplyAsync(":egg:");
        }

        [Command("wait")]
        public async Task WaitAsync()
        {
            await ReplyAsync("Ok I wait.");
        }

        [Command("pawfive")]
        public async Task PawfiveAsync()
        {
            Random rng = new Random();
            int number = rng.Next(6);
            switch (number)
            {
                case (0):
                    {
                        await ReplyAsync("https://cdn.discordapp.com/attachments/380456463396765696/387649656676679682/1fbe60df1019d22deae8bfe72f5c8eea--the-high-high-five.png");
                        break;
                    }
                case (1):
                    {
                        await ReplyAsync("https://cdn.discordapp.com/attachments/380456463396765696/387649926940721152/kitten-high-five-edited.png");
                        break;
                    }
                case (2):
                    {
                        await ReplyAsync("https://media.giphy.com/media/iARAzK9t0V6p2/giphy.gif");
                        break;
                    }
                case (3):
                    {
                        await ReplyAsync("https://cdn.discordapp.com/attachments/358411044856659969/387652254230118400/JPEG_20171019_194418.jpg");
                        break;
                    }
                case (4):
                    {
                        await ReplyAsync("https://cdn.discordapp.com/attachments/380456463396765696/387652928552566784/June-22-2012-17-52-51-kijjy.png");
                        break;
                    }
                case (5):
                    {
                        await ReplyAsync("https://cdn.discordapp.com/attachments/380456463396765696/387653159205732352/hight-five.png");
                        break;
                    }

                default:
                    {
                        await ReplyAsync("No pawfive for you");
                        break;
                    }
            }
        }


        [Command("bellyrub")]
        public async Task BellyrubAsync()
        {
            Random rng = new Random();
            List<string> links = new List<string>()
            {
                "https://cdn.discordapp.com/attachments/380456463396765696/387654223506046977/Im-ready-Rub-the-belly.png",
                "https://cdn.discordapp.com/attachments/380456463396765696/387654799321202698/belly-rub-animals-45__605.png",
                "https://cdn.discordapp.com/attachments/380456463396765696/387655883087282176/y2rYleP.png",
                "https://cdn.discordapp.com/attachments/380456463396765696/387656139740938241/tummy-rubs-for-the-spikey-boi-28319965.png",
                "https://media.giphy.com/media/NaITrcf0yQE0w/giphy.gif",
                "https://cdn.discordapp.com/attachments/380456463396765696/387657314359967775/3F090ED500000578-4389528-image-a-1_1491562337782.png",
                "https://cdn.discordapp.com/attachments/380456463396765696/387657682204622859/8a7778f451709473decfe230cadba790.png",
                "https://cdn.discordapp.com/attachments/380456463396765696/387657870738718722/ZEBRA_SHARK_GETS_BELLY_RUB_v3_1280x720_12398661535.png",
                "https://static.boredpanda.com/blog/wp-content/uploads/2015/08/seal-belly-rub-diver-gary-grayson-6.gif"
            };
            await ReplyAsync(links[rng.Next(links.Count)]);
            
        }

        [Command("hug")]
        public async Task HugAsync()
        {
            Random rng = new Random();
            List<string> links = new List<string>()
            {
                "https://tenor.com/view/hug-hugs-ghost-hug-its-there-gif-4951192",
                "https://media.giphy.com/media/l4FGpP4lxGGgK5CBW/giphy.gif",
                "https://tenor.com/view/loading-hug-smile-love-virtual-hug-gif-5512591",
                "https://media.giphy.com/media/jMGxhWR7rtTNu/giphy.gif",

                "https://media.giphy.com/media/INUsrrxQW4et2/giphy.gif",

                "https://tenor.com/view/hug-gif-8935008https://tenor.com/view/hug-gif-8935008",
                "https://cdn.discordapp.com/attachments/380456463396765696/387657682204622859/8a7778f451709473decfe230cadba790.png",
                "https://cdn.discordapp.com/attachments/380456463396765696/387657870738718722/ZEBRA_SHARK_GETS_BELLY_RUB_v3_1280x720_12398661535.png",
                "https://static.boredpanda.com/blog/wp-content/uploads/2015/08/seal-belly-rub-diver-gary-grayson-6.gif"
            };
            await ReplyAsync(links[rng.Next(links.Count)]);
            
        }


        [Command("think")]
        public async Task ThinkAsync()
        {
            await ReplyAsync(":thinking: I AM THINKING :thinking:");
        }

        [Command("lewd")]
        public async Task LewdAsync()
        {
            await ReplyAsync("Don't talk to me, lewd.");
        }

        [Command("ban")]
        public async Task BanAsync()
        {
            await ReplyAsync("If I have ban permission, I can ban everyone excluding you. Type -yes to continue");
        }

        [Command("yes")]
        public async Task YesAsync()
        {
            await ReplyAsync("PRANKED!");
        }

        [Command("no")]
        public async Task NoAsync()
        {
            await ReplyAsync("Sorry, do you mean -yes?");
        }

        [Command("god")]
        public async Task GodAsync()
        {
            if (Context.User.Id != 88011881498812416)
                await ReplyAsync("Sorry, do you mean Arrcival?");
            else
                await ReplyAsync("Yes, my lord?");
        }

        [Command("secret")]
        public async Task SecretAsync()
        {
            await ReplyAsync("Is this really a secret command? Maybe a dumb one.");
        }
        
        
        [Command("bot")]
        public async Task BotAsync()
        {
            await ReplyAsync(":robot:");
        }

        [Command("mine")]
        public async Task MineAsync()
        {
            if (Context.User.Id == 108037603890155520)
            {
                await ReplyAsync("Arrcival is yours.");
            }
        }

        [Command("java")]
        public async Task PandalanAsync()
        {
            await ReplyAsync("Pandalan is good with java \n:kek:");
        }

        [Command("sex")]
        public async Task SexAsync()
        {
            await ReplyAsync("https://cdn.discordapp.com/attachments/384847476320108556/406409749517828097/IMG_20180126_193654_618.jpg");
        }

        [Command("blame")]
        public async Task BlameAsync()
        {
            if (Context.User.Id != 132574039402217472)
            {
                await ReplyAsync("***BLAME PANDA***");
            }
            else
            {
                await ReplyAsync($"Why do you want to blame yourself {Context.User.Mention}");
            }
        }

        [Command("turtle")]
        public async Task TurtleAsync()
        {
            await ReplyAsync("https://cdn.discordapp.com/attachments/385134869245853707/387353276489924612/9k.png");
        }
        
        /*
        [Group("meme")]
        public class Meme : ModuleBase<SocketCommandContext>
        {
            // Default thing to display it nothing is inputed after -meme
            [Command]
            public async Task MemeDefaultAsync()
            {
                await ReplyAsync(":japanese_goblin:");
            }

            // -meme jp
            [Command("jp")]
            public async Task MemeJPAsync()
            {
                await ReplyAsync("JP is not hackerman.");
            }
        }*/
    }
}
