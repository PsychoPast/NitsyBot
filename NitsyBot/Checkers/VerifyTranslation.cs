using NitsyBot.Core.Setup;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System.Timers;

namespace NitsyBot.Checkers
{
    public static class VerifyTranslation
    {
        public static void TimerTrans()
        {
            Timer timer = new Timer()
            {
                Interval = 120000
            };
            timer.Elapsed += new ElapsedEventHandler(CheckTrans);
            timer.Start();
        }

        private static async void CheckTrans(object sender, ElapsedEventArgs e)
        {
            try
            {
                string changed = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.translationUrl);
                if (!changed.Equals(BotSetup.frtranslation))
                {
                    BotSetup.frtranslation = changed;
                }
                else
                {
                }
            }
            catch
            {
            }
        }
    }
}