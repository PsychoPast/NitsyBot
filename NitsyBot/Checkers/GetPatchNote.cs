using NitsyBot.Core.Interface;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using System.Threading.Tasks;
namespace NitsyBot.Checkers
{
    public class GetPatchNote : ISetup
    {
        private string patchnote;
        public bool IsSuccessful { get; set; }
        public async Task<string> GetContentAsync()
        {
            try
            {
                patchnote = await SingletonClass.Instance.httpClient.GetStringAsync(URLs.patchnoteUrl);
                IsSuccessful = true;
            }
            catch
            {
                IsSuccessful = false;
            }
            return patchnote;
        }
    }
}