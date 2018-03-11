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
    public class CuddleCommand : NekosLifeImage
    {
        [Command("cuddle")]
        [Description("Wyświetla obrazki cuddle.")]
        public async Task Cuddle(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.cuddle, member);
        }
    }
}
