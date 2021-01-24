using Discord;
using Discord.Commands;
using NitsyBot.AutoGetters;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Utilities;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class GetDonaldLocation : ModuleBase<SocketCommandContext>
    {
        [Command("location")]
        public async Task GetLoc()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            EmbedBuilder embed = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).WithTitle("Donald Mustard Location").AddField("Current Donald Mustard Location", GetAutoLocation.oldLocation, false).WithColor(new NitsyBotUtilities().GeneratedColor);
            await ReplyAsync("", false, embed.Build());
        }
    }
}