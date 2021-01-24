using Discord;
using Discord.Commands;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class SupportServer : ModuleBase<SocketCommandContext>
    {
        [Command("server")]
        public async Task SupportServerTask()
        {
            await ReplyAsync("Checks mps!");
            await UserExtensions.SendMessageAsync(Context.Message.Author, "https://discordapp.com/invite/whKMgG5");
        }
    }
}