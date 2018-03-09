using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class ChocolateCommand
    {
        [Command("chocolate")]
        [Description("Czekolada!")]
        public async Task Chocolate(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("https://www.youtube.com/watch?v=WIKqgE4BwAY");
        }
    }
}
