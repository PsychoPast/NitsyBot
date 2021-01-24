using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System;
using System.Net;

namespace NitsyBot.AutoGetters
{
    public class AutoPatchNote
    {
        public static void Timer1()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 9000;
            timer.Elapsed += Timer_Elapsed1;
            timer.Start();
        }

        public static async void Timer_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            try
            {
                string here = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.patchnoteUrl);
                if (!BotSetup.patchnote.Equals(here))
                {
                    BotSetup.patchnote = here;
                    WebRequest req = WebRequest.Create(here);
                    WebResponse res = req.GetResponse();

                    var patchnotebuild = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).AddField("Patch Note de la version " + BotSetup.version + " de Fortnite", BotSetup.patchnote, false).WithColor(Color.Gold);
                    var patchnotebuilden = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).AddField("Patch Note of the version " + BotSetup.version + " of Fortnite", BotSetup.patchnote.Replace("/fr/", "/en-US/"), false).WithColor(Color.Gold);
                    foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                    {
                        try
                        {
                            var clientmongo = new MongoClient(DataBaseCredentials.credentials);
                            var database = clientmongo.GetDatabase("PatchNoteChannel");
                            var collection = database.GetCollection<BsonDocument>(guild.Id.ToString());
                            var database1 = clientmongo.GetDatabase("Language");
                            var collection1 = database1.GetCollection<BsonDocument>(guild.Id.ToString());
                            var cursor = await collection.Find(new BsonDocument()).ToCursorAsync();
                            var cursor1 = await collection1.Find(new BsonDocument()).ToCursorAsync();
                            while (await cursor.MoveNextAsync())
                            {
                                foreach (var doc in cursor.Current)
                                {
                                    string longg = doc["channel"].AsString;
                                    ulong stachan = Convert.ToUInt64(longg);
                                    var channel = SingletonClass.Instance.client.GetChannel(stachan) as SocketTextChannel;
                                    while (await cursor1.MoveNextAsync())
                                    {
                                        foreach (var doc1 in cursor1.Current)
                                        {
                                            string lang = doc1["lang"].AsString;
                                            if (lang == "en")
                                            {
                                                await channel.SendMessageAsync("", false, patchnotebuilden.Build());
                                            }
                                            else if (lang == "fr")
                                            {
                                                await channel.SendMessageAsync("", false, patchnotebuild.Build());
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
                }
                else if (BotSetup.patchnote.Equals(here))
                {
                }
            }
            catch
            {
            }
        }
    }
}