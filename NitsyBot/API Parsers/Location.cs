using Newtonsoft.Json;

namespace GetLocationn
{
    public class GetLoc
    {
        [JsonProperty("location", NullValueHandling = NullValueHandling.Include)] public string Location { get; set; }
    }
}