using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class ChocolateCommand : PostMovieCommand
    {
        [Command("chocolate")]
        [Description("Czekolada!")]
        public async Task Chocolate(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=WIKqgE4BwAY", member);
        }
    }
}
