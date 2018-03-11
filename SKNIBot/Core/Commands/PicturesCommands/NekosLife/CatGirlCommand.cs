using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const.PicturesConst.NekosLife;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    [CommandsGroup("Obrazki")]
    public class CatGirlCommand : NekosLifeImage
    {
        [Command("catgirl")]
        [Description("Wyświetla słodkie catgirl.")]
        public async Task CatGirl(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.neko, member);
        }
    }
}
