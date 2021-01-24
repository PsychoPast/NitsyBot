using NitsyBot.Core.Interface;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System.Threading.Tasks;

namespace NitsyBot.Checkers
{
    public class GetFortniteVersion : ISetup
    {
        private string version; //A string to store the http://benbotfn.tk:8080/api/status content page.
        public bool IsSuccessful { get; set; } //Check if the page download was successful.
        public async Task<string> GetContentAsync()
        {
            try
            {
                version = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.fnversionUrl);
                IsSuccessful = true;
            }
            catch
            {
                IsSuccessful = false;
            }
            return version;
        }
    }
}