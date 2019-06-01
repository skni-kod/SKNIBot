using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class HiCommand : BaseCommandModule
    {
        private List<ulong> _developers = new List<ulong> {
            386559253088829440, // Azux Dario#2860
            352775074513682434, // TemporaryNick#7229
            305642795706744833, // Coedo#2037
            231846704947658752, // Szatku#4105
            263061784762515457, // Amy#4589
            256313921164673024, // Suzuri#9636 
        };

        [Command("cześć")]
        [Description("Przywitaj się.")]
        [Aliases("hello", "hi")]
        public async Task Czesc(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            if (_developers.Contains(ctx.User.Id))
            {
                await ctx.RespondAsync("Cześć mój programisto " + ctx.User.Mention);
            }
            else
            {
                await ctx.RespondAsync("Cześć " + ctx.User.Mention);
            }
        }
    }
}
