using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class RemoveLastMessagesAltCommand : BaseCommandModule
    {
        [Command("usuńAlt")]
        [Aliases("usunAlt", "deleteAlt")]
        [Description("Usuwa ostatnie x wiadomości. Wolniejsze niż `usuń` ale pozwala usunąć więcej niż 100.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task RemoveLastMessagesAlt(CommandContext ctx, [Description("Liczba ostatnich wiadomości do usunięcia.")] int messagesCount)
        {
            //Usunięcie też naszego polecenia
            var messages = await ctx.Channel.GetMessagesAsync(1);
            await ctx.Channel.DeleteMessageAsync(messages.First(), "Usuniecie wiadomosci");
            while (messagesCount > 0)
            {
                messages = await ctx.Channel.GetMessagesAsync(1);
                if (messages.Count == 0) break;

                await ctx.Channel.DeleteMessageAsync(messages.First());
                messagesCount--;
            }
        }
    }
}
