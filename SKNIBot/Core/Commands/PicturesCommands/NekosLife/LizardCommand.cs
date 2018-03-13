using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Const.PicturesConst.NekosLife;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    [CommandsGroup("Obrazki")]
    public class LizardCommand : NekosLifeImage
    {
        [Command("lizard")]
        [Description("Wyświetla obrazki lizard.")]
        public async Task Lizard(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.lizard, member);
        }
    }
}
