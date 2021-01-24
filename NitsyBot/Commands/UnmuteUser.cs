/*using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class UnMuteUser : ModuleBase<SocketCommandContext>
    {
        [Command("unmute")]
        public async Task UnMuteUserAsync(SocketGuildUser user)
        {
            if (Context.Guild.Id != 375265028154327040)
            {
                await ReplyAsync("What?! Sorry I didn't understand what you mean.");
            }
            else
            {
                var perm = Context.Message.Author as SocketGuildUser;
                if (perm.GuildPermissions.ViewAuditLog)
                {
                    await ReplyAsync($"{user.Mention} est de retour pour nous jouer un mauvais tour ;_;");
                    var role = Context.Guild.GetRole(615189002244587568) as IRole;
                    await user.RemoveRoleAsync(role);
                }
                else
                {
                    await ReplyAsync("T ki pour unmute les gens comme ça?");
                }
            }
        }
    }
}*/