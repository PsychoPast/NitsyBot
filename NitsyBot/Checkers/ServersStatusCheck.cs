using Newtonsoft.Json;
using NitsyBot.Core.Interface;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System.Threading.Tasks;
namespace NitsyBot.Checkers
{
    public class GetServersStatus : ISetup
    {
        private dynamic checkstatus;
        public bool IsSuccessful { get; set; }
        public async Task<string> GetContentAsync()
        {
            try
            {
                checkstatus = JsonConvert.DeserializeObject(await SingletonClass.Instance.httpClient.GetStringAsync(URLs.statusUrl));
                IsSuccessful = true;
            }
            catch
            {
                IsSuccessful = false;
            }
            return checkstatus[0].status;
        }
    }
}