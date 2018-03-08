using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class NekoCommand
    {
        [Command("neko")]
        [Description("Display some cute cat.")]
        [Aliases("kot", "cat")]
        public async Task Neko(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"Cute cat.jpg");
        }
    }
}
