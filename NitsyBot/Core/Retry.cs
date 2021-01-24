using NitsyBot.Core.Setup;
using System.Timers;

namespace NitsyBot.Core
{
    public class Retry
    {
        public static void RetryTimer()
        {
            Timer timer = new Timer
            {
                Interval = 70000
            };
            timer.Elapsed += RetryT;
            timer.Start();
        }

        private static async void RetryT(object sender, ElapsedEventArgs e)
        {
            if (BotSetup.success)
            {
                return;
            }
            if (await new BotSetup().SetEverythingUp())
            {
                new BotSetup().StartTheTimers();
            }
        }
    }
}