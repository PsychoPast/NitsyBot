using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NitsyBot.Core;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class GetReportsCommand : ModuleBase<SocketCommandContext>
    {
        [Command("get-report")]
        public async Task GetReportAsync(SocketUser socketUser = null)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            SocketUser user = socketUser ?? Context.Message.Author;
            SocketGuild guild = Context.Guild as SocketGuild;
            int report = await new GetReportNumber().GetReports(guild.Id.ToString(), user.Id.ToString());
            string language = GetGuildLanguage.languages[guild.Id.ToString()];
            EmbedBuilder rep = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle($"Reports of {user.Username}#{user.Discriminator}")
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            if (report == 0)
            {
                rep.AddField(language == "en" ? "No reports" : "Pas de reports", $"{user.Mention} {(language == "en" ? "has no reports" : "n'a pas de reports")}", false);
            }
            else
            {
                rep.AddField("Reports", $"{user.Mention} {(language == "en" ? "has" : "a")} **{report}** reports", false);
            }
            await ReplyAsync("", false, rep.Build());
        }
    }
}