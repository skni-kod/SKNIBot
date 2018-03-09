using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class KusokCommand
    {
        [Command("kusok")]
        [Description("Kanał Kusoka.")]
        public async Task Kusok(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("https://www.youtube.com/channel/UC6qnBvPmCZEOHOVaaXPLZhg");
        }
    }
}
