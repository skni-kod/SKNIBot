using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class RemoveLastMessagesGroupCommand
    {
        [Command("usuńGrupowo")]
        [Aliases("usunGrupowo", "deleteGroup")]
        [Description("Usuwa ostatnie x wiadomości w grupach.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task RemoveLastMessagesGroup(CommandContext ctx, [Description("Liczba usunięć.")] int deleteCount, [Description("Liczba wiadomości do usunięcia w grupie.")] int messagesCount)
        {
            var messages = await ctx.Channel.GetMessagesAsync(1);
            await ctx.Channel.DeleteMessagesAsync(messages);
            for (int i = 0; i < deleteCount; i++)
            {
                messages = await ctx.Channel.GetMessagesAsync(Math.Min(messagesCount, 100));
                if (messages.Count == 0) break;
                await ctx.Channel.DeleteMessagesAsync(messages);
            }
        }
    }
}
