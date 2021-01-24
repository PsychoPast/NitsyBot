using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
namespace NitsyBot.Core
{
    public class GetOwnerInfos
    {
        public static string[] Infos()
        {
            string username = $"Made by {SingletonClass.Instance.client.GetUser((ulong)(ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}";
            string avatar = SingletonClass.Instance.client.GetUser((ulong)(ulong)IDS.OwnerID).GetAvatarUrl();
            string[] both = { username, avatar };
            return both;
        }
    }
}