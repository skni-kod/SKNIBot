using System;
using System.Text;


namespace SKNIBot.Core.Commands.ManagementCommands
{
    [CommandsGroup("Tekst")]
    public class PingCommand : BaseCommandModule
    {
        [Command("odrwoc")]
        [Description("Odwraca tekst.")]
        public async Task Reverse(CommandContext ctx, [RemainingText] string message = null)
        {
            await ctx.TriggerTypingAsync();

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < message.Length; i++)
            {
                str.Append(message[message.Length - 1 - i]);
            }
            string mes = str.toString();
            await ctx.RespondAsync(mes);
        }
    }
}
