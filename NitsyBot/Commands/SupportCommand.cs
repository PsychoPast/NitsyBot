using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace NitsyBot.Commands
{
    public class SupportCommand : ModuleBase<SocketCommandContext>
    {
        [Command("support")]
        [RequireContext(ContextType.DM)]
        public async Task Support(ulong servid, [Remainder]string message = null)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[servid.ToString()];
            bool isenglish = language == "en";
            DateTime thistime = DateTime.Now;
            EmbedBuilder supportembed = new EmbedBuilder()
                .WithThumbnailUrl(Context.Message.Author.GetAvatarUrl())
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle("Support")
                .AddField(isenglish ? "Member" : "Membre", Context.Message.Author, false)
                .AddField("Id", Context.Message.Author.Id, false)
                .AddField("Message", message, false)
                .AddField("Date", $"{thistime.ToUniversalTime().ToString("dd/MM/yyyy")}{(isenglish ? "at" : "à")} {thistime.ToUniversalTime().ToString("HH:mm:ss")} UTC")
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            EmbedBuilder sentembed = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle(isenglish ? "Your message was sent and a staff member will reply as soon as possible." : "Votre message a été envoyer et un membre du staff vous répondra le plus rapidement possible")
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            IMongoCollection<BsonDocument> supportdatabase = DataBaseSetup.DataBaseAccess(DataBases.channelsDataBase, DataBases.supportCollection);
            EmbedBuilder filetype = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle(isenglish ? "Only .png and .jpg files are allowed!" : "Seulement les fichiers .png et .jpg sont autorisés")
                .WithColor(Color.Red);
            IAsyncCursor<BsonDocument> document = await supportdatabase.FindAsync(Builders<BsonDocument>.Filter.Eq("id", servid.ToString()));
            if (await document.FirstOrDefaultAsync() == null)
            {
                await ReplyAsync(isenglish ? "This server didn't set up a support channel!" : "Le serveur n'a pas de salon pour recevoir les messages de support!");
                return;
            }
            string longvalue = (await document.FirstOrDefaultAsync())["channel"].AsString;
            SocketTextChannel channel = SingletonClass.Instance.client.GetChannel(ulong.Parse(longvalue)) as SocketTextChannel;
            if (Context.Message.Attachments.Count != 0)
            {
                string url = Context.Message.Attachments.First().Url;
                if (NitsyBotUtilities.IsImage(url))
                {
                    supportembed.WithImageUrl(url);
                }
                else
                {
                    await ReplyAsync("", false, filetype.Build());
                    return;
                }
            }
            await channel.SendMessageAsync($"{Context.Message.Author.Id}", false, supportembed.Build());
            await ReplyAsync("", false, sentembed.Build());
        }
        [Command("reply")]
        public async Task Reply(ulong id, [Remainder]string message = null)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            bool isenglish = language == "en";
            EmbedBuilder filetype = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle(isenglish ? "Only .png and .jpg files are allowed!" : "Seulement les fichiers .png et .jpg sont autorisés")
                .WithColor(Color.Red);
            DateTime thistime = DateTime.Now;
            SocketUser user = SingletonClass.Instance.client.GetUser(id) as SocketUser;
            EmbedBuilder replyembed = new EmbedBuilder()
                .WithThumbnailUrl(Context.Message.Author.GetAvatarUrl())
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle("Support")
                .AddField(isenglish ? "Staff member" : "Membre du staff", Context.Message.Author, false)
                .AddField("Message", message, false)
                .AddField("Date", $"{thistime.ToUniversalTime().ToString("dd/MM/yyyy")}{(isenglish ? "at" : "à")} {thistime.ToUniversalTime().ToString("HH:mm:ss")} UTC")
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            EmbedBuilder replysent = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle(isenglish ? "Your reply was sent." : "Votre réponse a été envoyé!")
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            IMongoCollection<BsonDocument> supportdatabase = DataBaseSetup.DataBaseAccess(DataBases.channelsDataBase, DataBases.supportCollection);
            IAsyncCursor<BsonDocument> document = await supportdatabase.FindAsync(Builders<BsonDocument>.Filter.Eq("id", Context.Guild.Id.ToString()));
            if (await document.FirstOrDefaultAsync() == null) //Even though it can't be null but who knows ¯\_(ツ)_/¯
            {
                await ReplyAsync(isenglish ? "This server didn't set up a support channel!" : "Le serveur n'a pas de salon pour recevoir les messages de support!");
                return;
            }
            if (Context.Channel.Id != ulong.Parse((await document.FirstOrDefaultAsync())["channel"].AsString))
            {
                await ReplyAsync(isenglish ? "You cannot use that in this channel!" : "Vous ne pouvez pas utiliser cette commande dans ce salon!");
                return;
            }
            if (Context.Message.Attachments.Count != 0)
            {
                string url = Context.Message.Attachments.First().Url;
                if (NitsyBotUtilities.IsImage(url))
                {
                    replyembed.WithImageUrl(url);
                }
                else
                {
                    await ReplyAsync("", false, filetype.Build());
                    return;
                }
            }
            await ReplyAsync("", false, replysent.Build());
            try
            {
                await user.SendMessageAsync("", false, replyembed.Build());
            }
            catch (HttpException e)
            {
                await ReplyAsync($"{e.HttpCode}\n{e.Reason}");
                await ReplyAsync(isenglish ? "It looks like there was a problem DMing the user. Maybe he has DMs disabled !" : "Il y'a eut un problème quand le bot a essayé de DM le gars. Peut être que ses MP ne sont pas publiques !");
            }
        }
        [Command("support")]
        [RequireContext(ContextType.Guild)]
        public async Task ReplyGuild()
        {
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            bool isenglish = language == "en";
            string[] ownerinfos = GetOwnerInfos.Infos();
            await ReplyAsync("Check mps!");
            var supp = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle(isenglish ? "To send a message to the staff, write ```+support [server id] [message] ```in bot DM" : "Pour envoyer un message au staff, faites ```+support [id du serveur] [message]``` dans les DM du bot")
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            try
            {
                await UserExtensions.SendMessageAsync(Context.Message.Author, "", false, supp.Build());
            }
            catch (HttpException e)
            {
                await ReplyAsync($"{e.HttpCode}\n{e.Reason}");
                await ReplyAsync(isenglish ? "It looks like there was a problem DMing you. Check to see if you have your DMs disabled and try again!" : "Il y'a eut un problème quand le bot a essayé de te MP. Sois sur que tes MP sont publiques et réessayes!");
            }
        }
    }
}