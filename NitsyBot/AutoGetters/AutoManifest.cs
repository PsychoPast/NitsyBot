using Discord;
using Discord.WebSocket;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.Core.Utilities;
using System.Timers;

namespace NitsyBot.AutoGetters
{
    public class AutoManifest
    {
        readonly AuthLess authLess;
        public AutoManifest()
        {
            authLess = new AuthLess();
        }
        public void TimerMani()
        {
            Timer timer = new Timer
            {
                Interval = 240000
            };
            timer.Elapsed += CheckMan;
            timer.Start();
        }

        public void TimerManifest()
        {
            Timer timer = new Timer
            {
                Interval = 1.44e+7
            };
            timer.Elapsed += ReAuth;
            timer.Start();
        }

        public async void CheckMan(object sender, ElapsedEventArgs e)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string[] newman = await authLess.GetManifest().ConfigureAwait(false);
            if (newman != BotSetup.verfest)
            {
                BotSetup.verfest = newman;
                SocketUser me = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID) as SocketUser;
                if (newman == null)
                {
                    await me.SendMessageAsync("There was a problem and the manifest is null", false);
                }
                else
                {
                    EmbedBuilder mani = new EmbedBuilder().WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).AddField("Version", newman[0], false).AddField("Manifest", newman[1], false).WithColor(new NitsyBotUtilities().GeneratedColor);
                    await me.SendMessageAsync("", false, mani.Build());
                }
            }
        }
        public async void ReAuth(object sender, ElapsedEventArgs e)
        {
            await authLess.Auth();
        }
    }
}