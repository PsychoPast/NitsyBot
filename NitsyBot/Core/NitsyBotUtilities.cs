using System;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace NitsyBot.Core.Utilities
{
    internal class NitsyBotUtilities
    {
        public static bool IsImage(string image) => image.Contains("jpg") ? true : image.Contains("png") ? true : false;
        private readonly Random random = new Random();
        private int Red => random.Next(0, 255);
        private int Green => random.Next(0, 255);
        private int Blue => random.Next(0, 255);
        public Color GeneratedColor => new Color(Red, Green, Blue);
        public static bool HasPermissions(SocketUser user) => (user as SocketGuildUser).GuildPermissions.ManageGuild;
        public static string[] ExtractManifest(string content)
        {
            string[] infos = new string[2];
            JToken token = JToken.Parse(content);
            string version = token.SelectToken("buildVersion").ToString().Replace("++Fortnite+Release-", "").Replace("-Windows", "");
            infos[0] = version;
            string manifest = token.SelectToken("items").SelectToken("MANIFEST").SelectToken("path").ToString().Replace("Builds/Fortnite/CloudDir/", "").Replace(".manifest", "");
            infos[1] = manifest;
            return infos;
        }
    }
}