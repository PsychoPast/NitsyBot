using System.Threading.Tasks;

namespace NitsyBot.Core.Interface
{
    internal interface ISetup
    {
        public bool IsSuccessful { get; set; }

        public Task<string> GetContentAsync();
    }
}