using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Const.PicturesConst.NekosLife;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    [CommandsGroup("Obrazki")]
    public class LewdCommand : NekosLifeImage
    {
        //Z uwagi na możliwy ostry (zboczony) content na razie zakomentowane XD XD
        [Command("lewd")]
        [Description("Wyświetla obrazki lewd.")]        
        public async Task Lewd(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            if (ctx.Channel.IsNSFW)
                await SendImage(ctx, NekosLifePicturesEndpoints.Lewd, member);
            else
                await ctx.RespondAsync("Chciałbyś ;)");
        }
    }
}
