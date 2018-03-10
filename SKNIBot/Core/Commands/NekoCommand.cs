using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup("Koty")]
    public class NekoCommand
    {
        [Command("kot")]
        [Description("Display some cute cat.")]
        [Aliases("neko", "cat")]
        public async Task Neko(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var cat = client.DownloadString("http://random.cat/meow");
            var nekoContainer = JsonConvert.DeserializeObject<NekoContainer>(cat);
            var catPicture = client.DownloadData(nekoContainer.File);
            var stream = new MemoryStream(catPicture);

            await ctx.RespondWithFileAsync(stream, "neko.jpg");
        }
    }
}
