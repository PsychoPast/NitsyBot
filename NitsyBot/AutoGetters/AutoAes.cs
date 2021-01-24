using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System;
using System.Timers;

namespace NitsyBot.AutoGetters
{
    public static class AutoAes
    {
        public static void Timer2()
        {
            using Timer timer = new Timer()
            {
                Interval = 70000
            };
            timer.Elapsed += Timer_Elapsed2;
            timer.Start();
        }

        public static async void Timer_Elapsed2(object sender, System.Timers.ElapsedEventArgs e)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            try
            {
                string aes12 = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.aesUrl);
                if (!aes12.Equals(BotSetup.aeskeys))
                {
                    BotSetup.aeskeys = aes12;
                    dynamic result = JsonConvert.DeserializeObject(aes12);
                    JToken additionalKeys = result["additionalKeys"];
                    if (additionalKeys != null)
                    {
                        string main = result.mainKey;
                        var aes = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).WithTitle("Les clés aes pour la version " + BotSetup.version + " de Fortnite:").AddField("La clé principale:", main, false).WithColor(Color.DarkBlue);
                        var aesen = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).WithTitle("The aes keys for the version " + BotSetup.version + " of Fortnite:").AddField("The main key:", main, false).WithColor(Color.DarkBlue);
                        JToken pakmille = additionalKeys["pakchunk1000-WindowsClient.pak"];
                        if (pakmille != null)
                        {
                            string pak1000 = pakmille.Value<string>();
                            aes.AddField("Pak1000: ", pak1000, false);
                            aesen.AddField("Pak1000: ", pak1000, false);
                        }
                        JToken pakmille1 = additionalKeys["pakchunk1001-WindowsClient.pak"];
                        if (pakmille1 != null)
                        {
                            string pak1001 = pakmille1.Value<string>();
                            aes.AddField("Pak1001: ", pak1001, false);
                            aesen.AddField("Pak1001: ", pak1001, false);
                        }
                        JToken pakmille2 = additionalKeys["pakchunk1002-WindowsClient.pak"];
                        if (pakmille2 != null)
                        {
                            string pak1002 = pakmille2.Value<string>();
                            aes.AddField("Pak1002: ", pak1002, false);
                            aesen.AddField("Pak1002: ", pak1002, false);
                        }
                        JToken pakmille3 = additionalKeys["pakchunk1003-WindowsClient.pak"];
                        if (pakmille3 != null)
                        {
                            string pak1003 = pakmille3.Value<string>();
                            aes.AddField("Pak1003: ", pak1003, false);
                            aesen.AddField("Pak1003: ", pak1003, false);
                        }
                        JToken pakmille4 = additionalKeys["pakchunk1004-WindowsClient.pak"];
                        if (pakmille4 != null)
                        {
                            string pak1004 = pakmille4.Value<string>();
                            aes.AddField("Pak1004: ", pak1004, false);
                            aesen.AddField("Pak1004: ", pak1004, false);
                        }
                        JToken pakmille5 = additionalKeys["pakchunk1005-WindowsClient.pak"];
                        if (pakmille5 != null)
                        {
                            string pak1005 = pakmille5.Value<string>();
                            aes.AddField("Pak1005: ", pak1005, false);
                            aesen.AddField("Pak1005: ", pak1005, false);
                        }
                        JToken pakmille6 = additionalKeys["pakchunk1006-WindowsClient.pak"];
                        if (pakmille6 != null)
                        {
                            string pak1006 = pakmille6.Value<string>();
                            aes.AddField("Pak1006: ", pak1006, false);
                            aesen.AddField("Pak1006: ", pak1006, false);
                        }
                        JToken pakmille7 = additionalKeys["pakchunk1007-WindowsClient.pak"];
                        if (pakmille7 != null)
                        {
                            string pak1007 = pakmille7.Value<string>();
                            aes.AddField("Pak1007: ", pak1007, false);
                            aesen.AddField("Pak1007: ", pak1007, false);
                        }
                        JToken pakmille8 = additionalKeys["pakchunk1008-WindowsClient.pak"];
                        if (pakmille8 != null)
                        {
                            string pak1008 = pakmille8.Value<string>();
                            aes.AddField("Pak1008: ", pak1008, false);
                            aesen.AddField("Pak1008: ", pak1008, false);
                        }
                        JToken pakmille9 = additionalKeys["pakchunk1009-WindowsClient.pak"];
                        if (pakmille9 != null)
                        {
                            string pak1009 = pakmille9.Value<string>();
                            aes.AddField("Pak1009: ", pak1009, false);
                            aesen.AddField("Pak1009: ", pak1009, false);
                        }
                        JToken pakmille10 = additionalKeys["pakchunk1010-WindowsClient.pak"];
                        if (pakmille10 != null)
                        {
                            string pak1010 = pakmille10.Value<string>();
                            aes.AddField("Pak1010: ", pak1010, false);
                            aesen.AddField("Pak1010: ", pak1010, false);
                        }
                        JToken pakmille11 = additionalKeys["pakchunk1011-WindowsClient.pak"];
                        if (pakmille11 != null)
                        {
                            string pak1011 = pakmille11.Value<string>();
                            aes.AddField("Pak1011: ", pak1011, false);
                            aesen.AddField("Pak1011: ", pak1011, false);
                        }
                        JToken pakmille12 = additionalKeys["pakchunk1012-WindowsClient.pak"];
                        if (pakmille12 != null)
                        {
                            string pak1012 = pakmille12.Value<string>();
                            aes.AddField("Pak1012: ", pak1012, false);
                            aesen.AddField("Pak1012: ", pak1012, false);
                        }
                        JToken pakmille13 = additionalKeys["pakchunk1013-WindowsClient.pak"];
                        if (pakmille13 != null)
                        {
                            string pak1013 = pakmille13.Value<string>();
                            aes.AddField("Pak1013: ", pak1013, false);
                            aesen.AddField("Pak1013: ", pak1013, false);
                        }
                        JToken pakmille14 = additionalKeys["pakchunk1014-WindowsClient.pak"];
                        if (pakmille14 != null)
                        {
                            string pak1014 = pakmille14.Value<string>();
                            aes.AddField("Pak1014: ", pak1014, false);
                            aesen.AddField("Pak1014: ", pak1014, false);
                        }
                        JToken pakmille15 = additionalKeys["pakchunk1015-WindowsClient.pak"];
                        if (pakmille15 != null)
                        {
                            string pak1015 = pakmille15.Value<string>();
                            aes.AddField("Pak1015: ", pak1015, false);
                            aesen.AddField("Pak1015: ", pak1015, false);
                        }
                        JToken pakmille16 = additionalKeys["pakchunk1016-WindowsClient.pak"];
                        if (pakmille16 != null)
                        {
                            string pak1016 = pakmille16.Value<string>();
                            aes.AddField("Pak1016: ", pak1016, false);
                            aesen.AddField("Pak1016: ", pak1016, false);
                        }
                        JToken pakmille17 = additionalKeys["pakchunk1017-WindowsClient.pak"];
                        if (pakmille17 != null)
                        {
                            string pak1017 = pakmille17.Value<string>();
                            aes.AddField("Pak1017: ", pak1017, false);
                            aesen.AddField("Pak1017: ", pak1017, false);
                        }
                        JToken pakmille18 = additionalKeys["pakchunk1018-WindowsClient.pak"];
                        if (pakmille18 != null)
                        {
                            string pak1018 = pakmille18.Value<string>();
                            aes.AddField("Pak1018: ", pak1018, false);
                            aesen.AddField("Pak1018: ", pak1018, false);
                        }
                        JToken pakmille19 = additionalKeys["pakchunk1019-WindowsClient.pak"];
                        if (pakmille19 != null)
                        {
                            string pak1019 = pakmille19.Value<string>();
                            aes.AddField("Pak1019: ", pak1019, false);
                            aesen.AddField("Pak1019: ", pak1019, false);
                        }
                        JToken pakmille20 = additionalKeys["pakchunk1020-WindowsClient.pak"];
                        if (pakmille20 != null)
                        {
                            string pak1020 = pakmille20.Value<string>();
                            aes.AddField("Pak1020: ", pak1020, false);
                            aesen.AddField("Pak1020: ", pak1020, false);
                        }
                        JToken pakmille21 = additionalKeys["pakchunk1021-WindowsClient.pak"];
                        if (pakmille21 != null)
                        {
                            string pak1021 = pakmille21.Value<string>();
                            aes.AddField("Pak1021: ", pak1021, false);
                            aesen.AddField("Pak1021: ", pak1021, false);
                        }
                        JToken pakmille22 = additionalKeys["pakchunk1022-WindowsClient.pak"];
                        if (pakmille22 != null)
                        {
                            string pak1022 = pakmille22.Value<string>();
                            aes.AddField("Pak1022: ", pak1022, false);
                            aesen.AddField("Pak1022: ", pak1022, false);
                        }
                        foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                        {
                            try
                            {
                                var clientmongo = new MongoClient(DataBaseCredentials.credentials);
                                var database = clientmongo.GetDatabase("AesChannel");
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
                                                    await channel.SendMessageAsync("", false, aesen.Build());
                                                }
                                                else if (lang == "fr")
                                                {
                                                    await channel.SendMessageAsync("", false, aes.Build());
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
                }
                else if (aes12.Equals(BotSetup.aeskeys))
                {
                }
            }
            catch
            {
            }
        }
    }
}