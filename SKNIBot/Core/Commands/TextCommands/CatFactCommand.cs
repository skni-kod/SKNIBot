using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKNIBot.Core.Containers.TextContainers;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using System.Net;
using Newtonsoft.Json;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    class CatFactCommand
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
