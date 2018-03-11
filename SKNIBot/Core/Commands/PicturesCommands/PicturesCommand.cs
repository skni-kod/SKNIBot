using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.PicturesContainers;

namespace SKNIBot.Core.Commands.PicturesCommands.PicturesCommand
{
    [CommandsGroup("Obrazki")]
    public class PicturesCommand
    {
        private List<PictureData> _images;
        private const string _imagesFile = "images.json";

        public PicturesCommand()
        {
            using (var file = new StreamReader(_imagesFile))
            {
                _images = JsonConvert.DeserializeObject<List<PictureData>>(file.ReadToEnd());
            }
        }

        [Command("picture")]
        [Description("Wyświetl obrazek!")]
        [Aliases("pic")]
        public async Task Picture(CommandContext ctx, [Description("Wpisz !pic help aby uzyskać listę dostępnych opcji.")] string pictureName = null, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            if (pictureName == "list")
            {
                await ctx.RespondAsync($"Dostępne obrazki:\r\n{GetAvailableParameters()}");
                return;
            }

            var videoData = _images.FirstOrDefault(vid => vid.Names.Exists(p => p == pictureName));
            if (videoData == null)
            {
                await ctx.RespondAsync("Nieznany parametr, wpisz !pic list aby uzyskać listę dostępnych.");
                return;
            }

            var response = videoData.Link;
            if (member != null)
            {
                response += $" {member.Mention}";
            }

            await ctx.RespondAsync(response);
        }

        private string GetAvailableParameters()
        {
            return string.Join(", ", _images.Select(p => p.Names[0]).ToArray());
        }
    }
}
