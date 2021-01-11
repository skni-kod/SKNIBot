using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Services.WelcomeMessageService
{
    public class WelcomeMessageResponse
    {
        public bool Exist { get; }
        public ulong? ServerId { get; }
        public ulong? ChannelID { get; }
        public string Content { get; }

        public WelcomeMessageResponse(ulong serverId, ulong channelId, string content)
        {
            Exist = true;
            ServerId = serverId;
            ChannelID = channelId;
            Content = content;
        }

        public WelcomeMessageResponse()
        {
            Exist = false;
            ServerId = null;
            ChannelID = null;
            Content = null;
        }
    }
}
