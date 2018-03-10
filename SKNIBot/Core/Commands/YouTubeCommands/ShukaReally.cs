using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class ShukaReallyCommand : PostMovieCommand
    {
        [Command("shukareally")]
        [Description("Shuka really!")]
        public async Task ShukaReally(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=DCZPWAnXmx4", member);
        }
    }
}
