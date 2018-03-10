using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class RasputinCommand : PostMovieCommand
    {
        [Command("rasputin")]
        [Description("Rasputin!")]
        public async Task Rasputin(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=3AeN-5PXaaQ", member);
        }
    }
}
