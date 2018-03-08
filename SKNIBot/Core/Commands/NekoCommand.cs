using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;
using SKNIBot.Core.Settings;
using Newtonsoft.Json;
using System.IO;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class NekoCommand
    {
        [Command("neko")]
        [Description("Display some cute cat.")]
        [Aliases("kot", "cat")]
        public async Task Neko(CommandContext ctx)
        { 
            var client = new WebClient();
            var cat = client.DownloadString("http://random.cat/meow");
            NekoContainer nekoContainer = JsonConvert.DeserializeObject<NekoContainer>(cat);            
            var catPicture = client.DownloadData(nekoContainer.File);
            Stream stream = new MemoryStream(catPicture);
            await ctx.TriggerTypingAsync();
            await ctx.RespondWithFileAsync(stream, "neko.jpg");
        }
    }
}
