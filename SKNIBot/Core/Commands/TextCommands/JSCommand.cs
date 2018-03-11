using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class JSCommand
    {
        [Command("js")]
        [Description("Just Javascript.")]
        [Aliases("javascript")]
        public async Task JS(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            //Jeżeli długość jest jeden nie podano kodu
            if (member == null)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("JavaScript ssie");
            }
            else
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("JavaScript ssie " + member.Mention);
            }
        }

    }
}
