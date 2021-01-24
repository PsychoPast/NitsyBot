using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using NitsyBot.Core;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Utilities;
using NitsyBot.DataBase;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private const string supportsev = "https://discordapp.com/invite/whKMgG5";
        private const string website = "https://nitsy.home.blog";
        private const string trello = "https://trello.com/b/4ZSJu4Ou/nitsy";

        [Command("help")]
        public async Task Help()
        {
            string[] ownerinfos = GetOwnerInfos.Infos();
            SocketGuild guild = Context.Guild; ;
            DataBaseSetup db = new DataBaseSetup();
            IAsyncCursor<BsonDocument> cursorLang = null;// await db.DataBaseLang(guild.Id.ToString());
            EmbedBuilder help = new EmbedBuilder().WithTitle("Commandes de Nitsy").WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).AddField("+patchnote", "Vous donne un lien vers le patch note de la version actuelle.", false).AddField("+status", "Vous donne le status des serveurs de Fortnite.", false).AddField("+help", "Vous montre ce message.", false).AddField("+aes", "Vous donne la clé aes des pak encryptés et non encryptés(si quelqu'un est intéressé à fouiller dans les fichiers du jeu , ces clés sont obligatoires).", false).AddField("+fnversion", "Vous donne la version actuelle de Fortnite.", false).AddField("+recompstw", "Vous permet de réclamer vos récompenses quotidiennes Sauver Le Monde sans se co au jeu!", false).AddField("+create", "Moyen facile de réclamer tes récompenses sur Sauver le Monde(recommandé).", false).AddField("+invite", "Vous permet d'obtenir un lien pour inviter Nitsy sur votre serveur.", false).AddField("+drop", "Laisse Nitsy choisir au hasard une ville/emplacement pour drop.", false).AddField("+helpcosm", "Vous apprend à utiliser le +search.", false).AddField("+shop", "Coming Soon", false).AddField("+report [user] [raison]", "Report un membre à la modération à condition que la modération est faite +SetReportChannel", false).AddField("+user-info", "Donne les infos sur un user", false).AddField("+serverinfo", "Donne des infos à propos du serveur.", false).AddField("+support", "Vous met en direct contact avec le staff.", false).AddField("+nitsy", "Vous met en contact avec les développeurs", false).AddField("Sans commande , le bot:", "-Poste le patch note automatiquement directement à sa sortie \n -Poste automatiquement quand les serveurs de Fortnite sont hors-ligne\n-Poste automatiquement les mise à jour Fortnite.", false).WithColor(new NitsyBotUtilities().GeneratedColor);
            EmbedBuilder helpen = new EmbedBuilder().WithTitle("Nitsy Commands").WithFooter($"Made by {ownerinfos[0]}", ownerinfos[1]).AddField("+patchnote", "Gives you the current fortnite version patch note.", false).AddField("+status", "Gives you Fortnite servers status.", false).AddField("+help", "Shows you this message.", false).AddField("+aes", "Gives you latest aes keys", false).AddField("+fnversion", "Gives you Fortnite current version.", false).AddField("+recompstw", "Claim your Save The World daily rewards without opening the game.", false).AddField("+create", "Easy way to claim your Save the World daily rewards (recommended).", false).AddField("+invite", "Gives you a link to invite Nitsy to your server.", false).AddField("+drop", "Nitsy chooses a random place on the Fortnite map for you to drop", false).AddField("+helpcosm", "Gives you help to use +search command", false).AddField("+shop", "Coming soon", false).AddField("+support", "Gives you direct contact with server staff.", false).AddField("+nitsy", "Gives you contact with the developers", false).AddField("Without any command , Nitsy can:", "-Post the patchnote automatically.\n-Post automatically when Fortnite servers are down or up.\n-Post automatically when Fortnite is updated.\nSoon, post the shop automatically.\nNB:For all these , you need to set the channels.", false).AddField("+report [user] [reason]", "Report a user to the moderation IF it did +SetReportChannel", false).AddField("+user-info", "Gives infos about a user", false).AddField("+serverinfo", "Gives info about the server", false).WithColor(new NitsyBotUtilities().GeneratedColor);
            try
            {
                while (await cursorLang.MoveNextAsync())
                {
                    foreach (var doc1 in cursorLang.Current)
                    {
                        string lang = doc1["lang"].AsString;
                        if (lang == "en")
                        {
                            await ReplyAsync("", false, helpen.Build());
                            await ReplyAsync($"Check the website for a more detailed commands list at: {website}");
                            await ReplyAsync($"Check the trello board to check for any known bug at: {trello}");
                            await UserExtensions.SendMessageAsync(Context.Message.Author, $"Join the support server for help: {supportsev}");
                        }
                        else if (lang == "fr")
                        {
                            await ReplyAsync("", false, help.Build());
                            await ReplyAsync($"Check le site web pour une liste des commandes plus détaillée: {website}");
                            await ReplyAsync($"Check le trello board pour voir la liste des bugs connus: {trello}");
                            await UserExtensions.SendMessageAsync(Context.Message.Author, $"Rejoins le serveur support pour demander de l'aide: {supportsev}");
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