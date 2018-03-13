using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.PicturesContainers;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class XkcdCommand
    {
        private Random _random;

        private const string RandomXkcdUrl = "https://xkcd.com/{0}/info.0.json";
        private const int MaxIndex = 1500;

        public XkcdCommand()
        {
            _random = new Random();
        }

        [Command("xkcd")]
        [Description("Losowy obrazek z xkcd.")]
        public async Task Xkcd(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var randomIndex = _random.Next(0, MaxIndex);
            var link = string.Format(RandomXkcdUrl, randomIndex);

            var client = new WebClient();
            var url = client.DownloadString(link);
            var xkcdContainer = JsonConvert.DeserializeObject<XkcdContainer>(url);

            await ctx.RespondAsync(GetResponse(xkcdContainer));
        }

        private string GetResponse(XkcdContainer xkcdContainer)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"**{xkcdContainer.Title}**");
            stringBuilder.Append("\r\n\r\n");
            stringBuilder.Append(xkcdContainer.Img);

            return stringBuilder.ToString();
        }
    }
}
