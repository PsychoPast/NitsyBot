using NitsyBot.DataBase;
using System.Threading.Tasks;
namespace NitsyBot.Events
{
    public class OnBotReady
    {
        private static bool Downloaded = false;
        public async Task Ready()
        {
            if (!Downloaded)
            {
                await new GetGuildLanguage().GetLanguages();
                Downloaded = true;
            }
        }
    }
}