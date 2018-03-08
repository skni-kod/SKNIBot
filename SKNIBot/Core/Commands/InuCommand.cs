using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class InuCommand
    {
        [Command("inu")]
        [Description("Display some cute dogs.")]
        [Aliases("pies", "dog")]
        public async Task Inu(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"Cute dog.jpg");
        }
    }
}
