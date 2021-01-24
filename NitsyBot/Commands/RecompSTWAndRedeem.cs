using Discord;
using Discord.Commands;
using Flurl;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NitsyBot.Core;
using NitsyBot.Core.Crypt;
using NitsyBot.DataBase;
using NitsyBot.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NitsyBot.Commands
{
    public class RecompSTWAndRedeem : ModuleBase<SocketCommandContext>
    {
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
            email = emaildm.Trim();
            await ReplyAsync("Now , please write: ```+pass [your password]```(without [] of course)");
        }

        [Command("code")]
        [RequireContext(ContextType.DM)]
        public async Task Code(string code)
        {
            if (mfa == false)
            {
                return;
            }

            if (code.Length != 6)
            {
                await ReplyAsync("Code length should be 6 digits!");
                return;
            }
            if (!IsNumber(code))
            {
                await ReplyAsync("Code can only conatains digits!");
                return;
            }
            mfacode = code;

            await ApiSetup.GetCookies();

            string error = await ApiSetup.MFALogin();
            if (error.Contains("errors.com.epicgames.accountportal.mfa_code_invalid"))
            {
                await ReplyAsync("The code you entered is wrong!");
                return;
            }
            await ReplyAsync("Starting the TLS Handshake with host server. . .");
            await ApiSetup.ExchangeCode();
            await ApiSetup.Request();
            await Get();
        }

        private bool IsNumber(string code)
        {
            bool isnumber = code.All(c => "0123456789".Contains(c));
            return isnumber;
        }

        [Command("pass")]
        [RequireContext(ContextType.DM)]
        public async Task Pass(string passdm)
        {
            password = passdm.Trim();
            await ReplyAsync("Now , please write: ```+username [your username]```(without [] of course)");
        }

        [Command("username")]
        [RequireContext(ContextType.DM)]
        public async Task Username([Remainder]string usernamedm)
        {
            username = usernamedm.Trim();
            await ApiSetup.GetCookies();
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

        public async Task Get()
        {
            var cook = ApiSetup.cookies.GetCookies(new Uri("https://www.epicgames.com/id"));
            foreach (Cookie co in cook)
            {
                if (co.Name != "EPIC_DEVICE")
                {
                    co.Expired = true;
                }
            }
            mfa = false;
            await ReplyAsync("Perfect . . . Starting the operation . . .");
            var c = new ApiSetup();
            string end = await c.GetEndpoint("https://account-public-service-prod.ol.epicgames.com/account/api/public/account/displayName/" + username.Trim());

            if (!end.Contains("authentication_failed") && !end.Contains("account_not_found"))
            {
                string accountid = JsonConvert.DeserializeObject<AccountID>(end).Id;
                await ReplyAsync("You are connected to servers...claiming the price...");
                var urlid = Url.Combine("https://fortnite-public-service-prod11.ol.epicgames.com/fortnite/api/game/v2/profile/", accountid.Trim(), "/client/ClaimLoginReward?profileId=campaign");
                var urlid1 = Url.Decode(urlid, false);
                string ab = c.PostEndpoint(urlid1);

                var rew = new GetReward();
                if (!ab.Contains("check_access_failed") && !ab.Contains("missing_permission"))
                {
                    try
                    {
                        await ReplyAsync("You have successfully claimed your daily reward!", false, rew.GetRewardAsync(ab));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        await ReplyAsync("**You already claimed your rewards today!Come again tomorrow.**");
                    }
                }
                else if (ab.Contains("check_access_failed"))
                {
                    await ReplyAsync("This account doesn't have Save The World.Try again with an account **that has Save The World.**");
                }
                else if (ab.Contains("missing_permission"))
                {
                    await ReplyAsync("**The username you entered doesn't belong to the account with this email and password.Please write down the correct username**.");
                }
                await c.DeleteEndpoint("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/sessions/kill/" + ApiSetup.Token);
                await ReplyAsync("Killing the session to avoid throttling policy...");
                string checkkill = await c.CheckKill("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/verify");
                if (checkkill.Contains("oauth.invalid_token"))
                {
                    await ReplyAsync("The session was successfully killed!");
                }
            }
            else if (end.Contains("authentication_failed"))
            {
                await ReplyAsync("**Ouch!I couldn't connect to servers.Be sure the email and password you entered are correct and that the 2FA is disabled.**");
            }
            else if (end.Contains("account_not_found"))
            {
                await ReplyAsync("**Ouch!I couldn't find the account.Be sure that the username is correct.**");
            }
        }

        [Command("redeem")]
        [RequireContext(ContextType.DM)]
        public async Task RedeemAsync(string code)
        {
            DataBaseSetup db = new DataBaseSetup();
            IAsyncCursor<BsonDocument> cursorDB = null;// db.DataBaseAccess("STW", "Rewards");
            while (await cursorDB.MoveNextAsync())
            {
                foreach (var doc in cursorDB.Current)
                {
                    if (doc["id"].AsString == Context.Message.Author.Id.ToString())
                    {
                        if (code.Trim() == doc["code"].AsString)
                        {
                            email = DataCrypting.Decrypt(doc["email"].AsString.Replace("玩", "="), doc["key"].AsString, 256);
                            password = DataCrypting.Decrypt(doc["password"].AsString.Replace("戶", "="), doc["key"].AsString, 256);
                            username = DataCrypting.Decrypt(doc["username"].AsString.Replace("內", "="), doc["key"].AsString, 256);
                            await ApiSetup.GetCookies();
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
                        else
                        {
                            await ReplyAsync("The code you entered is wrong.");
                        }
                    }
                }
            }
        }

        public static string email { get; set; }
        public static string password { get; set; }
        public static string username { get; set; }
        public static string mfacode { get; private set; }
        public static bool mfa = false;
    }
}