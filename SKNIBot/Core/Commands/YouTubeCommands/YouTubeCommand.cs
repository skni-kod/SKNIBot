using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.YouTube;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup("YouTube")]
    public class YouTubeCommand
    {
        private List<VideoData> _videos;
        private const string _videosFile = "youtube.json";

        public YouTubeCommand()
        {
            using (var file = new StreamReader(_videosFile))
            {
                _videos = JsonConvert.DeserializeObject<List<VideoData>>(file.ReadToEnd());
            }
        }

        [Command("youtube")]
        [Description("Display video!")]
        [Aliases("yt")]
        public async Task YouTube(CommandContext ctx, [Description("Wpisz !yt help aby uzyskać listę dostępnych opcji.")] string videoName = null, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            if (videoName == "list")
            {
                await ctx.RespondAsync($"Dostępne filmy:\r\n{GetAvailableParameters()}");
                return;
            }

            var videoData = _videos.FirstOrDefault(vid => vid.Names.Exists(p => p == videoName));
            if (videoData == null)
            {
                await ctx.RespondAsync("Nieznany parametr, wpisz !yt list aby uzyskać listę dostępnych.");
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
            return string.Join(", ", _videos.OrderBy(p => p.Names[0]).Select(p => p.Names[0]).ToArray());
        }
    }
}
