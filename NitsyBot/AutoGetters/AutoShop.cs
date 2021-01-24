/*using Discord;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Setup;
using NitsyBot.Core;

namespace NitsyBot.AutoGetters
{
    public class AutoShop
    {
        public void Timer4()
        {
            var DailyTime = "00:01:20";
            var timeParts = DailyTime.Split(new char[1] { ':' });

            var dateNow = DateTime.Now.ToUniversalTime();
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                       int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = date.AddDays(1);
                ts = date - dateNow;
            }

            //waits certan time and run the code
            Task.Delay(ts).ContinueWith((x) => Timer_Elapsed4());
        }
        public async void Timer_Elapsed4()
        {
            var client1 = new BotSetup();
            try
            {
                var apirequ = new ApiSetup();
                string shop1 = apirequ.Fnbrco("https://fnbr.co/api/shop");
                if (!shop1.Equals(BotSetup.shopchanged))
                {
                    BotSetup.shopchanged = shop1;
                    dynamic shop = JsonConvert.DeserializeObject(shop1);

                    for (int i = 0; i <= 10; i++)
                    {
                        try
                        {
                            string imageurl = shop.data.featured[i].images.icon;
                            string namaewa = shop.data.featured[i].name;
                            string des = shop.data.featured[i].description;
                            string pri = shop.data.featured[i].price;
                            string ty = shop.data.featured[i].readableType;
                            string rar = shop.data.featured[i].rarity;
                            string date = shop.data.date;
                            DateTime date1 = shop.data.date;
                            var test = new EmbedBuilder().WithFooter("Made by PsychoPast / Powered by FNBR.Co API", "https://cdn.awwni.me/15pqc.png").WithImageUrl(imageurl).AddField("Name", namaewa, false).WithTitle("Featured items of " + date1.ToString("dd/MM/yyyy").Replace("00:00:00", "")).AddField("Description:", des, false).AddField("Price:", pri, false).AddField("Type:", ty, false).AddField("Rarity:", rar, false);
                            if (rar.Equals("legendary"))
                            {
                                test.WithColor(Color.LightOrange);
                            }
                            else if (rar.Equals("epic"))
                            {
                                test.WithColor(Color.Purple);
                            }
                            else if (rar.Equals("uncommon"))
                            {
                                test.WithColor(Color.Green);
                            }
                            else if (rar.Equals("rare"))
                            {
                                test.WithColor(Color.Blue);
                            }
                            else if (rar.Equals("commun"))
                            {
                                test.WithColor(Color.LightGrey);
                            }
                            foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                            {
                                try
                                {
                                    var clientmongo = new MongoClient(DataBaseCredentials.credentials);
                                    var database = clientmongo.GetDatabase("ShopChannel");
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
                                                        await channel.SendMessageAsync("", false, test.Build());
                                                    }
                                                    else if (lang == "fr")
                                                    {
                                                        await channel.SendMessageAsync("", false, test.Build());
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
                        catch (ArgumentException)
                        {
                        }
                    }
                    for (int i = 0; i <= 10; i++)
                    {
                        try
                        {
                            string imageurl1 = shop.data.daily[i].images.icon;
                            string namaewa1 = shop.data.daily[i].name;
                            string des1 = shop.data.daily[i].description;
                            string pri1 = shop.data.daily[i].price;
                            string ty1 = shop.data.daily[i].readableType;
                            string rar1 = shop.data.daily[i].rarity;
                            string date = shop.data.date;
                            DateTime date1 = shop.data.date;
                            var test1 = new EmbedBuilder().WithFooter("Made by PsychoPast / Powered by FNBR.Co API", "https://cdn.awwni.me/15pqc.png").WithImageUrl(imageurl1).AddField("Name", namaewa1, false).WithTitle("Daily items of " + date1.ToString("dd/MM/yyyy").Replace("00:00:00", "")).AddField("Description:", des1, false).AddField("Price:", pri1, false).AddField("Type:", ty1, false).AddField("Rarity:", rar1, false);
                            if (rar1.Equals("legendary"))
                            {
                                test1.WithColor(Color.LightOrange);
                            }
                            else if (rar1.Equals("epic"))
                            {
                                test1.WithColor(Color.Purple);
                            }
                            else if (rar1.Equals("uncommon"))
                            {
                                test1.WithColor(Color.Green);
                            }
                            else if (rar1.Equals("rare"))
                            {
                                test1.WithColor(Color.Blue);
                            }
                            else if (rar1.Equals("commun"))
                            {
                                test1.WithColor(Color.LightGrey);
                            }
                            foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                            {
                                try
                                {
                                    var clientmongo = new MongoClient(DataBaseCredentials.credentials);
                                    var database = clientmongo.GetDatabase("ShopChannel");
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
                                                        await channel.SendMessageAsync("", false, test1.Build());
                                                    }
                                                    else if (lang == "fr")
                                                    {
                                                        await channel.SendMessageAsync("", false, test1.Build());
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
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                }
                else if (shop1.Equals(BotSetup.shopchanged))
                {
                }
            }
            catch
            {
            }
        }
        public void Timer5()
        {
            var DailyTime = "00:10:00";
            var timeParts = DailyTime.Split(new char[1] { ':' });

            var dateNow = DateTime.Now.ToUniversalTime();
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                       int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = date.AddDays(1);
                ts = date - dateNow;
            }

            //waits certan time and run the code
             Task.Delay(ts).ContinueWith((x) => Timer_Elapsed5());
        }
        public async void Timer_Elapsed5()
        {
            var client1 = new BotSetup();
            try
            {
                var apirequ = new ApiSetup();
                string shop1 = apirequ.Fnbrco("https://fnbr.co/api/shop");
                if (!shop1.Equals(BotSetup.shopchanged))
                {
                    BotSetup.shopchanged = shop1;
                    dynamic shop = JsonConvert.DeserializeObject(shop1);

                    for (int i = 0; i <= 10; i++)
                    {
                        try
                        {
                            string imageurl = shop.data.featured[i].images.icon;
                            string namaewa = shop.data.featured[i].name;
                            string des = shop.data.featured[i].description;
                            string pri = shop.data.featured[i].price;
                            string ty = shop.data.featured[i].readableType;
                            string rar = shop.data.featured[i].rarity;
                            string date = shop.data.date;
                            DateTime date1 = shop.data.date;
                            var test = new EmbedBuilder().WithFooter("Made by PsychoPast / Powered by FNBR.Co API", "https://cdn.awwni.me/15pqc.png").WithImageUrl(imageurl).AddField("Name", namaewa, false).WithTitle("Featured items du " + date1.ToString("dd/MM/yyyy").Replace("00:00:00", "")).AddField("Description:", des, false).AddField("Price:", pri, false).AddField("Type:", ty, false).AddField("Rarity:", rar, false);
                            if (rar.Equals("legendary"))
                            {
                                test.WithColor(Color.LightOrange);
                            }
                            else if (rar.Equals("epic"))
                            {
                                test.WithColor(Color.Purple);
                            }
                            else if (rar.Equals("uncommon"))
                            {
                                test.WithColor(Color.Green);
                            }
                            else if (rar.Equals("rare"))
                            {
                                test.WithColor(Color.Blue);
                            }
                            else if (rar.Equals("commun"))
                            {
                                test.WithColor(Color.LightGrey);
                            }
                            foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                            {
                                try
                                {
                                    var clientmongo = new MongoClient(DataBaseCredentials.credentials);
                                    var database = clientmongo.GetDatabase("ShopChannel");
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
                                                        await channel.SendMessageAsync("", false, test.Build());
                                                    }
                                                    else if (lang == "fr")
                                                    {
                                                        await channel.SendMessageAsync("", false, test.Build());
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
                        catch (ArgumentException)
                        {
                        }
                    }
                    for (int i = 0; i <= 10; i++)
                    {
                        try
                        {
                            string imageurl1 = shop.data.daily[i].images.icon;
                            string namaewa1 = shop.data.daily[i].name;
                            string des1 = shop.data.daily[i].description;
                            string pri1 = shop.data.daily[i].price;
                            string ty1 = shop.data.daily[i].readableType;
                            string rar1 = shop.data.daily[i].rarity;
                            string date = shop.data.date;
                            DateTime date1 = shop.data.date;
                            var test1 = new EmbedBuilder().WithFooter("Made by PsychoPast / Powered by FNBR.Co API", "https://cdn.awwni.me/15pqc.png").WithImageUrl(imageurl1).AddField("Name", namaewa1, false).WithTitle("Daily items du " + date1.ToString("dd/MM/yyyy").Replace("00:00:00", "")).AddField("Description:", des1, false).AddField("Price:", pri1, false).AddField("Type:", ty1, false).AddField("Rarity:", rar1, false);
                            if (rar1.Equals("legendary"))
                            {
                                test1.WithColor(Color.LightOrange);
                            }
                            else if (rar1.Equals("epic"))
                            {
                                test1.WithColor(Color.Purple);
                            }
                            else if (rar1.Equals("uncommon"))
                            {
                                test1.WithColor(Color.Green);
                            }
                            else if (rar1.Equals("rare"))
                            {
                                test1.WithColor(Color.Blue);
                            }
                            else if (rar1.Equals("commun"))
                            {
                                test1.WithColor(Color.LightGrey);
                            }
                            foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
                            {
                                try
                                {
                                    var clientmongo = new MongoClient(DataBaseCredentials.credentials);
                                    var database = clientmongo.GetDatabase("ShopChannel");
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
                                                        await channel.SendMessageAsync("", false, test1.Build());
                                                    }
                                                    else if (lang == "fr")
                                                    {
                                                        await channel.SendMessageAsync("", false, test1.Build());
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
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                }
                else if (shop1.Equals(BotSetup.shopchanged))
                {
                }
            }
            catch
            {
            }
        }
    }
}*/