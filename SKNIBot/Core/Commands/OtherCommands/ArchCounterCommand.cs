using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.ArchCounterService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    class ArchCounterCommand : BaseCommandModule
    {
        private ArchCounterService _archCounterService;
        public ArchCounterCommand(ArchCounterService archCounterService)
        {
            _archCounterService = archCounterService;
        }

        [Command("ArchCounter")]
        [Description("Licznik nieudanych instalacji Arch Linuxa. Dostępne akcje: `add`, `value`, `reset`")]
        public async Task ArchCounter(CommandContext ctx, [Description("Akcja")] string action = "value")
        {
            string actionLower = action.ToLower();

            switch (actionLower)
            {
                case "add":
                    await PostEmbedHelper.PostEmbed(ctx, "ArchCounter", "Dodano. Aktualna ilość nieudanych instalacji: " + _archCounterService.IncrementCounter(ctx.Guild.Id));
                    break;
                case "value":
                    await PostEmbedHelper.PostEmbed(ctx, "ArchCounter", "Aktualna ilość nieudanych instalacji: " + _archCounterService.GetCounter(ctx.Guild.Id));
                    break;
                case "reset":
                    _archCounterService.ResetCounter(ctx.Guild.Id);
                    await PostEmbedHelper.PostEmbed(ctx, "ArchCounter", "Zresetowano");
                    break;
                default:
                    await PostEmbedHelper.PostEmbed(ctx, "ArchCounter", "Dostępne akcje to: `add`, `value`, `reset`");
                    break;
            }
        }
    }
}
