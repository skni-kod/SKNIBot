using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class MikuAllStarsCommand
    {
        [Command("mikuallstars")]
        [Description("Sing!")]
        public async Task MikuAllStars(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("https://www.youtube.com/watch?v=vLEs8iOFkAU");
        }
    }
}
