using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NitsyBot.DataBase
{
    public class GetGuildLanguage
    {
        public static Dictionary<string, string> languages;
        
        public GetGuildLanguage()
        {
             languages = new Dictionary<string, string>();
        }
        public async Task GetLanguages() //Inchalla this will work
        {
            IMongoCollection<BsonDocument> collectionlang = DataBaseSetup.DataBaseAccess(DataBases.languagedataBase,DataBases.languageCollection);
            Task<List<BsonDocument>> documents = (await collectionlang.FindAsync(new BsonDocument())).ToListAsync();
            List<string> guildsid = new List<string>();
            foreach (SocketGuild guild in SingletonClass.Instance.client.Guilds)
            {
                guildsid.Add(guild.Id.ToString());
            }
            foreach (BsonDocument document in await documents)
            {
                if (guildsid.Contains(document["id"].AsString))
                {
                    languages.Add(guildsid[guildsid.IndexOf(document["id"].AsString)], document["lang"].AsString);
                }
            }
            foreach (string id in guildsid)
            {
                if (!languages.ContainsKey(id))
                {
                    languages.Add(id, "en");
                }
            }
        }
        /*public static async Task<string> GetLanguage(SocketGuild guild)
        {
            IMongoCollection<BsonDocument> collectionlang = DataBaseSetup.DataBaseAccess("Language", "LanguageCollection");
            IAsyncCursor<BsonDocument> language = await collectionlang.FindAsync(Builders<BsonDocument>.Filter.Eq("id", guild.Id.ToString()));
            if (await language.FirstOrDefaultAsync() == null || (await language.FirstOrDefaultAsync())["lang"].AsString == "en")
            {
                return "en";
            }
            else
            {
                return "fr";
            }
        }*/
    }
}