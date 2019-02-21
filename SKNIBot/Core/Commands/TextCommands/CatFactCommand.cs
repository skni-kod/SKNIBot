using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class CatFactCommand : BaseCommandModule
    {
        [Command("kotciekawostka")]
        [Description("Ciekawostki o kotach.")]
        [Aliases("catfact", "nekofact")]
        public async Task CatFact(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var url = client.DownloadString("https://catfact.ninja/fact");
            var catContainer = JsonConvert.DeserializeObject<CatFactContainer>(url);

            await ctx.RespondAsync(catContainer.Fact);
        }
    }
}
