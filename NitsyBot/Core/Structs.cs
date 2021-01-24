namespace NitsyBot.Core.Structs
{
    public struct Twitter
    {
        public const string TwitterUrl = "https://api.twitter.com/1.1/users/show.json?screen_name=donaldmustard";
    }

    public struct DataBaseCredentials
    {
        public const string credentials = "mongodb+srv://PsychoPast:C85576218M@cluster0-yyor7.azure.mongodb.net/test?retryWrites=true&w=majority";
    }

    public enum IDS : ulong
    {
        OwnerID = 444779014888882177,
    }

    public struct URLs
    {
        public const string patchnoteUrl = "https://pastebin.com/raw/7PUqQy8M"; //get patchnote url from pastebin
        public const string fnversionUrl = "http://benbotfn.tk:8080/api/status";  //fortnite version from BenBot api
        public const string translationUrl = "https://fn.notofficer.de/api/fnbrtranslations"; //french translation api
        public const string aesUrl = "http://benbotfn.tk:8080/api/aes"; //aes keys from BenBot api
        public const string statusUrl = "https://lightswitch-public-service-prod.ol.epicgames.com/lightswitch/api/service/bulk/status?serviceId=fortnite"; //fortnite server status
    }
    public struct DataBases
    {
        public const string languagedataBase = "Language";
        public const string languageCollection = "LanguageCollection";
        public const string channelsDataBase = "Channels";
        public const string supportCollection = "SupportChannel";
        public const string aesCollection = "AesChannel";
        public const string nitsyCollection = "NitsyChannel";
        public const string patchnoteCollection = "PatchNoteChannel";
        public const string statusCollection = "StatusChannel";
        public const string reportCollection = "ReportChannel";
        public const string reportDataBase = "Reports";
    }
}