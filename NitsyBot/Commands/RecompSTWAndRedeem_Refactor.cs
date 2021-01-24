using CFortniteAuth;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NitsyBot.Core.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class RecompSTWAndRedeem_Refactor : ModuleBase<SocketCommandContext>
    {
        private string mfacode;
        readonly FNAuth fNAuth;
        string email;
        string password;
        string username;
        ulong userid;
        public RecompSTWAndRedeem_Refactor(out string mfacode)
        {
            mfacode = this.mfacode;
        }
        [Command("recompstw")]
        public async Task Recomp()
        {
            await ReplyAsync("Checks mps!");
            await UserExtensions.SendMessageAsync(Context.Message.Author, "Hey!Please write: ```+email [your email]```(without [] of course)");
        }
        [Command("email")]
        [RequireContext(ContextType.DM)]
        public async Task Email(string emaildm)
        {
            if (!emaildm.Contains("@"))
            {
                await ReplyAsync("Please enter a valid email!");
                return;
            }
            email = Regex.Replace(emaildm, @"\s", "");
            await ReplyAsync("Now , please write: ```+pass [your password]```(without [] of course)");
        }
        [Command("pass")]
        [RequireContext(ContextType.DM)]
        public async Task Pass(string passdm)
        {
            password = Regex.Replace(passdm, @"\s", "");
            await ReplyAsync("Now , please write: ```+username [your username]```(without [] of course)");
        }
        [Command("username")]
        [RequireContext(ContextType.DM)]
        public async Task Username([Remainder]string usernamedm)
        {
            userid = Context.Message.Author.Id;
            username = Regex.Replace(usernamedm, @"\s", "");
            fNAuth.OnLoginAttempt += FNAuth_OnLoginAttempt;
            await fNAuth.Authenticate().ConfigureAwait(false);
            string ismfa = await ApiSetup.Login(email, password);

            if (ismfa.Contains("errors.com.epicgames.common.two_factor_authentication.required"))
            {
                mfa = true;
                await ReplyAsync("2FA is activated! Please do +code [your code]!");
                return;
            }
            else
            {
                await ApiSetup.ExchangeCode();
                await ApiSetup.Request();
                await Get();
            }
        }
        public async Task<string> AskForCode()
        {
            SocketTextChannel user = SingletonClass.Instance.client.GetChannel(userid) as SocketTextChannel;
            string message = "2FA is activated! Please write your 6 digits code.";
            await user.SendMessageAsync(message);
        NotYet:
            List<IMessage> messages = (await user.GetMessagesAsync(1).FlattenAsync().ConfigureAwait(false)).ToList<IMessage>();
            if (messages[0].Content == message)
            {
                goto NotYet;
            }
            if (messages[0]) { }
        }




        private async void FNAuth_OnLoginAttempt(LoginResult result)
        {
            if(!result.SuccessfulyLoggedIn)
            {
                if(result.MFAEnabled)
                {
                    string code = await AskForCode().ConfigureAwait(false);
                    await fNAuth.Authenticate(code).ConfigureAwait(false);
                }
            }
        }
    }
}
