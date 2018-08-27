using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class SeppukuCommand : BaseCommandModule
    {
        [Command("Seppuku")]
        [Description("Zgiń honorową śmiercią!")]
        public async Task Seppuku(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            await ctx.Member.RemoveAsync("Popełnił Seppuku. Gloria Victis!");
            await ctx.RespondAsync($"{ctx.Member.DisplayName} popełnił Seppuku");
        }
    }
}
