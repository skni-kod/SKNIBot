using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.MessageResponds
{
    public class XDMessageRespond
    {
        [MessageRespond]
        public static async Task Respond(DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            var msg = args.Message.Content.ToLower();
            if (msg.Contains("xd") || msg.Contains("iks de") || msg.Contains("eks di"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("XD");
            }
        }
    }
}
