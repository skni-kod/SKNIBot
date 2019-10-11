using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Helpers;
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
        [Description("Zmienia nazwę bota.\nMusisz być dopisany jako twórca bota aby wykonać tę komendę.")]
        public async Task ChangeName(CommandContext ctx, [Description("Nowa nazwa bota.")] [RemainingText] string name = null)
        {
            if(DeveloperHelper.IsDeveloper(ctx.User.Id))
            {
                await Bot.DiscordClient.UpdateCurrentUserAsync(name);
            }
            else
            {
                await ctx.RespondAsync("You aren't my father.");
            }
        }
    }
}
