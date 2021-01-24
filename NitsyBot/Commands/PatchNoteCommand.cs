using Discord;
using Discord.Commands;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Net;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class PatchNoteCommand : ModuleBase<SocketCommandContext>
    {
        [Command("patchnote")]
        [Summary("Gives latest fortnite patchnote.")]
        public async Task PatchNoteAsync()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            bool success = true;
            try
            {
                WebRequest req = WebRequest.Create(BotSetup.patchnote);
                WebResponse res = req.GetResponse();
            }
            catch (WebException)
            {
                success = false;
            }
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            switch (language)
            {
                case "fr":
                embedBuilder
                  .AddField("Patch Note de la version " + BotSetup.version + " de Fortnite", BotSetup.patchnote, false);
                    break;
                case "en":
                embedBuilder
                    .AddField("Patch Note of the version " + BotSetup.version + " of Fortnite", BotSetup.patchnote.Replace("/fr/", "/en-US/"), false);
                    break;
            }
            EmbedBuilder error = new EmbedBuilder()
               .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                   .WithColor(Color.Red);
            switch (language)
            {
                case "fr":
                error
                   .AddField("Patch Note de la version " + BotSetup.version + " de Fortnite", "**Sorry,le patch note de la version** " + BotSetup.version + " **de Fortnite n'est pas encore disponible**", false);
                    break;
                case "en":
                error
                    .AddField("Patch Note of the version " + BotSetup.version + " of Fortnite", "**Sorry,the patch note of the version** " + BotSetup.version + " **of Fortnite is not out yet**", false);
                    break;
            }       
            switch (success)
            {
                case true:
                await ReplyAsync("", false, embedBuilder.Build());
                    break;
                case false:
                await ReplyAsync("", false, error.Build());
                    break;
            }
        }
    }
}