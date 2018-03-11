using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const.TextConst.NekosLife;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers.NekosLife;

namespace SKNIBot.Core.Commands.TextCommands.NekosLife
{
    [CommandsGroup("Tekst")]
    public class WhyCommand
    {
        [Command("why")]
        [Description("Dlaczego.")]
        public async Task Why(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var client = new WebClient();
            var url = client.DownloadString(NekosLifeTextsEndpoints.why);
            var whyContainer = JsonConvert.DeserializeObject<NekosWhy>(url);
            await ctx.RespondAsync(whyContainer.Why);
        }
    }
}