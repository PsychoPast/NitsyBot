using Discord;
using Discord.WebSocket;
using MongoDB.Driver;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NitsyBot.Events
{
    public class DatabaseDelete
    {
        public async Task DeleteGuild(SocketGuild guild)
        {
            MongoClient clientmongo = DataBaseClient.mongoClient;
            IMongoDatabase database1 = clientmongo.GetDatabase("Language");
            IMongoDatabase database2 = clientmongo.GetDatabase("NitsyChannel");
            IMongoDatabase database3 = clientmongo.GetDatabase("AesChannel");
            IMongoDatabase database4 = clientmongo.GetDatabase("PatchNoteChannel");
            IMongoDatabase database5 = clientmongo.GetDatabase("ShopChannel");
            IMongoDatabase database6 = clientmongo.GetDatabase("StatusChannel");
            IMongoDatabase database7 = clientmongo.GetDatabase("WarnChannel");
            IMongoDatabase database8 = clientmongo.GetDatabase("ReportChannel");
            IMongoDatabase database9 = clientmongo.GetDatabase("SupportChannel");
            List<IMongoDatabase> databases = new List<IMongoDatabase>()
            {
                database1,
                database2,
                database3,
                database4,
                database5,
                database6,
                database7,
                database8,
                database9
            };
            try
            {
                clientmongo.DropDatabase(guild.Id.ToString());
            }
            catch
            {

            }
            foreach (IMongoDatabase database in databases)
            {
                try
                {
                    database.DropCollection(guild.Id.ToString());
                }
                catch
                {

                }
            }
            GetGuildLanguage.languages.Remove(guild.Id.ToString());
            IUser user = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID) as IUser;
            await UserExtensions.SendMessageAsync(user, $"Nitsy left the server **{guild.Name}** and the database linked to it was successfuly deleted.");
        }
    }
}