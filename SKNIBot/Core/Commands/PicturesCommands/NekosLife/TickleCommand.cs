using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Const.PicturesConst.NekosLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    [CommandsGroup("Obrazki")]
    class TickleCommand : NekosLifeImage
    {
        [Command("tickle")]
        [Description("Wyświetla obrazki tickle.")]
        public async Task Hug(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Tickle, "Tickle", member);
        }
    }
}
