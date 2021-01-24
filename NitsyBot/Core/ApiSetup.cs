using GetLocationn;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitsyBot.Commands;
using NitsyBot.Core.Structs;
using RestSharp;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Token;
using Tweetinvi;

namespace NitsyBot.Core
{
    public class ApiSetup
    {
        public static string Token = null;
        private static string Cookie = null;
        private static string SpecialToken = null;
        private const string FnbrKey = "1a2e6980-1c57-4bc7-a3f5-4c50ff1c9b57";
        private const string BaseUrl = "https://account-public-service-prod03.ol.epicgames.com/";
        private const string ManifestUrl = "https://launcher-public-service-prod06.ol.epicgames.com/launcher/api/public/assets/Windows/4fe75bbc5a674f4f9b356b5c90567da5/Fortnite?label=Live";
        public static CookieContainer cookies = new CookieContainer();
        private static RestClient epicClient = new RestClient("https://www.epicgames.com/id/api/") { CookieContainer = cookies };
        private const string Launcher_Token = "basic MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=";
        private static string method = null;

        private static async Task<string> GetToken()
        {
            RestClient authClient = new RestClient(BaseUrl);
            RestRequest authRequest = new RestRequest("account/api/oauth/token", Method.POST);
            authRequest.AddParameter("Authorization", Launcher_Token, ParameterType.HttpHeader);
            authRequest.AddParameter("grant_type", "exchange_code", ParameterType.GetOrPost);
            authRequest.AddParameter("exchange_code", await ExchangeCode(), ParameterType.GetOrPost);
            authRequest.AddParameter("token_type", "eg1", ParameterType.GetOrPost);
            IRestResponse authResponse = await authClient.ExecuteTaskAsync(authRequest).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<GetToken>(authResponse.Content).AccessToken;
        }

        public static async Task GetCookies()
        {
            IRestResponse csrfResponse = await epicClient.ExecuteTaskAsync(new RestRequest("csrf", Method.GET)).ConfigureAwait(false);
            string xsfrToken = csrfResponse.Cookies.First(x => x.Name == "XSRF-TOKEN").Value;
            Cookie = xsfrToken;
        }

        public static async Task<string> ExchangeCode()
        {
            IRestResponse exchangeResponse = await epicClient.ExecuteTaskAsync(new RestRequest("exchange", Method.GET).AddHeader("x-xsrf-token", Cookie)).ConfigureAwait(false);
            string exchangeCode = JsonConvert.DeserializeObject<GetToken>(exchangeResponse.Content).Code;
            return exchangeCode;
        }

        public static async Task<string> Login(string email, string pass)
        {
            RestRequest loginRequest = new RestRequest("login", Method.POST);
            loginRequest.AddParameter("email", email, ParameterType.GetOrPost);
            loginRequest.AddParameter("password", pass, ParameterType.GetOrPost);
            loginRequest.AddParameter("rememberMe", false, ParameterType.GetOrPost);
            loginRequest.AddHeader("x-xsrf-token", Cookie);
            loginRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            IRestResponse log = await epicClient.ExecuteTaskAsync(loginRequest).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(log.Content))
            {
                method = JsonConvert.DeserializeObject<GetToken>(log.Content).Metadata.Method;
            }
            return log.Content;
        }

        public static async Task<string> MFALogin()
        {
            RestRequest loginRequest = new RestRequest("login/mfa", Method.POST);
            loginRequest.AddParameter("code", RecompSTWAndRedeem.mfacode, ParameterType.GetOrPost);
            loginRequest.AddParameter("method", method, ParameterType.GetOrPost);
            loginRequest.AddParameter("rememberDevice", false, ParameterType.GetOrPost);
            loginRequest.AddHeader("x-xsrf-token", Cookie);
            loginRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            IRestResponse log = await epicClient.ExecuteTaskAsync(loginRequest).ConfigureAwait(false);
            return log.Content;
        }

        public static async Task<string> Request()
        {
            Token = await GetToken();
            return Token;
        }

        public string SpecialExchangeToken()
        {
            RestClient client = new RestClient(BaseUrl);
            RestRequest req = new RestRequest("account/api/oauth/token", Method.POST);
            req.AddHeader("Authorization", Launcher_Token);
            req.AddParameter("grant_type", "client_credentials");
            req.AddParameter("token_type", "eg1");
            string ExchangeToken = JsonConvert.DeserializeObject<GetToken>(client.Execute(req).Content).AccessToken;
            return ExchangeToken;
        }

        public void SpecialRequest()
        {
            SpecialToken = SpecialExchangeToken();
        }

        public string GetManifest()
        {
            RestClient client = new RestClient(ManifestUrl);
            RestRequest req = new RestRequest(Method.GET);
            req.AddHeader("Authorization", "bearer " + SpecialToken);
            IRestResponse response = client.Execute(req);
            string content = response.Content;
            content = JToken.Parse(content).ToString(Formatting.Indented);
            dynamic getmani = JsonConvert.DeserializeObject(content);
            string getman = getmani["items"]["MANIFEST"]["path"];
            string manifest = getman.Replace("Builds/Fortnite/CloudDir/", "").Replace(".manifest", "");
            string getvers = getmani["buildVersion"];
            string version = getvers.Replace("++Fortnite+Release-", "").Replace("-Windows", "");
            string both = $"{version}:{manifest}";
            return both;
        }

        public async Task<string> GetEndpoint(string url)
        {
            var client = new RestClient(url);
            var req = new RestRequest(Method.GET);
            req.AddHeader("Authorization", "bearer " + Token);
            var response = await client.ExecuteTaskAsync(req);
            var content = response.Content;
            content = JToken.Parse(content).ToString(Formatting.Indented);
            return content;
        }

        public string PostEndpoint(string url)
        {
            RestClient client = new RestClient(url);
            RestRequest req = new RestRequest(Method.POST);
            req.AddHeader("X-EpicGames-Language", "fr");
            req.AddHeader("Authorization", "bearer " + Token);
            req.AddJsonBody(new { });
            IRestResponse response = client.Execute(req);
            string content = response.Content;
            content = JToken.Parse(content).ToString(Formatting.Indented);
            return content;
        }

        public async Task DeleteEndpoint(string url)
        {
            RestClient client = new RestClient(url);
            RestRequest req = new RestRequest(Method.DELETE);
            req.AddHeader("Authorization", "bearer " + Token);
            await client.ExecuteTaskAsync(req);
        }

        public async Task<string> CheckKill(string url)
        {
            RestClient client = new RestClient(url);
            RestRequest req = new RestRequest(Method.GET);
            req.AddHeader("Authorization", "bearer " + Token);
            IRestResponse response = await client.ExecuteTaskAsync(req);
            string content = response.Content;
            content = JToken.Parse(content).ToString(Formatting.Indented);
            return content;
        }

        public string Fnbrco(string url)
        {
            RestClient client = new RestClient(url);
            RestRequest req = new RestRequest(Method.GET);
            req.AddHeader("x-api-key", FnbrKey);
            string reque = client.Execute(req).Content;
            dynamic appro = JsonConvert.DeserializeObject(reque);
            string cosmetic = JsonConvert.SerializeObject(appro, Formatting.Indented);
            return cosmetic;
        }

        
    }
}