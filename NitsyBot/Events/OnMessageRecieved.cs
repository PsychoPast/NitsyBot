using Discord.Commands;
using Discord.WebSocket;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using System.Threading.Tasks;
namespace NitsyBot.Events
{
    internal class OnMessageRecieved
    {
        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            SocketUserMessage message = messageParam as SocketUserMessage;
            if (message == null || message.Content == "")
            {
                return;
            }
            int argPos = 0;
            if (!(message.HasCharPrefix('+', ref argPos) || message.HasMentionPrefix(SingletonClass.Instance.client.CurrentUser, ref argPos)) || message.Author.IsBot)
            {
                return;
            }
            // Temporary until I seperate each command.
            if (!BotSetup.success)
            {
                SocketTextChannel channel = SingletonClass.Instance.client.GetChannel(message.Channel.Id) as SocketTextChannel;
                await channel.SendMessageAsync("Bot is initializing , please wait.");
                return;
            }
            SocketCommandContext context = new SocketCommandContext(SingletonClass.Instance.client, message);
            await SingletonClass.Instance.Commands.ExecuteAsync(context, argPos, null);
        }
    }
}