using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ManagementCommands
{
    [CommandsGroup("Zarządzanie")]
    class ChangeNameCommand : BaseCommandModule
    {
        [Command("zmieńNazwe")]
        [Aliases("changeName")]
        [Description("Zmienia nazwę bota.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task ChangeName(CommandContext ctx, [Description("Nowa nazwa bota.")] string name = null)
        {
           await Bot.DiscordClient.UpdateCurrentUserAsync(name);
        }
    }
}
