using Discord.Commands;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class Restart : ModuleBase<SocketCommandContext>
    {
        [RequireOwner]
        [Command("restart")]
        public async Task RestartBot()
        {
            await ReplyAsync("Bot will restart in 10 seconds. . .");
            await ReplyAsync("Restarting . . .");
            await SingletonClass.Instance.client.LogoutAsync();
            await SingletonClass.Instance.client.StopAsync();
            await Task.Delay(10000);
            await new BotSetup().MainAsync();
        }
    }
}