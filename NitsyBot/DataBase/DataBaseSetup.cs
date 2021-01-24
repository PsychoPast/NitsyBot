using MongoDB.Bson;
using MongoDB.Driver;

namespace NitsyBot.DataBase
{
    internal class DataBaseSetup
    {
        public static IMongoCollection<BsonDocument> DataBaseAccess(string dataBase,string collection)
        {
            IMongoDatabase database = DataBaseClient.mongoClient.GetDatabase(dataBase);
            IMongoCollection<BsonDocument> databaseCollection = database.GetCollection<BsonDocument>(collection);
            return databaseCollection;
        }
    }
    public class DataBaseClient
    {
        public static MongoClient mongoClient { get; private set; }
        public DataBaseClient(string credentials)
        {
            mongoClient = new MongoClient(credentials);
        }
        public DataBaseClient()
        {

        }
    }
}