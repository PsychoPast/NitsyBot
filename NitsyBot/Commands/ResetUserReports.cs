using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NitsyBot.Core;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class ResetUserReportCommand : ModuleBase<SocketCommandContext>
    {
        [Command("reset-report")]
        public async Task ResetReportAsync(SocketUser socketUser = null)
        {
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            bool isenglish = language == "en";
            string[] ownerinfos = GetOwnerInfos.Infos();
            if (socketUser == null)
            {
                await ReplyAsync(isenglish ? "Please, specify a user!" : "Veuillez spécifier un utilisateur!");
                return;
            }
            if (socketUser == Context.Message.Author)
            {
                await ReplyAsync(isenglish ? "You cannot reset yourself 🤦" : "Vous ne pouvez pas vous réinitialiser 🤦");
                return;

            }
            if (!NitsyBotUtilities.HasPermissions(socketUser))
            {
                await ReplyAsync(isenglish ? "Error! You need ManageGuild permission in order to exectute that command!" : "Erreur! Tu as besoin de la permission ManageGuild pour exécuter cette commande!");
                return;
            }
            int report = await new GetReportNumber().GetReports(Context.Guild.Id.ToString(), socketUser.Id.ToString());
            
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1]);
            if (report != 0)
            {
                await new ReportsDataBase().ResetReportData(Context.Guild, socketUser);
                if (isenglish)
                {
                    embedBuilder
                    .WithTitle($"{socketUser.Username}#{socketUser.Discriminator} reports have been reseted!")
                         .AddField($"{socketUser.Username}#{socketUser.Discriminator}", "reports have been successfuly reseted.", false)
                         .WithColor(Color.Green);
                }
                else
                {
                    embedBuilder
                   .WithTitle($"{socketUser.Username}#{socketUser.Discriminator} reports ont été réinitialiser!")
                        .AddField($"{socketUser.Username}#{socketUser.Discriminator}", "reports ont été réinitialiser avec succès.", false)
                        .WithColor(Color.Green);
                }
            }
            else
            {
                if (isenglish)
                {
                    embedBuilder
                        .WithTitle($"{socketUser.Username}#{socketUser.Discriminator} has no reports.")
                        .AddField($"{socketUser.Username}#{socketUser.Discriminator}", "has no reports.", false)
                        .WithColor(Color.Red);
                }
                else
                {
                    embedBuilder
                        .WithTitle($"{socketUser.Username}#{socketUser.Discriminator} n'a pas de reports.")
                        .AddField($"{socketUser.Username}#{socketUser.Discriminator}", "n'a pas de report.", false)
                        .WithColor(Color.Red);
                }
            }
            await ReplyAsync("", false, embedBuilder.Build());
        }
    }
}