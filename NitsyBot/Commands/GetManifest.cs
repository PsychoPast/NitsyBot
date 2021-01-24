using Discord;
using Discord.Commands;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Utilities;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class GetManifest : ModuleBase<SocketCommandContext>
    {
        [Command("manifest")]
        public async Task GetMan()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string[] getmanifest = BotSetup.verfest;
            if (getmanifest == null)
            {
                EmbedBuilder error = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).WithTitle("There was a problem getting the manifest. Please contact the owner.").WithColor(Color.Red);
                await ReplyAsync("", false, error.Build());
            }
            else
            {
                EmbedBuilder mani = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).AddField("Version", getmanifest[0], false).AddField("Manifest", getmanifest[1], false).WithColor(new NitsyBotUtilities().GeneratedColor);
                await ReplyAsync("", false, mani.Build());
            }
        }
    }
}