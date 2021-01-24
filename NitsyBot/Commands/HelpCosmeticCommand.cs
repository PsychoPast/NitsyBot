using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class HelpCosmeticCommand : ModuleBase<SocketCommandContext>
    {
        [Command("helpcosm")]
        public async Task HelpCosm()
        {
            SocketGuild guild = Context.Guild;
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[guild.Id.ToString()];
            EmbedBuilder helpcosm = new EmbedBuilder()
                .WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1])
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            if(language == "fr")
            {
                helpcosm
                    .WithTitle("Utiliser le +search")
                    .AddField("La Commande:", "+search", false)
                    .AddField("La Structure:", "+search [type] [nom]", false)
                    .AddField("Les types sont:", "backpack, emote, glider, emoji, outfit, pickaxe, skydive, loading, umbrella, spray, toy, pet, music, wrap, banner, bundle, misc", false)
                    .AddField("Nom:", "Nom du cosmetique en **anglais**\nPour les cosmétiques à nom composé (exemple:Black Widow Outfit , **il faut mettre le nom entre deux \" \" comme ça: +search outfit \"Black Widow Outfit\"**", false)
                    .AddField("Pour avoir l'icon à la place de l'image featured", "Ajoute \"-icon\" à la commande", false);
            }
            else
            {
                helpcosm
                    .WithTitle("Use the search function")
                    .AddField("The Main Command:", "+search", false)
                    .AddField("The Structure:", "+search [type] [name]", false)
                    .AddField("The types are:", "backpack, emote, glider, emoji, outfit, pickaxe, skydive, loading, umbrella, spray, toy, pet, music, wrap, banner, bundle, misc", false)
                    .AddField("Name:", "Name of cosmetic\nIf the cosmetic has composed name (example: Black Widow Outfit , you must write it between two \" \" like that: +search outfit \"Black Widow Outfit\"", false)
                    .AddField("To get the icon instead of the featured image", "Add -icon to the command", false);
            }
            
            
            try
            {
                DataBaseSetup db = new DataBaseSetup();
                IAsyncCursor<BsonDocument> cursorLang = null;// await db.DataBaseLang(guild.Id.ToString());
                while (await cursorLang.MoveNextAsync())
                {
                    foreach (var doc1 in cursorLang.Current)
                    {
                        string lang = doc1["lang"].AsString;
                        if (lang == "en")
                        {
                            await ReplyAsync("", false, helpcosm.Build());
                        }
                        else if (lang == "fr")
                        {
                            await ReplyAsync("", false, helpcosm.Build());
                        }
                        else
                        {
                            await ReplyAsync("I couldn't post.Check that you set correctly the language to fr or en.");
                        }
                    }
                }
            }
            catch
            {
                await ReplyAsync("I couldn't post.Check that you set correctly the language to fr or en.");
            }
        }
    }
}