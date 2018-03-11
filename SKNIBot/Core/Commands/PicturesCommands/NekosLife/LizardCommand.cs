using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const.NekosLife;
using DSharpPlus.Entities;

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
            await SendImage(ctx, NekosLifeEndpoints.lizard, member);
        }
    }
}
