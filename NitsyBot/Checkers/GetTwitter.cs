using GetLocationn;
using Newtonsoft.Json;
using NitsyBot.Core.Interface;
using NitsyBot.Core.Structs;
using System;
using System.Threading.Tasks;
using Tweetinvi;

namespace NitsyBot.Checkers
{
    public class GetTwitter : ISetup
    {
        private readonly string _key;
        private readonly string _keysecret;
        private readonly string _token;
        private readonly string _tokensecret;
        private string loc;
        public GetTwitter(string key,string keysecret,string token,string tokensecret)
        {
            _key = key;
            _keysecret = keysecret;
            _token = token;
            _tokensecret = tokensecret;
        }
        public bool IsSuccessful { get; set; }
        public Task<string> GetContentAsync()
        {
            try
            {
                Auth.SetUserCredentials(_key, _keysecret, _token, _tokensecret);
                string getuserinfos = TwitterAccessor.ExecuteGETQueryReturningJson(Twitter.TwitterUrl);
                loc = JsonConvert.DeserializeObject<GetLoc>(getuserinfos).Location;
                IsSuccessful = true;
            }
            catch
            {
                IsSuccessful = false;
            }
            return Task.Run(() =>
            {
                return loc;
            });
        }
    }
}