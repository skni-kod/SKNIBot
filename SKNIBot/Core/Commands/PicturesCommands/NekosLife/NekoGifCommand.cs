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
    class NekoGifCommand : NekosLifeImage
    {
        [Command("nekogif")]
        [Description("Wyświetla neko gify.")]
        public async Task Lewd(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            if (ctx.Channel.IsNSFW)
                await SendImage(ctx, NekosLifePicturesEndpoints.NsfwNekoGif, member);
            else
                await ctx.RespondAsync("Chciałbyś ;)");
        }
    }
}
