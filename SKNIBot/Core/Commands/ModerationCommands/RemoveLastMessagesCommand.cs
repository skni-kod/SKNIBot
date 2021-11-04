using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class RemoveLastMessagesCommand : BaseCommandModule
    {
        [Command("usuń")]
        [Aliases("usun", "delete")]
        [Description("Usuwa ostatnie x wiadomości.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task RemoveLastMessages(CommandContext ctx, [Description("Liczba ostatnich wiadomości do usunięcia.")] int messagesCount)
        {
            var messages = await ctx.Channel.GetMessagesAsync(Math.Min(messagesCount + 1, 100));
            await ctx.Channel.DeleteMessagesAsync(messages, "Usuniecie wiadomości przez " + ctx.User.Username + ":" + ctx.User.Discriminator);
        }
    }
}
