using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup("Tekst")]
    public class WikipediaCommand
    {
        private const string _randomSiteURL = "https://pl.wikipedia.org/wiki/Specjalna:Losowa_strona";

        [Command("wiki")]
        [Description("Wylosuj artykuł z Wikipedii")]
        [Aliases("wikipedia")]
        public async Task Wikipedia(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var request = (HttpWebRequest)WebRequest.Create(_randomSiteURL);
            request.AllowAutoRedirect = false;

            using (var response = request.GetResponse())
            {
                await ctx.RespondAsync(response.Headers["Location"]);
            }
        }
    }
}
