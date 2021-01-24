using Discord.Commands;
using Discord.WebSocket;
using System.Net.Http;

namespace NitsyBot.Core.Singleton
{
    public sealed class SingletonClass
    {
        public DiscordSocketClient client;
        public CommandService Commands;
        public HttpClient httpClient = new HttpClient();
        private static SingletonClass instance;

        public static SingletonClass Instance
        {
            get
            {
                if (instance == null)
                    instance = new SingletonClass();
                return instance;
            }
        }

        private SingletonClass()
        {
        }
    }
}