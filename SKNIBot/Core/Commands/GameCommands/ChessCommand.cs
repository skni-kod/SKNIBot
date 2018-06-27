using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class ChessCommand
    {
        [Command("szachy")]
        [Description("Dev.")]
        [Aliases("chess", "c")]
        public async Task Hangman(CommandContext ctx)
        {
            await ctx.RespondAsync("test");
        }
    }
}
