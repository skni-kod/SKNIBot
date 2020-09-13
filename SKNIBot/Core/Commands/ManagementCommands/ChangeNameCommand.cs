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
        [Command("zmieńNazwę")]
        [Aliases("changeName", "zmienNazwe", "zmieńNazwe", "zmienNazwę")]
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

        [Command("zmienPseudonim")]
        [Aliases("changeNick", "zmienNick")]
        [Description("Zmień pseudonim bota.")]
        public async Task ChangeNick(CommandContext ctx, [Description("Nowy pseudonim.")] [RemainingText] string name)
        {
            if (DeveloperHelper.IsDeveloper(ctx.User.Id))
            {
                await ctx.Guild.CurrentMember.ModifyAsync(p => p.Nickname = name);
            }
            else
            {
                await ctx.RespondAsync("You aren't my father.");
            }  
        }

        [Command("usunPseudonim")]
        [Aliases("deleteNick", "usunNick")]
        [Description("Usuń pseudonim bota.")]
        public async Task RemoveNick(CommandContext ctx)
        {
            if (DeveloperHelper.IsDeveloper(ctx.User.Id))
            {
                await ctx.Guild.CurrentMember.ModifyAsync(p => p.Nickname = null);
            }
            else
            {
                await ctx.RespondAsync("You aren't my father.");
            }
        }
    }
}
