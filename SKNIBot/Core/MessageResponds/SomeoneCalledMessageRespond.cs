using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.MessageResponds
{
    public class SomeoneCalledMessageRespond
    {
        [MessageRespond]
        public static async Task Respond(Bot bot, DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            if (args.Message.Content.ToLower().Contains("bocik"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("Ktoś mnie wołał?");
            }
            if (args.Message.Content.ToLower().Contains("zensoku zenshin"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("yousoro!");
            }
            if (args.Message.Content.ToLower().Contains("kan kan"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("mikan");
            }
            if (args.Message.Content.ToLower().Contains("yoshiko"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("Yohane!");
            }
            if (args.Message.Content.ToLower().Contains("somebody"))
            {
                await args.Channel.TriggerTypingAsync();
                await args.Channel.SendMessageAsync("https://www.youtube.com/watch?v=L_jWHffIx5E");
            }
        }
    }
}
