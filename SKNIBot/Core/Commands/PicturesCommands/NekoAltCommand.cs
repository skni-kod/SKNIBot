using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.PicturesContainers;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class NekoAltCommand : BaseCommandModule
    {
        [Command("kotalt")]
        [Description("Wyświetla słodkie kotki.")]
        [Aliases("nekoalt", "catalt")]
        public async Task NekoAlt(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var cat = client.DownloadString("http://aws.random.cat/meow");
            var nekoContainer = JsonConvert.DeserializeObject<NekoContainer>(cat);
            var catPicture = client.DownloadData(nekoContainer.File);
            var stream = new MemoryStream(catPicture);

            await ctx.RespondWithFileAsync("neko.jpg", stream);
        }
    }
}
