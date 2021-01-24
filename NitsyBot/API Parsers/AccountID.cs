using Newtonsoft.Json;

namespace NitsyBot.Json
{
    public class AccountID
    {
        [JsonProperty("id", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)] public string Id { get; private set; }
    }
}