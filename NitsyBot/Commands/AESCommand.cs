using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitsyBot.Core;
using NitsyBot.Core.Setup;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class AESKey : ModuleBase<SocketCommandContext>
    {
        [Command("aes")]
        public async Task AesCommand()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            bool isenglish = language == "en";
            JToken result = JToken.Parse(BotSetup.aeskeys);
            if (result == null)
            {

            }
            EmbedBuilder aes = new EmbedBuilder()
                .WithFooter(ownerinfos[0], ownerinfos[1])
                .WithTitle($"AES keys for the version {BotSetup.version} of Fortnite")
                .WithColor(new NitsyBotUtilities().GeneratedColor)
                .AddField("Main Key",$"{(result["mainKey"].ToString() ?? "Error")}",false);
            JToken additionalKeys = result["additionalKeys"];
            if (additionalKeys != null)
            {
                SortedDictionary<string, string> KeysDict = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(additionalKeys.ToString());
                if (KeysDict != null)
                {
                    foreach (KeyValuePair<string, string> KvP in KeysDict)
                    {
                        aes.AddField(KvP.Key, KvP.Value, false);
                    }
                }
            }
            await ReplyAsync("", false, aes.Build());
        }

    }
}