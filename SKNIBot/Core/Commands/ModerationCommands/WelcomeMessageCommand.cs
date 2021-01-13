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
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
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
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
        public async Task SetWelcomeMessage(CommandContext ctx, [Description("Kanał do wysyłania wiadomości.")]  DiscordChannel channel, [Description("Treść wiadomości.")] [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();
            _welcomeMessageService.SetWelcomeMessage(ctx.Guild.Id, channel.Id, message);
            await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Ustawiono nową wiadomość powitalną");
        }

        [Command("zmienKanalWiadomosciPowitalnej")]
        [Description("Zmienia kanał wiadomości powitalnej. Wiadomość musi być uprzednio ustawiona na serwerze.")]
        [Aliases("zmieńKanałWiadomościPowitalnej")]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
        public async Task ChangeWelcomeMessageChannel(CommandContext ctx, [Description("Nowy kanał do wysyłania wiadomości.")] DiscordChannel channel)
        {
            await ctx.TriggerTypingAsync();
            if(_welcomeMessageService.IsWelcomeMessageOnServer(ctx.Guild.Id))
            {
                _welcomeMessageService.ChangeWelcomeMessageChannel(ctx.Guild.Id, channel.Id);
                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Zmieniono kanał wiadomości powitalnej.");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Najpierw ustaw wiadomość powitalną.");
            }
        }

        [Command("zmienTrescWiadomosciPowitalnej")]
        [Description("Zmienia treść wiadomości powitalnej. Wiadomość musi być uprzednio ustawiona na serwerze.")]
        [Aliases("zmieńTreśćWiadomościPowitalnej")]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
        public async Task ChangeWelcomeMessageContent(CommandContext ctx, [Description("Nowa treść wiadomości.")][RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();
            if (_welcomeMessageService.IsWelcomeMessageOnServer(ctx.Guild.Id))
            {
                _welcomeMessageService.ChangeWelcomeMessageContent(ctx.Guild.Id, message);
                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Zmieniono treść wiadomości powitalnej.");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Najpierw ustaw wiadomość powitalną.");
            }
        }

        [Command("usunWiadomoscPowitalna")]
        [Description("Usuwa wiadomość powitalną.")]
        [Aliases("usuńWiadomośćPowitalną")]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
        public async Task RemoveelcomeMessage(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            if (_welcomeMessageService.IsWelcomeMessageOnServer(ctx.Guild.Id))
            {
                _welcomeMessageService.RemoveWelcomeMessage(ctx.Guild.Id);
                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Wiadomość powitalna została usunięta.");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wiadomość powitalna", "Na serwerze nie ma wiadomości powitalnej.");
            }
        }
    }
}
