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
    public class LewdCommand : NekosLifeImage
    {
        //Z uwagi na możliwy ostry (zboczony) content na razie zakomentowane XD XD
        //[Command("lewd")]
        //[Description("Wyświetla obrazki lewd.")]
        public async Task Lewd(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.lewd, member);
        }
    }
}
