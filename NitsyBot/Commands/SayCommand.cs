using Discord.Commands;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class SayCommand : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder] string echo)
        {
            if (echo.Contains("@everyone") || echo.Contains("@here"))
            {
                await ReplyAsync("ok");
            }
            await ReplyAsync(echo);
        }
    }
}