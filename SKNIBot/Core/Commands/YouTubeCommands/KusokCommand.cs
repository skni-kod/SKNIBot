using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class KusokCommand
    {
        [Command("kusok")]
        [Description("Kanał Kusoka.")]
        public async Task Kusok(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            if (member == null)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("https://www.youtube.com/channel/UC6qnBvPmCZEOHOVaaXPLZhg");
            }
            else
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("https://www.youtube.com/channel/UC6qnBvPmCZEOHOVaaXPLZhg " + member.Mention);
            }
        }
    }
}
