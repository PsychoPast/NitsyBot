using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class WebSite : ModuleBase<SocketCommandContext>
    {
        [Command("website")]
        public async Task WebsiteTask()
        {
            await ReplyAsync("Checks mps!");
            await UserExtensions.SendMessageAsync(Context.Message.Author, "https://nitsy.home.blog");
        }
    }
}