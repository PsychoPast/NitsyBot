using NitsyBot.Core.Interface;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System.Threading.Tasks;

namespace NitsyBot.Checkers
{
    public class GetFrenchTranslation : ISetup
    {
        private string translate; //A string to store the https://fn.notofficer.de/api/fnbrtranslations content page.
        public bool IsSuccessful { get; set; }
        public async Task<string> GetContentAsync()
        {
            try
            {
                translate = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.translationUrl);
                IsSuccessful = true;
            }
            catch
            {
                IsSuccessful = false;
            }
            return translate;
        }
    }
}