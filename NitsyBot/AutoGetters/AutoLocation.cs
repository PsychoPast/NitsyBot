using Discord;
using Discord.WebSocket;
using GetLocationn;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System;
using System.Timers;
using Tweetinvi;

namespace NitsyBot.AutoGetters
{
    public class GetAutoLocation
    {
        public static string oldLocation;

        public static void Tim()
        {
            Timer timer = new Timer(20000)
            {
                AutoReset = true
            };
            timer.Elapsed += new ElapsedEventHandler(AutoGet);
            timer.Start();
        }

        private async static void AutoGet(object sender, ElapsedEventArgs e)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string userinfos = TwitterAccessor.ExecuteGETQueryReturningJson(Twitter.TwitterUrl);
            try
            {
                string location = JsonConvert.DeserializeObject<GetLoc>(userinfos).Location;
                if (location != oldLocation)
                {
                    location = string.IsNullOrEmpty(location) ? "No location" : location;
                    oldLocation = string.IsNullOrEmpty(oldLocation) ? "No location" : oldLocation;
                    EmbedBuilder embed = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).WithTitle("Location of Donald Mustard Changed!").AddField("Old location", oldLocation, false).AddField("New Location", location, false).WithColor(new NitsyBotUtilities().GeneratedColor);
                    DataBaseSetup database = new DataBaseSetup();
                    foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                    {
                        IAsyncCursor<BsonDocument> cursorLang = null;// await database.DataBaseAccess("StatusChannel", guild.Id.ToString());
                        try
                        {
                            while (await cursorLang.MoveNextAsync())
                            {
                                foreach (BsonDocument doc in cursorLang.Current)
                                {
                                    string longg = doc["channel"].AsString;
                                    ulong stachan = Convert.ToUInt64(longg);
                                    SocketTextChannel channel = SingletonClass.Instance.client.GetChannel(stachan) as SocketTextChannel;
                                    await channel.SendMessageAsync("", false, embed.Build());
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    Tweet.PublishTweet("Location of @DonaldMustard has changed = = = = \n - His old location was: '" + oldLocation + "' \n - His new one is: '" + location + "'");
                    oldLocation = location == "No location" ? string.Empty : location;
                }
                else
                {
                }
            }
            catch
            {
            }
        }
    }
}