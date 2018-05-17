using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class RemoveMessageCommand
    {
        [Command("usuńID")]
        [Aliases("usunID", "deleteID")]
        [Description("Usuwa konkretną wiadomość.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task RemoveTest(CommandContext ctx, [Description("ID wiadomości.")] ulong messageID)
        {
            var message = await ctx.Channel.GetMessageAsync(messageID);
            await ctx.Channel.DeleteMessageAsync(message);
            message = await ctx.Channel.GetMessageAsync(ctx.Message.Id);
            await ctx.Channel.DeleteMessageAsync(message);
        }
    }
}
