using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class DoFlopCommand : PostMovieCommand
    {
        [Command("doflop")]
        [Description("do flop!")]
        public async Task DoFlop(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://youtu.be/tBJNYdHPcDE", member);
        }
    }
}
