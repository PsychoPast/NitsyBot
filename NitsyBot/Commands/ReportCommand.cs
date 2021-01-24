using Discord;
using Discord.Commands;
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
    public class ReportCommand : ModuleBase<SocketCommandContext>
    {
        [Command("report")]
        [Summary("Report a user")]
        public async Task ReportAsync(SocketUser user = null, [Remainder]string reason = "Undefined")
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            string language = GetGuildLanguage.languages[Context.Guild.Id.ToString()];
            bool isenglish = language == "en";
            if (user == null)
            {
                await ReplyAsync(isenglish ? "Please, specify a user!" : "Veuillez spécifier un utilisateur!");
                return;
            }
            else if (user == Context.Message.Author)
            {
                await ReplyAsync(isenglish ? "You cannot report yourself 🤦" : "Vous ne pouvez pas vous signaler 🤦");
                return;
            }
            EmbedBuilder filetype = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle(isenglish ? "Only .png and .jpg files are allowed!" : "Seulement les fichiers .png et .jpg sont autorisés")
                .WithColor(Color.Red);
            ReportsDataBase reportdatabase = new ReportsDataBase();
            SocketGuild guild = Context.Guild;
            await reportdatabase.CreateReportData(guild, user);
            int reportnum = await new GetReportNumber().GetReports(guild.Id.ToString(), user.Id.ToString());
            SocketRole role = (user as SocketGuildUser).Roles.OrderByDescending(x => x.Position).FirstOrDefault();
            SocketUser author = Context.User;
            EmbedAuthorBuilder embedAuthor = new EmbedAuthorBuilder()
                .WithName($"{author.Username}#{author.Discriminator}")
                .WithIconUrl(author.GetAvatarUrl()).WithUrl($"https://discordapp.com/channels/{Context.Guild.Id}/{Context.Channel.Id}/{Context.Message.Id}");
            EmbedBuilder report = new EmbedBuilder()
                .WithAuthor(embedAuthor)
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithThumbnailUrl(user.GetAvatarUrl())
                .WithTitle("[REPORT] **" + user.Username + "**")
                .AddField(isenglish ? "Reported Member" : "Membre signalé", user.Mention, false)
                .AddField(isenglish ? "Member" : "Membre", author.Mention, false)
                .AddField(isenglish ? "Reason" : "Raison", reason, false)
                .AddField("Channel", $"<#{Context.Channel.Id}>", false)
                .AddField("Message", "https://discordapp.com/channels/" + Context.Guild.Id + "/" + Context.Channel.Id + "/" + Context.Message.Id, false)
                .AddField("Date", $"{DateTime.Now.ToUniversalTime().ToString("dd/MM/yyyy")}{(isenglish ? "at" : "à")} {DateTime.Now.ToUniversalTime().ToString("HH:mm:ss")} UTC")
                .AddField(isenglish ? "Number of Reports" : "Nombre de reports", reportnum, false)
                .AddField(isenglish ? "Highest role on the server" : "Le plus haut role sur le serveur", role, false)
                .WithColor(role.Color);
            if (Context.Message.Attachments.Count != 0)
            {
                string url = Context.Message.Attachments.First().Url;
                if (NitsyBotUtilities.IsImage(url))
                {
                    report.WithImageUrl(url);
                }
                else
                {
                    await ReplyAsync("", false, filetype.Build());
                    return;
                }
            }
            EmbedBuilder smallreport = new EmbedBuilder()
                .WithFooter($"{ownerinfos[0]}", ownerinfos[1])
                .WithTitle("**[REPORT]**")
                .AddField(isenglish ? "Your report has been sent to the moderation.Thank you!" : "Votre rapport a été envoyé à la modération.Merci!", isenglish ? "Take , here is a 🍪" : "Tiens, voici un cookie", false)
                .WithColor(Color.Red);
            await ReplyAsync("", false, smallreport.Build());
            IMongoCollection<BsonDocument> collection = DataBaseSetup.DataBaseAccess(DataBases.channelsDataBase, DataBases.reportCollection);
            string channel = (await collection.FindAsync(Builders<BsonDocument>.Filter.Eq("id", guild.Id.ToString()))).FirstOrDefault()["channel"].AsString;
            await (SingletonClass.Instance.client.GetChannel(ulong.Parse(channel)) as SocketTextChannel).SendMessageAsync("",false,report.Build());
            await Context.Message.DeleteAsync();
        }
    }
} 