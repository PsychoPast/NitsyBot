using Discord;
using Discord.WebSocket;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.DataBase;
using System.Threading.Tasks;

namespace NitsyBot.Events
{
    public class OnGuildJoined
    {
        public async Task Joined(SocketGuild guild)
        {
            try
            {
                SocketUser joined = SingletonClass.Instance.client.GetUser(guild.Owner.Id) as SocketUser;
                await joined.SendMessageAsync("Hello.Thx for using Nitsy.You can choose 2 languages for your server:English or French.To choose a language,simply write in any channel `+SetLang [fr]/[en]`");
                foreach(SocketGuildChannel channel in guild.Channels)
                {

                }
                IUser user = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID) as IUser;
                await UserExtensions.SendMessageAsync(user, $"Nitsy joined the server **{guild.Name}**.");
                GetGuildLanguage.languages.Add(guild.Id.ToString(), "en");
            }
            catch
            {
            }
        }
    }
}