using DSharpPlus;
using SKNIBot.Core.Services.WelcomeMessageService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Handlers.WelcomeMessageHandlers
{
    class WelcomeMessageHandler
    {
        private WelcomeMessageService _welcomeMessageService;

        public WelcomeMessageHandler(WelcomeMessageService welcomeMessageService)
        {
            _welcomeMessageService = welcomeMessageService;
        }

        public async void SendWelcomeMessage(DiscordClient client, DSharpPlus.EventArgs.GuildMemberAddEventArgs e)
        {
            if(_welcomeMessageService.IsWelcomeMessageOnServer(e.Guild.Id))
            {
                var welcomeMessage = _welcomeMessageService.GetWelcomeMessage(e.Guild.Id);

                var channel = e.Guild.GetChannel(welcomeMessage.ChannelID.Value);

                if (channel != null)
                {
                    await channel.SendMessageAsync(welcomeMessage.Content.Replace("{UserMention}", e.Member.Mention));
                }
            }
        }
    }
}
