using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.WelcomeMessageService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class WelcomeMessageCommand : BaseCommandModule
    {
        private WelcomeMessageService _welcomeMessageService;

        public WelcomeMessageCommand(WelcomeMessageService welcomeMessageService)
        {
            _welcomeMessageService = welcomeMessageService;
        }

        [Command("pokazWiadomoscPowitalna")]
        [Description("Pokazuje czy wiadomość powitalna istnieje, jej kanał oraz treść.")]
        [Aliases("pokażWiadomośćPowitalną")]
        public async Task ShowWelcomeMessage(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var welcomeMessage = _welcomeMessageService.GetWelcomeMessage(ctx.Guild.Id);
            if(welcomeMessage.Exist == true)
            {
                StringBuilder Response = new StringBuilder();
                var channel = ctx.Guild.GetChannel(welcomeMessage.ChannelID.Value);
                if(channel != null)
                {
                    Response.Append("Kanał: ").AppendLine(channel.Name);
                }
                else
                {
                    Response.AppendLine("Kanał dla wiadomości powitalnej został usunięty");
                }

                Response.AppendLine("Treść: ").AppendLine(welcomeMessage.Content);

                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", Response.ToString());
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Na tym serwerze nie ma ustawionej wiadomości powitalnej");
            }
        }

        [Command("ustawWiadomoscPowitalna")]
        [Description("Ustawia kanał i treść powitalnej widomości./nMożna użyć `{UserMention}` jako miejsca do wstawienia wzmianki.")]
        [Aliases("ustawWiadomośćPowitalną")]
        public async Task SetWelcomeMessage(CommandContext ctx, [Description("Kanał do wysyłania wiadomości.")]  DiscordChannel channel, [Description("Treść wiadomości.")] [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();
            _welcomeMessageService.SetWelcomeMessage(ctx.Guild.Id, channel.Id, message);
            await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Ustawiono nową wiadomość powitalną");
        }
    }
}
