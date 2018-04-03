using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class RemoveMessageCommand
    {
        [Command("usuńID")]
        [Aliases("usunID", "deleteID")]
        [Description("Usuwa konkretną wiadomość.")]
        [RequireRolesAttribute("Projekt - Bot")]
        public async Task RemoveTest(CommandContext ctx, [Description("ID wiadomości.")] ulong ID)
        {
            var message = await ctx.Channel.GetMessageAsync(ID);
            await ctx.Channel.DeleteMessageAsync(message);
            message = await ctx.Channel.GetMessageAsync(ctx.Message.Id);
            await ctx.Channel.DeleteMessageAsync(message);
        }
    }
}
