using Discord;
using Discord.Commands;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Structs;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class ResetAllReports : ModuleBase<SocketCommandContext>
    {
        [Command("reset-all")]
        public async Task ResetAllReportAsync()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            if (!NitsyBotUtilities.HasPermissions(Context.User))
            {
                await ReplyAsync(language == "en" ? "Error! You need ManageGuild permission in order to exectute that command!" : "Erreur! Tu as besoin de la permission ManageGuild pour exécuter cette commande!");
                return;
            }
            MongoClient delete = DataBaseClient.mongoClient;
            IMongoDatabase rep = delete.GetDatabase(DataBases.reportDataBase);
            await rep.DropCollectionAsync(Context.Guild.Id.ToString());
            var allres = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle(language == "en " ? $"**{Context.Guild.Name}** reports have been reseted!" : $"**{Context.Guild.Name}** reports ont été réinitialiser!")
                .AddField($"{Context.Guild.Name}", language == "en" ? "reports have been successfuly reseted" : "reports ont été réinitialiser avec succès", false)
                .WithColor(Color.Green);
            await ReplyAsync("", false, allres.Build());
        }
    }
}