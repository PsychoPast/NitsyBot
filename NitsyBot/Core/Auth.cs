using System.Net;
using System.Threading.Tasks;
using CFortniteAuth;
using NitsyBot.Core.Utilities;
namespace NitsyBot.Core
{
    internal class AuthLess
    {
        readonly FNAuthLess authLess;

        const string manifestUrl = "https://launcher-public-service-prod06.ol.epicgames.com/launcher/api/public/assets/Windows/4fe75bbc5a674f4f9b356b5c90567da5/Fortnite?label=Live";
        public AuthLess()
        {
            authLess = new FNAuthLess();
        }

        public async Task<bool> Auth() => await authLess.Access().ConfigureAwait(false);

        public async Task<string[]> GetManifest()
        {
            IResponse response = await authLess.SendRequestAsync(manifestUrl).ConfigureAwait(false);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return NitsyBotUtilities.ExtractManifest(response.Content);
            }
            return null;
        }
        //to do: add more endpoints
    }
}
