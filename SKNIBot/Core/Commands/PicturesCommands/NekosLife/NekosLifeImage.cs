using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.PicturesContainers.NekosLife;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    public class NekosLifeImage : BaseCommandModule
    {
        public async Task SendImage(CommandContext ctx, string endpoint, string title, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var url = client.DownloadString(endpoint);
            var pictureContainer = JsonConvert.DeserializeObject<NekosFileImage>(url);

            await PostEmbedHelper.PostEmbed(ctx, title, member?.Mention, pictureContainer.Url);
        }

        public string GetExtension(string url)
        {
            var array = url.Split('.');
            return array[array.Length - 1];
        }
    }
}
