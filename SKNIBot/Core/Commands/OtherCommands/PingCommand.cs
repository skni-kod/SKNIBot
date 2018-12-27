using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    public class PingCommand : BaseCommandModule
    {
        [Command("ping")]
        [Description("Wyświetl najnowszy ping.")]
        public async Task Avatar(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"{ctx.Client.Ping} ms");
        }
    }
}
