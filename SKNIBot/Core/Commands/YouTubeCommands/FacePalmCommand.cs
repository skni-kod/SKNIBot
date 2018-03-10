using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class FacePalmCommand : PostMovieCommand
    {
        [Command("facepalm")]
        [Description("face palm!")]
        public async Task FacePalm(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=XZxzJGgox_E", member);
        }
    }
}
