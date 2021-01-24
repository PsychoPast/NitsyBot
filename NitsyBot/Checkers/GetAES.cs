using NitsyBot.Core.Interface;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System.Threading.Tasks;

namespace NitsyBot.Checkers
{
    public class GetAES : ISetup
    {
        private string aes; //A string to store the http://benbotfn.tk:8080/api/aes content page.
        public bool IsSuccessful { get; set; }//Check if the page download was successful.
        public async Task<string> GetContentAsync()
        {
            try
            {
                aes = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.aesUrl);
                IsSuccessful = true;
            }
            catch
            {
                IsSuccessful = false;
            }
            return aes;
        }
    }
}