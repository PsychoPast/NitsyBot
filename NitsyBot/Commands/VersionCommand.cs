using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class VersionCommand : ModuleBase<SocketCommandContext>
    {
        [Command("fnversion")]
        [Summary("Gives latest fortnite version.")]
        public async Task Update()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            if (language == "fr")
            {
                embedBuilder
                .WithTitle("Version actuelle de Fortnite")
                .AddField("La version actuelle est:", "**" + BotSetup.fnversion.Replace("++Fortnite+Release-", "")
                .Replace("-Windows", "") + "**", false);
            }
            else
            {
                embedBuilder
                .WithTitle("Current Fortnite Version")
                .AddField("Current Fornite's version is:", "**" + BotSetup.fnversion.Replace("++Fortnite+Release-", "")
                .Replace("-Windows", "") + "**", false);
            }
            await ReplyAsync("", false, embedBuilder.Build());
        }
    }
}