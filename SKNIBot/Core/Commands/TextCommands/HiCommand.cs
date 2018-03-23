using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class HiCommand
    {
        private List<string> _developers = new List<string> { "Azux Dario2860", "TemporaryNick7229", "Coedo2037"};

        [Command("cześć")]
        [Description("Przywitaj się.")]
        [Aliases("hello", "hi")]
        public async Task Czesc(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            if (_developers.Contains(ctx.User.Username + ctx.User.Discriminator ))
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
