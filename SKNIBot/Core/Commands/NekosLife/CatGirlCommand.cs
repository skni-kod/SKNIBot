using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const.NekosLife;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.NekosLife
{
    [CommandsGroup("Obrazki")]
    public class CatGirlCommand : NekosLifeImage
    {
        [Command("catgirl")]
        [Description("Wyświetla słodkie catgirl.")]
        public async Task CatGirl(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifeEndpoints.neko, member);
        }
    }
}
