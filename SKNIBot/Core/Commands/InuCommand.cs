using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;
using Newtonsoft.Json;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class InuCommand
    {
        [Command("inu")]
        [Description("Display some cute dogs.")]
        [Aliases("pies", "dog")]
        public async Task Inu(CommandContext ctx)
        {
            var client = new WebClient();
            var dog = client.DownloadString("https://random.dog/woof.json");
            DogContainer dogContainer = JsonConvert.DeserializeObject<DogContainer>(dog);
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"" + dogContainer.Url);
        }
    }
}
