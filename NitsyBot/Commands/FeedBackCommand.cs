using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using NitsyBot.Core.Singleton;
using NitsyBot.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NitsyBot.Core.Structs;
namespace NitsyBot.Commands
{
    public class FeedBackCommand : ModuleBase<SocketCommandContext>
    {
        [Command("feedback")]
        [RequireContext(ContextType.DM)]
        public async Task SendFeedBack([Remainder]string content = null)
        {
            EmbedBuilder fileType = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                    IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                },
                Title = "Only .png and .jpg files are allowed!",
                Color = Color.Red
            };

            EmbedBuilder feedback = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                    IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                },
                Title = "Feedback",
                Color = new NitsyBotUtilities().GeneratedColor,
                ThumbnailUrl = Context.Message.Author.GetAvatarUrl(),
                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "User",
                        Value = Context.Message.Author,
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Id",
                        Value = Context.Message.Author.Id,
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Message",
                        Value = content,
                        IsInline = false
                    }
                }
            };

            if (Context.Message.Attachments.Count > 0)
            {
                string url = Context.Message.Attachments.First().Url;
                if (NitsyBotUtilities.IsImage(url))
                    feedback.ImageUrl = url;
                else
                {
                    await ReplyAsync("", false, fileType.Build());
                    return;
                }
            }

            SocketTextChannel owner = SingletonClass.Instance.client.GetChannel(615615178981244958) as SocketTextChannel;
            EmbedBuilder embed = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                    IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                },
                Title = "Your message was sent and a developer will reply as soon as possible.",
                Color = new NitsyBotUtilities().GeneratedColor
            };

            await ReplyAsync("", false, embed.Build());
            await owner.SendMessageAsync("", false, feedback.Build());
        }

        [Command("nitsy")]
        public async Task Reply()
        {
            EmbedBuilder nitsy = new EmbedBuilder
            {
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                    IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                },
                Title = "To send a message to the developers, kindly write down **+feedback [your message]**.",
                Color = new NitsyBotUtilities().GeneratedColor,
                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "You can attach to your message one picture (only .png and .jpg files are supported).",
                        Value = "Please if you have any suggestion , bug report or you need help , send a message using this command and one of the developer will reply to your as soon as possible.Thank you!",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "NB",
                        Value = "Be sure you have enabled private messages else the developers will not be able to reply to you.",
                        IsInline = false
                    }
                }
            };

            await ReplyAsync("Check mps!");
            await UserExtensions.SendMessageAsync(Context.Message.Author, "", false, nitsy.Build());
        }

        [Command("dev")]
        public async Task OwnerReply(ulong id, [Remainder]string message = null)
        {
            //right user (maybe) but wrong channel
            //can't be used in 'else if'
            if (Context.Channel.Id != 615615178981244958)
            {
                await ReplyAsync("You cannot use this command in this channel!");
                return;
            }

            if (Context.Message.Author.Id == (ulong)IDS.OwnerID || Context.Message.Author.Id == 266533773284212736)
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                        IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                    },
                    Title = "Your reply was sent.",
                    Color = new NitsyBotUtilities().GeneratedColor
                };

                EmbedBuilder fileType = new EmbedBuilder
                {
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                        IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                    },
                    Title = "Only .png and .jpg files are allowed!",
                    Color = Color.Red
                };

                EmbedBuilder psycho = new EmbedBuilder
                {
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"Made by {SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Username}#{SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).Discriminator}",
                        IconUrl = SingletonClass.Instance.client.GetUser((ulong)IDS.OwnerID).GetAvatarUrl()
                    },
                    Title = "Feedback reply",
                    Color = new NitsyBotUtilities().GeneratedColor,
                    Fields = new List<EmbedFieldBuilder>
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Developer",
                            Value = Context.Message.Author,
                            IsInline = false
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Reply",
                            Value = message,
                            IsInline = false
                        }
                    }
                };

                if (Context.Message.Attachments.Count > 0)
                {
                    string url = Context.Message.Attachments.First().Url;
                    if (NitsyBotUtilities.IsImage(url))
                        psycho.ImageUrl = url;
                    else
                    {
                        await ReplyAsync("", false, fileType.Build());
                        return;
                    }
                }

                SocketUser channel = SingletonClass.Instance.client.GetUser(id);
                try
                {
                    await ReplyAsync("", false, embed.Build());
                    await channel.SendMessageAsync("", false, psycho.Build());
                }
                catch (HttpException)
                {
                    await ReplyAsync("This user has disabled private messages.");
                }
            }
            else
                await ReplyAsync("Only the developers can use this command!");
        }
    }
}