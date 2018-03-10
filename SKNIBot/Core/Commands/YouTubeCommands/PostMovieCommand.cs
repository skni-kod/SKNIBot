using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    public class PostMovieCommand
    {
        public async Task postMovie(CommandContext ctx, string movieUrl, DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            if (member == null)
            {
                await ctx.RespondAsync(movieUrl);
            }
            else
            {
                await ctx.RespondAsync(movieUrl + " " + member.Mention);
            }
        }
    }
}
