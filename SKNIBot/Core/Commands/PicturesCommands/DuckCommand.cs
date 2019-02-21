using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class DuckCommand : BaseCommandModule
    {
        [Command("duck")]
        [Description("Kaczka, po prostu")]
        [Aliases("kaczka")]
        public async Task Duck(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();

            var duckJson = client.DownloadString("https://random-d.uk/api/v1/random");
            var duckContainer = JsonConvert.DeserializeObject<DuckContainer>(duckJson);

            await PostEmbedHelper.PostEmbed(ctx, "Kaczka", imageLink: duckContainer.url, footerText: duckContainer.message);
        }

        class DuckContainer
        {
            public string url;
            public string message;
        }
    }
}
