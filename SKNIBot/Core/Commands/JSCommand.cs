using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class JSCommand
    {
        [Command("js")]
        [Description("Just Javascript.")]
        [Aliases("javascript")]
        public async Task JS(CommandContext ctx, [Description("The user to say hi to.")] DiscordMember member = null)
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
