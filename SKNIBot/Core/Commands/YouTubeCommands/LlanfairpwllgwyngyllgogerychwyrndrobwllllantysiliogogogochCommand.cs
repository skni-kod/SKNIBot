using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class LlanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogochCommand : PostMovieCommand
    {
        [Command("llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch")]
        [Description("llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch!")]
        public async Task Llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await postMovie(ctx, "https://youtu.be/fHxO0UdpoxM", member);
        }
    }
}
