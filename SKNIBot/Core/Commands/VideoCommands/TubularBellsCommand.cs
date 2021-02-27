using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.VideoCommands
{
    [CommandsGroup("Wideo")]
    class TubularBellsCommand : BaseCommandModule
    {
        static List<string> list = new List<string>(new string[] {
            "https://youtu.be/enuOArEfqGo?t=40",
            "https://youtu.be/enuOArEfqGo?t=65",
            "https://youtu.be/enuOArEfqGo?t=160",
            "https://youtu.be/enuOArEfqGo?t=185"
        }
        );
        [Command("dzwonnik")]
        [Description("Taki tam dzwonnik rurowy")]
        public async Task Avatar(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            Random rnd = new Random();
            int index = rnd.Next(0, list.Count);

            await ctx.RespondAsync("A trzeba było zostać dzwonnikiem rurowym...\n" + list[index]);

        }
    }
}
