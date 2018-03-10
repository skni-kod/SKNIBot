using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class MikuAllStarsCommand
    {
        [Command("mikuallstars")]
        [Description("Sing!")]
        public async Task MikuAllStars(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            if (member == null)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("https://www.youtube.com/watch?v=vLEs8iOFkAU");
            }
            else
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("https://www.youtube.com/watch?v=vLEs8iOFkAU " + member.Mention);
            }
        }
    }
}
