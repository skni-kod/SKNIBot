using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using System;
using SKNIBot.Core.Const.GamesConst;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class HangmanCommand
    {
        [Command("wisielec")]
        [Description("Gra w wisielca.")]
        [Aliases("hangman")]
        public async Task Czesc(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            string lol = "";
            for (int i = 0; i < HangmanConst.stages.Length; i++)
                for (int j = 0; j < HangmanConst.stages[i].Length; j++)
                    lol += HangmanConst.stages[i][j] += "\n";
            await ctx.RespondAsync(lol);
        }
    }
}
