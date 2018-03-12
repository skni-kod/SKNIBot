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
    public class NekoAltCommand
    {
        private const string _randomSiteURL = "http://thecatapi.com/api/images/get?format=src";

        [Command("kotalt")]
        [Description("Wyświetla słodkie kotki w postaci gifów.")]
        [Aliases("nekoalt", "catalt")]
        public async Task NekoAlt(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var request = (HttpWebRequest)WebRequest.Create(_randomSiteURL);
            request.AllowAutoRedirect = false;

            using (var response = request.GetResponse())
            {
                await ctx.RespondAsync(response.Headers["Location"]);
            }
        }
    }
}
