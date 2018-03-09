using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class HiCommand
    {
        [Command("hi")]
        [Description("Przywitaj się.")]
        [Aliases("hello", "cześć")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("Hi " + ctx.User.Mention);
        }
    }
}
