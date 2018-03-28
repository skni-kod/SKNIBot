using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class NekoCommand
    {
        private const string _randomSiteURL = "http://thecatapi.com/api/images/get?format=src";

        [Command("kot")]
        [Description("Wyświetla słodkie kotki w postaci gifów.")]
        [Aliases("neko", "cat")]
        public async Task Neko(CommandContext ctx)
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
