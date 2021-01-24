namespace NitsyBot.Core.UserInfo
{
    public class User
    {
        public ulong id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string MFACode { get; set; }
    }
}