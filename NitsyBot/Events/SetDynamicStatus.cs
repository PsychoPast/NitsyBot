using Discord;
using Discord.WebSocket;
using NitsyBot.Core.Singleton;
using System;
using System.Timers;

namespace NitsyBot.Events
{
    internal class SetDynamicStatus
    {
        public void TimerStatus()
        {
            Timer timer = new Timer
            {
                Interval = 10000
            };
            timer.Elapsed += SetStatus;
            timer.Start();
        }

        private static async void SetStatus(object sender, ElapsedEventArgs e)
        {
            int guildnum = SingletonClass.Instance.client.Guilds.Count;
            int membcount = 0;
            foreach (SocketGuild member in SingletonClass.Instance.client.Guilds)
            {
                membcount += member.MemberCount;
            }
            string[] activities = { $"{guildnum} servers", $"{membcount} members", "+help" };
            ActivityType[] activityType = { ActivityType.Watching, ActivityType.Watching, ActivityType.Playing };
            Random rnd = new Random();
            int index = rnd.Next(activities.Length);
            await SingletonClass.Instance.client.SetGameAsync(activities[index], "", activityType[index]);
        }
    }
}