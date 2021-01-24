using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.DataBase;
using System;
using System.Timers;
namespace NitsyBot.AutoGetters
{
    public class AutoStatus
    {
        private DataBaseSetup databaseSetup;
        IAsyncCursor<BsonDocument> cursorLang;
        IAsyncCursor<BsonDocument> cursorDB;
        public void Timer()
        {
            Timer timer = new Timer
            {
                Interval = 15000
            };
            timer.Elapsed += GetStatus;
            timer.Start();
        }
        public async void GetStatus(object sender, ElapsedEventArgs e)
        {
            databaseSetup = new DataBaseSetup();
            string[] ownerinfos = GetOwnerInfos.Infos();
            AutoStatus autoStatus = new AutoStatus();
            DateTime thistime = DateTime.Now;
            string getStatus = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.statusUrl);
            dynamic status = JsonConvert.DeserializeObject(getStatus);
            string statusvalue = status[0].status;
            try
            {
                if (BotSetup.status == "UP" && statusvalue == "DOWN")
                {
                    BotSetup.status = "DOWN";
                    EmbedBuilder frenchembed = new EmbedBuilder()
                        .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                        .AddField("Status des serveurs de Fortnite", "Les serveurs de Fortnite sont **en maintenance** le " + thistime.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:ss").Replace("2019", "2019 à "), false)
                        .WithColor(Color.Red);
                    EmbedBuilder englishembed = new EmbedBuilder()
                        .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                        .AddField("Fortnite servers status", "The Fortnite servers are **down** on " + thistime.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:ss").Replace("2019", "2019 at "), false)
                        .WithColor(Color.Red);
                    autoStatus.SendStatus(englishembed, frenchembed, databaseSetup);
                }
                else if (BotSetup.status == "Down" && statusvalue == "DOWN")
                {

                }
                else if (BotSetup.status == "DOWN" && statusvalue == "UP")
                {
                    BotSetup.status = "UP";
                    EmbedBuilder frenchemebed = new EmbedBuilder()
                        .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                        .AddField("Status des serveurs de Fortnite", "Les serveurs de Fortnite sont de nouveau **en ligne** le " + DateTime.Now.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:ss").Replace("2019", "2019 à "), false)
                        .WithColor(Color.Green);
                    EmbedBuilder englishemebed = new EmbedBuilder()
                        .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                        .AddField("Fortnite servers status", "The Fortnite servers are **online** on " + DateTime.Now.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:ss").Replace("2019", "2019 at "), false)
                        .WithColor(Color.Green);
                    autoStatus.SendStatus(englishemebed, frenchemebed, databaseSetup);
                }
                else if (BotSetup.status == "UP" && statusvalue == "UP")
                {

                }
            }

            catch
            {

            }
        }
        private async void SendStatus(EmbedBuilder english, EmbedBuilder french, DataBaseSetup databaseSetup)
        {
            foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
            {
                cursorLang = null;// await databaseSetup.DataBaseLang(guild.Id.ToString());
                cursorDB = null;// databaseSetup.DataBaseAccess("StatusChannel", guild.Id.ToString());
                try
                {
                    while (await cursorLang.MoveNextAsync())
                    {
                        foreach (BsonDocument channelDoc in cursorLang.Current)
                        {
                            string channelId = channelDoc["channel"].AsString;
                            ulong uchannelId = Convert.ToUInt64(channelId);
                            SocketTextChannel channel = SingletonClass.Instance.client.GetChannel(uchannelId) as SocketTextChannel;
                            while (await cursorDB.MoveNextAsync())
                            {
                                foreach (BsonDocument langDoc in cursorDB.Current)
                                {
                                    string lang = langDoc["lang"].AsString;
                                    if (lang == "en" || string.IsNullOrEmpty(lang))
                                    {
                                        await channel.SendMessageAsync("", false, english.Build());
                                    }
                                    else if (lang == "fr")
                                    {
                                        await channel.SendMessageAsync("", false, french.Build());
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
}
