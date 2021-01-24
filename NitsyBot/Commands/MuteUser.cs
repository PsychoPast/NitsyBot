/*using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class MuteUser : ModuleBase<SocketCommandContext>
    {
        [Command("mute")]
        public async Task MuteUserAsync(SocketGuildUser user)
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
                    await ReplyAsync($"{user.Mention} a été neutralisé!");
                    var role = Context.Guild.GetRole(615189002244587568) as IRole;
                    await user.AddRoleAsync(role);
                }
                else
                {
                    await ReplyAsync("T ki pour mute les gens comme ça?");
                }
            }
        }
    }
}*/