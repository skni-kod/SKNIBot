using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class ChangeRoleColorCommand : BaseCommandModule
    {

        [Command("ChangeColor")]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [Description("Zmień kolor roli")]
        async Task ChangeColor(CommandContext ctx, [Description("Nazwa Roli (bez @)")] string roleName, [Description("Nowy kolor, który zrozumie Discord, np. #ffffff")] DiscordColor color)
        {
            await ctx.TriggerTypingAsync();

            var role = ctx.Guild.Roles.Where(r => r.Value.Name == roleName).FirstOrDefault();
            if(role.Value == null)
            {
                await ctx.RespondAsync("Nie znaleziono podanej roli");
                return;
            }

            await role.Value.ModifyAsync(color: color);
            await ctx.RespondAsync("My job is done");
        }

        [Command("RandomColor")]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [Description("Zmień kolor roli na losowy")]
        async Task ChangeColorToRandom(CommandContext ctx, [Description("Nazwa Roli (bez @)")] string roleName)
        {
            await ctx.TriggerTypingAsync();

            var role = ctx.Guild.Roles.Where(r => r.Value.Name == roleName).FirstOrDefault();
            if (role.Value == null)
            {
                await ctx.RespondAsync("Nie znaleziono podanej roli");
                return;
            }

            await role.Value.ModifyAsync(color: ColorHelper.RandomColor());
            await ctx.RespondAsync("My job is done");
        }
    }
}
