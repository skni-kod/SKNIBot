using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;
using Newtonsoft.Json;
using SKNIBot.Core.Settings;
using System.IO;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class InuCommand
    {
        [Command("pies")]
        [Description("Display some cute dogs.")]
        [Aliases("inu", "dog")]
        public async Task Inu(CommandContext ctx)
        {
            var client = new WebClient();
            var dog = client.DownloadString("https://random.dog/woof.json");
            DogContainer dogContainer = JsonConvert.DeserializeObject<DogContainer>(dog);
            var dogPicture = client.DownloadData(dogContainer.Url);
            Stream stream = new MemoryStream(dogPicture);
            await ctx.TriggerTypingAsync();
            await ctx.RespondWithFileAsync(stream, "inu.jpg"); ;
        }
    }
}
