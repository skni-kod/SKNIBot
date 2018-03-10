using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class HiCommand
    {
        [Command("cześć")]
        [Description("Przywitaj się.")]
        [Aliases("hello", "hi")]
        public async Task Czesc(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("Cześć " + ctx.User.Mention);
        }
    }
}
