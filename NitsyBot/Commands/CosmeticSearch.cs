using Discord;
using Discord.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class CosmeticSearch : ModuleBase<SocketCommandContext>
    {
        [Command("search")]
        public async Task SearchCosmetics(string type = null, string item = null, string icon = null)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            EmbedBuilder embed = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {ownerinfos[0]}",
                    IconUrl = ownerinfos[1]
                },
                Title = "Je n'ai pas pu envoyer des informations sur ce cosmétique. Il peut y avoir plusieurs raisons:",
                Color = Color.Red,
                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "1)",
                        Value = "Le cosmétique n'existe pas dans l'api de FNBR.CO.",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "2)",
                        Value = "T'as écris le nom du cosmétique en français alors qu'il fallait le faire en anglais.",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "3)",
                        Value = "T'as pas respecter le format ```+search [type] [nom du cosmetique]``` ou si tu veux l'icon au lieu de l'image featured ```+search [type] [nom du cosmétique] -icon```",
                        IsInline = false
                    },
                     new EmbedFieldBuilder
                    {
                        Name = "4)",
                        Value = "T'as fais une erreur dans le nom du cosmétique ou du type",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "5)",
                        Value = "Soit sur de mettre les noms composés entre deux \" \"",
                        IsInline = false
                    }
                }
            };

            EmbedBuilder embedEn = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {ownerinfos[0]}",
                    IconUrl = ownerinfos[1]
                },
                Title = "I couldn't find informations about that cosmetic, it could be for multiple reasons:",
                Color = Color.Red,
                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "1)",
                        Value = "The cosmetic doesn't exist in FNBR.CO API.",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "2)",
                        Value = "You wrote the cosmetic name or type name incorrectly.",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "3)",
                        Value = "You didn't respect the format ```+search [type] [cosmetic name]``` or if you want the icon ```+search [type] [cosmetic name] -icon```",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "4)",
                        Value = "Be sure to write composed names between two \" \"",
                        IsInline = false
                    }
                }
            };

            CosmeticEmbedBuilder cosmetic = new CosmeticEmbedBuilder();
            try
            {
                DataBaseSetup db = new DataBaseSetup();
                IAsyncCursor<BsonDocument> cursorDB = null;// await db.DataBaseLang(Context.Guild.Id.ToString());

                while (await cursorDB.MoveNextAsync())
                {
                    foreach (BsonDocument doc1 in cursorDB.Current)
                    {
                        string lang = doc1["lang"].AsString;
                        if (lang == "en")
                        {
                            try
                            {
                                await ReplyAsync("", false, cosmetic.Cosmetic(item, type, icon));
                            }
                            catch
                            {
                                await ReplyAsync("", false, embedEn.Build());
                            }
                        }
                        else if (lang == "fr")
                        {
                            try
                            {
                                await ReplyAsync("", false, cosmetic.FrenchCosmetic(item, type, icon));
                            }
                            catch
                            {
                                await ReplyAsync("", false, embed.Build());
                            }
                        }
                        else
                            await ReplyAsync("I couldn't post. Check that you set correctly the language to fr or en.");
                    }
                }
            }
            catch { /**/ }
        }
    }
}