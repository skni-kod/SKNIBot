using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ReverseCommand
{
    [CommandsGroup("Tekst")]
    public class ReverseCommand : BaseCommandModule
    {
        [Command("odwroc")]
        [Description("Odwraca tekst.")]
        public async Task Reverse(CommandContext ctx, [RemainingText] string message = null)
        {
            await ctx.TriggerTypingAsync();

            if (message == null) await ctx.RespondAsync("Podaj wiadomosc do odwrocenia!");
            else
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < message.Length; i++)
                {
                    str.Append(message[message.Length - 1 - i]);
                }
                string mes = str.ToString();
                await ctx.RespondAsync(mes);
            }
        }
    }
}
