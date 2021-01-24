using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using NitsyBot.AutoGetters;
using NitsyBot.Checkers;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Structs;
using NitsyBot.DataBase;
using NitsyBot.Events;
using System;
using System.Reflection;
using System.Threading.Tasks;
namespace NitsyBot.Core.Setup
{
    public class BotSetup
    {
        #region VARIABLES
        public static string[] verfest; //fnversion + manifest
        public static string fnversion; //fnversion from benbot api
        public static string status; //fortnite default status
        public static string aeskeys; //get aes keys
        private const string botToken = "NjAzMzMxOTM1MjU0NzQxMDI0.XfajTw.gZKCLR-NyCC-FHb5ZFhVtBid6J4";
        public const string twitterKey = "JVm7PLm16JpPa8pB1XgGnRTgS";
        public const string twitterkeySecret = "Zt5BpJjasfCAOshEAYfIhluhgWZvOvPtgW6n4FOdxOnRaN2Kd3";
        public const string twitterToken = "932163845749010432-2jxLfPqoQ75RKsmHKweDpo74vOE9Ptg";
        public const string twittertokenSecret = "uR9BCHZFoAm9NSuNeA0YpdP4kpI9YJrDs2mDdfflBveqP";
        public static string version;//read patchnote url
        public static string patchnote;//patchnote
        public static string frtranslation; //get french translartion
        public static bool success; //temporary to check if the setup was successful
        #endregion VARIABLES


        public  static void Main()
        {
            new DataBaseClient(DataBaseCredentials.credentials);
            new BotSetup().MainAsync().GetAwaiter().GetResult();
        }


        public async Task<bool> SetEverythingUp()
        {
            try
            {
                dynamic versionnum = JsonConvert.DeserializeObject(await new GetFortniteVersion().GetContentAsync());
                fnversion = versionnum.currentFortniteVersion;
                version = versionnum.currentFortniteVersionNumber;
                aeskeys = await new GetAES().GetContentAsync();
                frtranslation = await new GetFrenchTranslation().GetContentAsync();
                string twitter = await new GetTwitter(twitterKey, twitterkeySecret, twitterToken, twittertokenSecret).GetContentAsync();
                //GetAutoLocation.oldLocation = String.IsNullOrEmpty(twitter) ? "No location" : twitter;
                patchnote = await new GetPatchNote().GetContentAsync();
                status = await new GetServersStatus().GetContentAsync();

                verfest = await new AuthLess().GetManifest();
                Console.WriteLine("Download has finished and was successful!");
                success = true;
            }
            catch (Exception a)
            {
                Console.WriteLine($"{a.Message}\n{a.InnerException}\n{a.TargetSite}\n{a.Source}");
                success = false;
            }
            return success;
        }

        public void StartTheTimers()
        {
            //GetAutoLocation.Tim();
            VerifyTranslation.TimerTrans();
            AutoFortniteUpdate.Timer3();
            AutoPatchNote.Timer1();
            new AutoStatus().Timer();
            AutoAes.Timer2();
            AutoManifest.TimerMani();
            AutoManifest.TimerManifest();
        }

        public async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Downloading required stuff to memory, please wait . . .");
            if (await SetEverythingUp())
            {
                StartTheTimers();
            }
            else
            {
                Retry.RetryTimer();
            }
            new SetDynamicStatus().TimerStatus();
            Console.ForegroundColor = ConsoleColor.White;
            DatabaseDelete db = new DatabaseDelete();
            OnGuildJoined join = new OnGuildJoined();
            OnMessageRecieved msg = new OnMessageRecieved();
            OnBotReady ready = new OnBotReady();
            SingletonClass.Instance.client = new DiscordSocketClient();
            SingletonClass.Instance.client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });
            SingletonClass.Instance.Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });
            await SingletonClass.Instance.Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            SingletonClass.Instance.client.Log += Log;
            await SingletonClass.Instance.client.LoginAsync(TokenType.Bot, botToken);
            await SingletonClass.Instance.client.StartAsync();
            SingletonClass.Instance.client.MessageReceived += msg.HandleCommandAsync;
            SingletonClass.Instance.client.JoinedGuild += join.Joined;
            SingletonClass.Instance.client.LeftGuild += db.DeleteGuild;
            SingletonClass.Instance.client.Ready += ready.Ready;
            await Task.Delay(-1);
        }
        private Task Log(LogMessage messsge)
        {
            Console.WriteLine(messsge.ToString());
            return Task.CompletedTask;
        }
    }
}