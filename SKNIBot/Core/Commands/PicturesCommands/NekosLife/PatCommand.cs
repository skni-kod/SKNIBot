using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Const.PicturesConst.NekosLife;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    [CommandsGroup("Obrazki")]
    public class PatCommand : NekosLifeImage
    {
        [Command("pat")]
        [Description("Wyświetla obrazki pat.")]
        public async Task Pat(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Pat, "Pat", member);
        }
    }
}
