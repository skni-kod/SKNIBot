using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Other")]
    public class CompileCommand
    {
        [Command("compile")]
        [Description("asd")]
        public async Task SpaceX(CommandContext ctx, string code = null, string input)
        {
            await ctx.TriggerTypingAsync();
        }
    }
}
