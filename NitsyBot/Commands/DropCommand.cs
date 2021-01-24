using Discord;
using Discord.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class DropCommand : ModuleBase<SocketCommandContext>
    {
        private static readonly string[] _name = {
            "https://imgur.com/mCmYRfW.png",    //JUNK JUNCTION
            "https://imgur.com/peS27bZ.png",    //HAUNTED HILLS
            "https://imgur.com/FgoKH9s.png",    //THE BLOCK
            "https://imgur.com/4ehWwie.png",    //PLEASANT PARK
            "https://imgur.com/DzNuY6S.png",    //SNOBBY SHORES
            "https://imgur.com/K5S0CC5.png",    //FROSTY FLIGHTS
            "https://imgur.com/X4twot7g.png",
            "https://imgur.com/mDTuxTT.png",    //HAPPY HAMLET
            "https://imgur.com/9tZL1bk.png",    //SHIFTY SHAFTS
            "https://imgur.com/VxmqIKG.png",    //TILTED TOWN
            "https://imgur.com/QytiFFz.png",    //LOOT LAKE
            "https://imgur.com/8rVCiZ5.png",
            "https://imgur.com/Gqqhr5n.png",    //DUSTY DEPOT
            "https://imgur.com/1vKgPoy.png",    //SALTY SPRINGS
            "https://imgur.com/BPEuRIN.png",    //FATAL FIELDS
            "https://imgur.com/IJWozjQ.png",    //LAZY LAGOON
            "https://imgur.com/jT6GLTP.png",    //PRESSURE PLANT
            "https://imgur.com/KFV2bfE.png",    //RETAIL ROW
            "https://imgur.com/9jZeZW1.png",    //SUNNY STEPS
            "https://imgur.com/D8Rvsru.png",    //LONELY LODGE
            "https://imgur.com/2xHLeoz.png",    //PARADISE PALMS
            "https://imgur.com/fq89cXX.png",    //LUCKY LANDING
            "https://imgur.com/J6RxhJX.png",
            "https://imgur.com/AZWMd2c.png",    //VIKING VILLAGE
            "https://imgur.com/bUuwYT9.png",
            "https://imgur.com/C4kY69a.png",
            "https://imgur.com/mALmmIx.png"
        };

        [Command("drop")]
        [Summary("Choose a place on the Fortnite Map to drop.")]
        public async Task Drop()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            int city = new Random().Next(_name.Length);
            EmbedBuilder drop = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {ownerinfos[0]}",
                    IconUrl = ownerinfos[1]
                },
                ImageUrl = _name[city],
                Color = new NitsyBotUtilities().GeneratedColor
            };
            try
            {
                DataBaseSetup database = new DataBaseSetup();
                IAsyncCursor<BsonDocument> cursorLang = null;// await database.DataBaseLang(Context.Guild.Id.ToString());

                while (await cursorLang.MoveNextAsync())
                {
                    foreach (BsonDocument doc1 in cursorLang.Current)
                    {
                        string lang = doc1["lang"].AsString;
                        if (lang == "en")
                        {
                            drop.WithTitle("Nitsy says to land at");
                            await ReplyAsync("", false, drop.Build());
                        }
                        else if (lang == "fr")
                        {
                            drop.WithTitle("Nitsy dit de land à");
                            await ReplyAsync("", false, drop.Build());
                        }
                        else
                            await ReplyAsync("I couldn't post.Check that you set correctly the language to fr or en.");
                    }
                }
            }
            catch
            {
                await ReplyAsync("I couldn't post.Check that you set correctly the language to fr or en.");
            }
        }
    }
}