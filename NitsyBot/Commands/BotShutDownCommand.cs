using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.DataBase;
using System;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class BotShutDownCommand : ModuleBase<SocketCommandContext>
    {
        private string _reas { get; set; }
        private string _reasFr { get; set; }
        [RequireOwner]
        [Command("reason")]
        public async Task Reason([Remainder]string reason = "Undefined")
        {
            if (Context.User.Id == (ulong)IDS.OwnerID)
            {
                _reas = reason;
                await ReplyAsync("Reason set!(english)");
            }
        }
        [Command("reasonfr")]
        public async Task ReasonFR([Remainder]string reasonfr = "Indéfini")
        {
            if (Context.User.Id == (ulong)IDS.OwnerID)
            {
                _reasFr = reasonfr;
                await ReplyAsync("Reason set!(french)");
            }
        }
        [Command("shutdown")]
        [Summary("Shutdown Nitsy.")]
        public async Task ShutDown()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            if (Context.User.Id == (ulong)(ulong)IDS.OwnerID)
            {
                foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                {
                    DataBaseSetup db = new DataBaseSetup();
                    IAsyncCursor<BsonDocument> cursorLang = null;// await db.DataBaseLang(guild.Id.ToString());
                    IAsyncCursor<BsonDocument> cursorDB = null;// db.DataBaseAccess("NitsyChannel", guild.Id.ToString());
                    EmbedBuilder shutdownen = new EmbedBuilder()
                        .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                        .WithTitle("Nitsy")
                        .AddField("Maintenance Mode **ON**", "Sorry, I need to go offline for maintenance :(", false).AddField("Reason", _reas, false)
                        .WithColor(Color.Red);
                    EmbedBuilder shutdownfr = new EmbedBuilder()
                        .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                        .WithTitle("Nitsy").AddField("Maintenance Mode **ON**", "Désolé, je dois entrer en maintenance :(", false)
                        .AddField("Raison", _reasFr, false).WithColor(Color.Red);
                    try
                    {
                        while (await cursorLang.MoveNextAsync())
                        {
                            foreach (var doc in cursorLang.Current)
                            {
                                string longg = doc["channel"].AsString;

                                ulong stachan = Convert.ToUInt64(longg);
                                SocketTextChannel channel = SingletonClass.Instance.client.GetChannel(stachan) as SocketTextChannel;
                                while (await cursorDB.MoveNextAsync())
                                {
                                    foreach (var doc1 in cursorDB.Current)
                                    {
                                        string lang = doc1["lang"].AsString;
                                        if (lang == "en" || string.IsNullOrEmpty(lang))
                                        {
                                            await channel.SendMessageAsync("", false, shutdownen.Build());
                                        }
                                        else if (lang == "fr")
                                        {
                                            await channel.SendMessageAsync("", false, shutdownfr.Build());
                                        }
                                    }
                                }

                            }
                        }
                    }
                    catch
                    {

                    }
                }
                await SingletonClass.Instance.client.LogoutAsync();
                await SingletonClass.Instance.client.StopAsync();
            }
            else
            {
                await ReplyAsync("T'es pas mon père pour me dire de shutdown!");
            }
        }
    }
}