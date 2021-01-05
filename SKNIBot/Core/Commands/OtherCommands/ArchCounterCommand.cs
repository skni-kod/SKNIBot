using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    class ArchCounterCommand : BaseCommandModule
    {
        int _counter;

        [Command("ArchCounter")]
        [Description("Licznik nieudanych instalacji Arch Linuksa. Dostępne akcje: add, value, reset")]
        public async Task ArchCounter(CommandContext ctx, [Description("Akcja")] string action)
        {
            string actionLower = action.ToLower();

            switch (actionLower)
            {
                case "add":

                    _counter++;
                    await ctx.RespondAsync($"Dodano. Aktualna ilość nieudanych instalacji: {_counter}");
                    break;

                case "value":
                    await ctx.RespondAsync($"Aktualna ilość nieudanych instalacji: {_counter}");

                    break;

                case "reset":
                    _counter = 0;
                    await ctx.RespondAsync("Zresetowano");

                    break;

            }

        }
    }
}
