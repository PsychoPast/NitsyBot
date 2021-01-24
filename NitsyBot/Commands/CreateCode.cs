using Discord;
using Discord.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core.Crypt;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class CreateCode : ModuleBase<SocketCommandContext>
    {
        [Command("create")]
        [RequireContext(ContextType.DM)]
        public async Task Shit(string secret, string email, string pass, [Remainder]string user)
        {
            if (!email.Contains("@"))
            {
                await ReplyAsync("You didn't wrote a valid email format. Please write `+create [your code] [email] [pass] [username]`");
                return;
            }

            string key = DataCrypting.GenerateKey(256);
            string[] en = DataCrypting.Encrypt(email, pass, user, key, 256);
            string fiemail = en[0].Replace("=", "玩");
            string fipass = en[1].Replace("=", "戶");
            string fiusername = en[2].Replace("=", "內");
            MongoClient client = new MongoClient(DataBaseCredentials.credentials);
            IMongoDatabase database = client.GetDatabase("STW");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Rewards");

            BsonDocument document = new BsonDocument
            {
                { "user", Context.Message.Author.Username },
                { "id", Context.Message.Author.Id.ToString() },
                { "code", secret },
                { "key", key },
                { "email", fiemail },
                { "password", fipass },
                { "username", fiusername }
            };

            DataBaseSetup db = new DataBaseSetup();
            IAsyncCursor<BsonDocument> cursorDB = null;// db.DataBaseAccess("STW", "Rewards");

            bool taken = false;
            try
            {
                while (await cursorDB.MoveNextAsync())
                {
                    foreach (var doc in cursorDB.Current)
                    {
                        if (secret == doc["code"])
                        {
                            taken = true;
                        }
                    }
                }
            }
            catch { /**/ }

            if (taken)
                await ReplyAsync("Sorry that code is taken.Please try with another one.");
            else
            {
                await collection.ReplaceOneAsync(
                    filter: new BsonDocument("id", Context.Message.Author.Id.ToString()),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: document);

                await ReplyAsync("This account was successfuly set!");
            }
        }

        [Command("create")]
        public async Task Createinchannel()
        {
            EmbedBuilder info = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                    IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                },
                Title = "Setup Fortnite Easy Save the World reward claiming",
                Color = new NitsyBotUtilities().GeneratedColor,
                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Command syntax",
                        Value = "`+create [your code] [your email] [your password] [your username]`",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "[your code]",
                        Value = "A code of your choice which you have a remember next time you want to redeem. I can contains letter , numbers and characters.",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "[your email] [your password] [your username]",
                        Value = "Your Epic Games account's email , password and username.",
                        IsInline = false
                    },
                     new EmbedFieldBuilder
                    {
                        Name = "When you want to redeem, you do",
                        Value = "`+redeem [your code]`",
                        IsInline = false
                    }
                }
            };

            await ReplyAsync("Check mps!");
            await UserExtensions.SendMessageAsync(Context.Message.Author, "", false, info.Build());
        }
    }
}