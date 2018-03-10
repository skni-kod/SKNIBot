using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class NikoNikoNiiCommand : PostMovieCommand
    {
        [Command("nikonikonii")]
        [Description("nico nico nii!")]
        [Aliases("niconiconii")]
        public async Task NikoNikoNii(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=3oD8tj3eBls", member);
        }
    }
}
