using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.MessageResponds
{
    class WhatIsLoveRespond
    {
        [MessageRespond]
        public static async Task Respond(Bot bot, DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            var message = args.Message.Content.ToLower();
            if (message.Contains("what is love"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("Baby don't hurt me,\nDon't hurt me, no more\nhttps://www.youtube.com/watch?v=HEXWRTEbj1I");
            }
            else if (message.Contains("zukyun"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("dokyun!");
            }
            else if(message.Contains("parabole tańczą"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("tańczą tańczą tańczą!");
            }
            else if (message.Contains("gura") || message.Contains("gawr"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("a");
            }
        }
    }
}
