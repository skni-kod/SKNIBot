using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.MessageResponds
{
    public class SomeoneCalledMessageRespond
    {
        [MessageRespond]
        public static async Task Respond(DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            if (args.Message.Content.ToLower().Contains("bocik"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("Ktoś mnie wołał?");
            }
        }
    }
}
