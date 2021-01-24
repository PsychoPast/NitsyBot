using Discord;
using Discord.Commands;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class BotInvite : ModuleBase<SocketCommandContext>
    {
        [Command("invite")]
        public async Task InviteTask()
        {
            await ReplyAsync("Checks mps!");
            await UserExtensions.SendMessageAsync(Context.Message.Author, "https://discordapp.com/api/oauth2/authorize?client_id=602126871337107468&permissions=8&scope=bot");
        }
    }
}