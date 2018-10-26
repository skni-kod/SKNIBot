using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.MessageResponds
{
    class WhatIsLoveRespond
    {
        [MessageRespond]
        public static async Task Respond(DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            if (args.Message.Content.ToLower().Contains("what is love"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("Baby don't hurt me,\nDon't hurt me, no more\nhttps://www.youtube.com/watch?v=HEXWRTEbj1I");
            }
            else if (args.Message.Content.ToLower().Contains("zukyun"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("dokyun!");
            }
            else if(args.Message.Content.ToLower().Contains("parabole tańczą"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("tańczą tańczą tańczą!");
            }
        }
    }
}
