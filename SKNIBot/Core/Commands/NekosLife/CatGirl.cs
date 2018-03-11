using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.NekosLife;
using SKNIBot.Core.Const.NekosLife;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.NekosLife
{
    [CommandsGroup("Obrazki")]
    public class NekoCommand
    {
        [Command("catgirl")]
        [Description("Wyświetla słodkie catgirl.")]
        public async Task CatGirl(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var catGirl = client.DownloadString(NekosLifeEndpoints.catGirl);
            var catGirlContainer = JsonConvert.DeserializeObject<NekosFileImage>(catGirl);
            var catGirlPicture = client.DownloadData(catGirlContainer.Url);
            var stream = new MemoryStream(catGirlPicture);

            if (member != null)
            {
                await ctx.RespondWithFileAsync(stream, "catgirl.jpg", member.Mention);
            }
            else
            {
                await ctx.RespondWithFileAsync(stream, "catgirl.jpg");
            }

            
        }
    }
}
