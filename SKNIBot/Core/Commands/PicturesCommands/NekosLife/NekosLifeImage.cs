using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Const.NekosLife;
using SKNIBot.Core.Containers.NekosLife;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands.NekosLife
{
    public class NekosLifeImage
    {
        public async Task SendImage(CommandContext ctx, string endpoint, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var url = client.DownloadString(endpoint);
            var pictureContainer = JsonConvert.DeserializeObject<NekosFileImage>(url);
            /*var picture = client.DownloadData(pictureContainer.Url);
            var stream = new MemoryStream(picture);*/

            if (member != null)
            {
                //await ctx.RespondWithFileAsync(stream, "picture" + GetExtension(url), member.Mention);
                await ctx.RespondAsync(pictureContainer.Url + " " + member.Mention);
            }
            else
            {
                //await ctx.RespondWithFileAsync(stream, "picture" + GetExtension(url));
                await ctx.RespondAsync(pictureContainer.Url);
            }
        }

        public string GetExtension(string url)
        {
            string[] array = url.Split('.');
            return array[array.Length - 1];
        }
    }
}
