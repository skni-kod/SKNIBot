using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKNIBot.Core.Containers.TextContainers;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using System.Net;
using Newtonsoft.Json;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    class LinkCommand
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
