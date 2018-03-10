using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class TooketeCommand : PostMovieCommand
    {
        [Command("todokete")]
        [Description("Snow halation!")]
        public async Task Todokete(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=TnLMlQJHAUU", member);
        }
    }
}
