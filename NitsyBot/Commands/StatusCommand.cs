using Discord;
using Discord.Commands;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.DataBase;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class StatusCommand : ModuleBase<SocketCommandContext>
    {
        [Command("status")]
        [Summary("Give current status of Fortnite servers")]

        public async Task Status()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            if (BotSetup.status == "UP")
            {
                EmbedBuilder embedBuilder = new EmbedBuilder()
                    .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                    .WithColor(Color.Green);

                switch (language)
                {
                    case "fr":
                    embedBuilder
                        .AddField("Status des serveurs de Fortnite", "Les serveurs de Fortnite sont **en ligne**", false);
                        break;
                    case "en":
                    embedBuilder
                    .AddField("Fortnite servers status", "Fortnite servers are **online**", false);
                        break;
                }
                await ReplyAsync("", false, embedBuilder.Build());
            }
            else
            {
                EmbedBuilder embedBuilder = new EmbedBuilder()
                    .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                    .WithColor(Color.Red);
                switch (language)
                {
                    case "fr":
                    embedBuilder
                    .AddField("Status des serveurs de Fortnite", "Les serveurs de Fortnite sont **en maintenance**", false);
                        break;
                    case "en":
                    embedBuilder
                        .AddField("Fortnite servers status", "Fortnite servers are **down**", false);
                        break;
                }
                await ReplyAsync("", false, embedBuilder.Build());
            }
        }
    }
}