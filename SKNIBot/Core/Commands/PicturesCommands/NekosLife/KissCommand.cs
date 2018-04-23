﻿using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Const.PicturesConst.NekosLife;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    [CommandsGroup("Obrazki")]
    public class KissCommand : NekosLifeImage
    {
        [Command("kiss")]
        [Description("Wyświetla obrazki kiss.")]
        public async Task Kiss(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            if (ctx.Channel.IsNSFW)
                await SendImage(ctx, NekosLifePicturesEndpoints.Kiss, member);
            else
                await ctx.RespondAsync("Nie tutaj ;)");
        }
    }
}
