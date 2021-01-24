using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core.Utilities;
using System.Threading.Tasks;
using Discord;
using NitsyBot.Core.Singleton;
using NitsyBot.Core;
using System.Linq;
using NitsyBot.Core.Structs;

namespace NitsyBot.DataBase
{
    public class SetDataBases : ModuleBase<SocketCommandContext>
    {
        
        
        //private const string shopChannel = "ShopChannel"; //coming soon
        [Command("SetLang")]
        public async Task SetLangAsync(string language)
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            
            if (language != "fr" && language != "en")
            {
                EmbedBuilder embed = new EmbedBuilder()
                    .WithFooter(ownerinfos[0], ownerinfos[1])
                    .WithTitle("Currently supported languages")
                    .AddField("english(default)", "en", false)
                    .AddField("french", "fr", false);
                await ReplyAsync("Invalid input!To set your server's language ,write `+SetLang [language]`");
                await ReplyAsync("", false, embed.Build());
                return;
            }
            if (!NitsyBotUtilities.HasPermissions(Context.Message.Author))
            {
                await ReplyAsync(language == "en" ? "Error! You need ManageGuild permission in order to exectute that command!" : "Erreur! Tu as besoin de la permission ManageGuild pour exécuter cette commande!");
                return;
            }
            SocketGuild guild = Context.Guild as SocketGuild;
            IMongoCollection<BsonDocument> langCollection = DataBaseSetup.DataBaseAccess(DataBases.languagedataBase, DataBases.languageCollection);
            IAsyncCursor<BsonDocument> documentExists = await langCollection.FindAsync(Builders<BsonDocument>.Filter.Eq("id", guild.Id.ToString()));
            BsonDocument document = new BsonDocument
                  {
                    { "name", guild.Name},
                    { "id", guild.Id.ToString()},
                    { "lang", language },
                  };
            if (await documentExists.FirstOrDefaultAsync() == null)
            {
                await langCollection.InsertOneAsync(document);
            }
            else
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", guild.Id.ToString());
                UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("lang", language);
                await langCollection.UpdateOneAsync(filter, update);
            }
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithFooter(ownerinfos[0], ownerinfos[1])
                .WithColor(new NitsyBotUtilities().GeneratedColor);
            if (language == "fr")
            {
                embedBuilder
               .AddField("Informations", "[salon] est soit l'ID du salon,soit #[salon]. Mais pas le nom du salon!!De plus,ID est optionnel.Si vous ne mettez rien après la commande +Set, le salon dans lequel vous avez fait la commande sera set")
               .AddField("+SetPatchNoteChannel [salon]", "Le salon dans lequel le bot va poster le patchnote", false)
               .AddField("+SetStatusChannel [salon]", "Le salon dans lequel le bot va poster le status des serveurs Fortnite en plus de la dernière version", false)
               .AddField("+SetAesChannel [salon]", "Le salon dans lequel le bot va poster les clés AES", false)
               //.AddField("+SetShopChannel [salon]","Le salon dans lequel le bot va poster la boutique",false) //coming soon
               .AddField("+SetNitsyChannel [salon]", "Le salon dans lequel le bot va poster ses nouveautés et son status", false)
               
               .AddField("+SetReportChannel [salon]", "Le salon dans lequel le bot va poster les logs des reports", false)
               .AddField("+SetSupportChannel [salon]", "Le salon pour recevoir les messages des membres qui ont besoin du support", false);
                await ReplyAsync("La langue de ce serveur est le français!", false, embedBuilder.Build());
            }
            else
            {
                embedBuilder
               .AddField("Informations", "[channel] is the channel's ID ,or #[channel]. But not the channel's name! Not putting anything after +Set command will set the channel where the command was executed")
               .AddField("+SetPatchNoteChannel [channel]", "The channel where the bot will poste the patchnote", false)
               .AddField("+SetStatusChannel [channel]", "The channel where the bot will poste Fortnite servers' status as well as latest version", false)
               .AddField("+SetAesChannel [channel]", "The channel where the bot will post the AES keys", false)
               //.AddField("+SetShopChannel [channel]","The channel where the bot will post the shop",false) //coming soon
               .AddField("+SetNitsyChannel [channel]", "The channel where the bot will post its changelog and maintenance", false)
              
               .AddField("+SetReportChannel [channel]", "The channel where the bot will post users' reports logs", false)
               .AddField("+SetSupportChannel [channel]", "The channel to recieve the messages of users who need help", false);
                await ReplyAsync("The language for this guild has been set to english!", false, embedBuilder.Build());
            }
        }

        [Command("SetPatchNoteChannel")]
        public async Task SetPNChanAsync(string channel = null) => await GeneralChannelSet(channel, DataBases.patchnoteCollection);

        [Command("SetStatusChannel")]

        public async Task SetStatusChanAsync(string channel = null) => await GeneralChannelSet(channel, DataBases.statusCollection);

        [Command("SetAesChannel")]
        public async Task SetAesChanAsync(string channel = null) => await GeneralChannelSet(channel, DataBases.aesCollection);

        [Command("SetNitsyChannel")]
        public async Task SetNitsyChanAsync(string channel = null) => await GeneralChannelSet(channel, DataBases.nitsyCollection);

       

        [Command("SetReportChannel")]
        public async Task SetReportChanAsync(string channel = null) => await GeneralChannelSet(channel, DataBases.reportCollection);

        [Command("SetSupportChannel")]
        public async Task SetSupport(string channel = null) => await GeneralChannelSet(channel, DataBases.supportCollection);

        /*[Command("SetShopChannel")]
        public async Task SetShopAsync(string channel = null) => await new SetDataBases().GeneralChannelSet(channel, shopChannel);*/
        //Coming soon

        private static async Task GiveNitsyPerms(SocketTextChannel channel) => await channel.AddPermissionOverwriteAsync(SingletonClass.Instance.client.CurrentUser as IUser, OverwritePermissions.AllowAll(channel));
        private async Task GeneralChannelSet(string channelId, string databaseCollection)
        {
            SocketGuild guild = Context.Guild as SocketGuild;
            string language = GetGuildLanguage.languages[guild.Id.ToString()];
            string channel = (!string.IsNullOrEmpty(channelId) ? channelId : Context.Channel.Id.ToString()).Replace("<#", "").Replace(">", "");
            if (!NitsyBotUtilities.HasPermissions(Context.Message.Author))
            {
                await ReplyAsync(language == "en" ? "Error! You need ManageGuild permission in order to exectute that command!" : "Erreur! Tu as besoin de la permission ManageGuild pour exécuter cette commande!");
                return;
            }
            if (!ChannelIsValid(channel))
            {
                await ReplyAsync("Invalid input!");
                return;
            }
            
            IMongoCollection<BsonDocument> channelsCollection = DataBaseSetup.DataBaseAccess(DataBases.channelsDataBase, databaseCollection);
            
            BsonDocument document = new BsonDocument()
                {
                    { "name", guild.Name},
                    { "id", guild.Id.ToString()},
                     { "channel", channel },
                };
            if ((await channelsCollection.FindAsync(Builders<BsonDocument>.Filter.Eq("id", guild.Id.ToString()))).FirstOrDefault() == null)
            {
                await channelsCollection.InsertOneAsync(document);
            }
            else
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", guild.Id.ToString());
                UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("channel", channel);
                await channelsCollection.UpdateOneAsync(filter, update);
            }
            string lang;
            if (language == "en")
            {
                lang = $"The channel <#{guild.GetChannel(ulong.Parse(channel)).Id}> has been sucessfuly set!";
            }
            else
            {
                lang = $"Le channel <#{guild.GetChannel(ulong.Parse(channel)).Id}> a bien été set!";
            }
            await ReplyAsync(lang);
            await GiveNitsyPerms(guild.GetChannel(ulong.Parse(channel)) as SocketTextChannel);
        }
        private static bool ChannelIsValid(string channel) => channel.Length == 18 && channel.All(c => "0123456789".Contains(c));
    }
}