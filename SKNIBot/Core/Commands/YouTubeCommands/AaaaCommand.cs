using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class AaaaCommand : PostMovieCommand
    {
        [Command("aaaa")]
        [Description("aaaa!")]
        public async Task Aaaa(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://www.youtube.com/watch?v=rvrZJ5C_Nwg", member);
        }
    }
}
