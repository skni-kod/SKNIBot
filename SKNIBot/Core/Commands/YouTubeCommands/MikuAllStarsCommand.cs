using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class MikuAllStarsCommand : PostMovieCommand
    {
        [Command("mikuallstars")]
        [Description("Sing!")]
        public async Task MikuAllStars(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=vLEs8iOFkAU", member);
        }
    }
}
