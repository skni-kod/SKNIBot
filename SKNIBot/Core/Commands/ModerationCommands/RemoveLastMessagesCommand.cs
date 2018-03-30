using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class RemoveLastMessagesCommand
    {
        [Command("usuń")]
        [Aliases("usun")]
        [Description("Usuwa ostatnie x wiadomości.")]
        [RequireRolesAttribute("Projekt - Bot")]
        public async Task RemoveLastMessages(CommandContext ctx, [Description("Liczba ostatnich wiadomości do usunięcia.")] int messagesCount)
        {
            var messages = await ctx.Channel.GetMessagesAsync(Math.Min(messagesCount + 1, 100));
            await ctx.Channel.DeleteMessagesAsync(messages);
        }
    }
}
