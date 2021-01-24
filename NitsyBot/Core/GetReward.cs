using Discord;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitsyBot.Core.Utilities;
using System;

namespace NitsyBot.Core
{
    public class GetReward
    {
        public Embed GetRewardAsync(string url)
        {
            dynamic content = JsonConvert.DeserializeObject(url);
            JToken fieldarray = content.notifications[0].items[0];
            JToken itemfield = fieldarray["itemType"];
            string items = itemfield.Value<string>();
            JToken quantity = fieldarray["quantity"];
            string itemquantity = quantity.Value<string>();
            DateTime thistime = DateTime.Now.ToUniversalTime();
            string[] ownerinfos =  GetOwnerInfos.Infos();
            EmbedBuilder rew = new EmbedBuilder()
                .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                .WithTitle("Your Daily Save The World Reward")
                .AddField("Claimed on", thistime.ToString("dd/MM/yyyy") + " at " + thistime.ToString("HH:mm:ss"), false)
                .AddField("Item you got", items, false)
                .AddField("Quantity", itemquantity, false)
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            return rew.Build();
        }
    }
}