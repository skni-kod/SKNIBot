using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class RemoveMessagesToCommand : BaseCommandModule
    {
        [Command("usuńDoID")]
        [Aliases("usunDoID", "deleteToID")]
        [Description("Usuwa wiadomości do wiadomości o podanym id włącznie.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task RemoveLastMessagesTo(CommandContext ctx, [Description("Liczba ostatnich wiadomości do usunięcia.")] ulong messageID)
        {
            //Usunięcie też naszego polecenia
            var messages = await ctx.Channel.GetMessagesAsync(1);
            await ctx.Channel.DeleteMessageAsync(messages.First(), "Usuniecie wiadomości przez " + ctx.User.Username + ":" + ctx.User.Discriminator);
            messages = await ctx.Channel.GetMessagesAsync(1);
            do
            {
                await ctx.Channel.DeleteMessageAsync(messages.First());
                messages = await ctx.Channel.GetMessagesAsync(1);
                if (messages.Count == 0) break;
            }
            while (messages.First().Id != messageID);

        }
    }
}
