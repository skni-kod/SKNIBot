using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.OtherContainers;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Różne")]
    public class LinkCommand : BaseCommandModule
    {
        [Command("link")]
        [Description("Skracacz linków. Dodawaj http, bo się wykolei.")]
        public async Task Link(CommandContext ctx, [Description("Link do skrócenia z http.")] string link)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var url = client.DownloadString("https://api.waa.ai/shorten?url=" + link + "&key=" + SettingsLoader.Container.Waaai_Key);
            var linkContainer = JsonConvert.DeserializeObject<LinkContainer>(url);

            await ctx.RespondAsync(linkContainer.Data.Url);
        }
    }
}
