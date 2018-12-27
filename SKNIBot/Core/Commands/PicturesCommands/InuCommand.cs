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
    public class InuCommand : BaseCommandModule
    {
        [Command("pies")]
        [Description("Wyświetla słodkie pieski.")]
        [Aliases("inu", "dog")]
        public async Task Inu(CommandContext ctx, DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            DogContainer dogContainer;

            var dog = client.DownloadString("https://random.dog/woof.json");
            dogContainer = JsonConvert.DeserializeObject<DogContainer>(dog);

            await PostEmbedHelper.PostEmbed(ctx, "Pies", member?.Mention, dogContainer.Url);
        }
    }
}
