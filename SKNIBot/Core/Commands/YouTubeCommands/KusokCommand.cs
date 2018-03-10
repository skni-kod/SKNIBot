using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class KusokCommand : PostMovieCommand
    {
        [Command("kusok")]
        [Description("Kanał Kusoka.")]
        public async Task Kusok(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/channel/UC6qnBvPmCZEOHOVaaXPLZhg", member);
        }
    }
}
