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
    public class KissCommand : NekosLifeImage
    {
        //Zablokowana na życzenie innego developera
        //[Command("kiss")]
        //[Description("Wyświetla obrazki kiss.")]
        public async Task Kiss(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifeEndpoints.kiss, member);
        }
    }
}
