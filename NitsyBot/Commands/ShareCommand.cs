using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Checkers;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class ShareCommand : ModuleBase<SocketCommandContext>
    {
        const string url = "https://nitsy.home.blog/changements";
        [Command("share")]
        [RequireOwner]
        public async Task Share()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            GetNitsyBotVersion nitsy = new GetNitsyBotVersion();
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithColor(Color.DarkMagenta);
            IMongoCollection<BsonDocument> list = DataBaseSetup.DataBaseAccess(DataBases.channelsDataBase, DataBases.nitsyCollection);
            List<BsonDocument> guildlist = await (await list.FindAsync(new BsonDocument())).ToListAsync();
            foreach (BsonDocument doc in guildlist)
            {
                string channel = doc["id"].AsString;
                string language = GetGuildLanguage.languages[channel];
                if (language == "fr")
                {
                    embedBuilder
                        .WithTitle($"Mise à jour {nitsy.NitsyVersion}")
                        .AddField("Changements", $"Pour voir les nouveautés de cette mise à jour va sur {url}", false);
                }
                else
                {
                    embedBuilder
                        .WithTitle($"Update {nitsy.NitsyVersion}")
                        .AddField("Changes", $"To see what are the changes of this update , go to {url}", false);
                }
                await (SingletonClass.Instance.client.GetChannel(ulong.Parse(channel)) as SocketTextChannel).SendMessageAsync("", false, embedBuilder.Build());
            }
        }
    }
}