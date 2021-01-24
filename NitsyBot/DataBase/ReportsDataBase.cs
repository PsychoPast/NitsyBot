using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core.Structs;
using System.Threading.Tasks;

namespace NitsyBot.DataBase
{
    public class ReportsDataBase
    {
        public async Task CreateReportData(SocketGuild guild,SocketUser user)
        {
            int report = await new GetReportNumber().GetReports(guild.Id.ToString(), user.Id.ToString());
            IMongoDatabase reportDatabase = DataBaseClient.mongoClient.GetDatabase(DataBases.reportDataBase);
            IMongoCollection<BsonDocument> reportCollection = reportDatabase.GetCollection<BsonDocument>(guild.Id.ToString());
            IAsyncCursor<BsonDocument> documentExists = await reportCollection.FindAsync(Builders<BsonDocument>.Filter.Eq("userid", user.Id.ToString()));
            BsonDocument document = new BsonDocument
                  {
                    { "username",user.Username},
                    { "userid",user.Id.ToString()},
                    { "servername",guild.Name},
                    { "serverid",guild.Id.ToString()},
                    { "reportnum", 1},
                  };
            if(await documentExists.FirstOrDefaultAsync() == null)
            {
                await reportCollection.InsertOneAsync(document);
            }
            else
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("userid", user.Id.ToString());
                UpdateDefinition <BsonDocument> update = Builders<BsonDocument>.Update.Set("reportnum", report + 1);
                await reportCollection.UpdateOneAsync(filter, update);
            }
        }
        public async Task ResetReportData(SocketGuild guild, SocketUser user)
        {
            IMongoDatabase reportDatabase = DataBaseClient.mongoClient.GetDatabase(DataBases.reportDataBase);
            IMongoCollection<BsonDocument> reportCollection = reportDatabase.GetCollection<BsonDocument>(guild.Id.ToString());
            IAsyncCursor<BsonDocument> documentExists = await reportCollection.FindAsync(Builders<BsonDocument>.Filter.Eq("userid", user.Id.ToString()));
            BsonDocument document = new BsonDocument
                  {
                    { "username",user.Username},
                    { "userid",user.Id.ToString()},
                    { "servername",guild.Name},
                    { "serverid",guild.Id.ToString()},
                    { "reportnum", 0},
                  };
            if (await documentExists.FirstOrDefaultAsync() == null)
            {
                await reportCollection.InsertOneAsync(document);
            }
            else
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("userid", user.Id.ToString());
                UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("reportnum", 0);
                await reportCollection.UpdateOneAsync(filter, update);
            }
        }
    }
}