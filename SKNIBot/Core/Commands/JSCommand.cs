using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;

namespace SKNIBot.Core.Commands {
    [CommandsGroup]
    public class JSCommand {
        [Command("js")]
        [Description("Just Javascript.")]
        //[Aliases("pies", "dog")]
        public async Task JS(CommandContext ctx) {
          
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"JavaScript ssie");
        }
    }
}
