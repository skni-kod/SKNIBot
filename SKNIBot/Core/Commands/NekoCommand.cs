using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;
using SKNIBot.Core.Settings;
using Newtonsoft.Json;

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
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"" + nekoContainer.File);
        }
    }
}
