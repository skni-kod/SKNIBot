using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup]
    public class ChocolateCommand
    {
        [Command("chocolate")]
        [Description("Czekolada!")]
        public async Task Chocolate(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            if (member == null)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("https://www.youtube.com/watch?v=WIKqgE4BwAY");
            }
            else
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("https://www.youtube.com/watch?v=WIKqgE4BwAY " + member.Mention);
            }
        }
    }
}
