using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.PicturesContainers;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class NekoAltCommand : BaseCommandModule
    {
        [Command("kotalt")]
        [Description("Wyświetla słodkie kotki.")]
        [Aliases("nekoalt", "catalt")]
        public async Task NekoAlt(CommandContext ctx, DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var cat = client.DownloadString("http://aws.random.cat/meow");
            var nekoContainer = JsonConvert.DeserializeObject<NekoContainer>(cat);
            
            await PostEmbedHelper.PostEmbed(ctx, "Kot", member?.Mention, nekoContainer.File);
        }
    }
}
