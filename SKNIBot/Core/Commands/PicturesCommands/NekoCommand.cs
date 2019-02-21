using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class NekoCommand : BaseCommandModule
    {
        private const string _randomSiteURL = "http://thecatapi.com/api/images/get?format=src";

        [Command("kot")]
        [Description("Wyświetla słodkie kotki w postaci gifów.")]
        [Aliases("neko", "cat")]
        public async Task Neko(CommandContext ctx, DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var request = (HttpWebRequest)WebRequest.Create(_randomSiteURL);
            request.AllowAutoRedirect = true;

            using (var response = request.GetResponse())
            {
                await PostEmbedHelper.PostEmbed(ctx, "Kot", member?.Mention, response.ResponseUri.ToString());
            }
        }
    }
}
