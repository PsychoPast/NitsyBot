using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System;
using System.Timers;

namespace NitsyBot.AutoGetters
{
    public class AutoFortniteUpdate
    {
        public static string fnstatus;

        public static void Timer3()
        {
            using Timer timer = new Timer
            {
                Interval = 70000
            };
            timer.Elapsed += Timer_Elapsed3;
            timer.Start();
        }

        public static async void Timer_Elapsed3(object sender, System.Timers.ElapsedEventArgs e)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            try
            {
                dynamic versionum = JsonConvert.DeserializeObject(await SingletonClass.Instance.httpClient.GetStringAsync(URLs.fnversionUrl));
                string changed = versionum.currentFortniteVersionNumber;
                if (!changed.Equals(BotSetup.version))
                {
                    BotSetup.version = changed;
                }
                else
                {
                }
                fnstatus = versionum.currentFortniteVersion;

                if (!fnstatus.Equals(BotSetup.fnversion))
                {
                    var fnversion1 = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).WithTitle("Mise à jour Fortnite").AddField("De la version", "**" + BotSetup.fnversion.Replace("++Fortnite+Release-", "").Replace("-Windows", "") + "**", false).AddField("A la version", "**" + fnstatus.Replace("++Fortnite+Release-", "").Replace("-Windows", "") + "**", false).WithColor(Color.LightOrange);
                    var fnversion2 = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).WithTitle("Fortnite Update").AddField("From the version", "**" + BotSetup.fnversion.Replace("++Fortnite+Release-", "").Replace("-Windows", "") + "**", false).AddField("To the version", "**" + fnstatus.Replace("++Fortnite+Release-", "").Replace("-Windows", "") + "**", false).WithColor(Color.LightOrange);
                    foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                    {
                        try
                        {
                            var clientmongo = new MongoClient(DataBaseCredentials.credentials);
                            var database = clientmongo.GetDatabase("StatusChannel");
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
                                                await channel.SendMessageAsync("", false, fnversion2.Build());
                                            }
                                            else if (lang == "fr")
                                            {
                                                await channel.SendMessageAsync("", false, fnversion1.Build());
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

                    BotSetup.fnversion = fnstatus;
                }
                else if (fnstatus.Equals(BotSetup.fnversion))
                {
                }
            }
            catch
            {
            }
        }
    }
}