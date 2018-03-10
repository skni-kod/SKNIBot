using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class ShakaronCommand : PostMovieCommand
    {
        [Command("shakaron")]
        [Description("shakaron!")]
        [Aliases("makaron")]
        public async Task Shakaron(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=YTcYqLKfuac", member);
        }
    }
}
