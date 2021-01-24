using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core.Structs;
using System.Threading.Tasks;
namespace NitsyBot.DataBase
{
    internal class GetReportNumber
    {
        private int report = 0;
        public async Task<int> GetReports(string guildid, string userid)
        {
            IMongoCollection<BsonDocument> reportCollection = DataBaseSetup.DataBaseAccess(DataBases.reportDataBase, guildid);
            IAsyncCursor<BsonDocument> findReport = await reportCollection.FindAsync(Builders<BsonDocument>.Filter.Eq("userid", userid));
            BsonDocument reportnum = await findReport.FirstOrDefaultAsync();
            report = reportnum == null ? 0 : reportnum["reportnum"].AsInt32;
            return report;
        }
    }
}