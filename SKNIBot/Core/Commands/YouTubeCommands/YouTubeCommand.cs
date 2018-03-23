using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;

namespace SKNIBot.Core.Commands.YouTubeCommands
{
    [CommandsGroup("YouTube")]
    public class YouTubeCommand
    {
        [Command("youtube")]
        [Description("Display video!")]
        [Aliases("yt")]
        public async Task YouTube(CommandContext ctx, [Description("Wpisz !yt help aby uzyskać listę dostępnych opcji.")] string videoName = null, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DatabaseContext())
            {
                if (videoName == "list")
                {
                    await ctx.RespondAsync($"Dostępne filmy:\r\n\r\n{GetAvailableParameters()}");
                    return;
                }

                var videoData = databaseContext.Videos.FirstOrDefault(vid => vid.Names.Any(p => p.Name.Equals(videoName, StringComparison.InvariantCultureIgnoreCase)));
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
        }

        private string GetAvailableParameters()
        {
            using (var databaseContext = new DatabaseContext())
            {
                var stringBuilder = new StringBuilder();
                var categories = databaseContext.Videos.GroupBy(p => p.VideoCategory.Name).OrderBy(p => p.Key).ToList();

                foreach (var category in categories)
                {
                    var sortedCategory = category.OrderBy(p => p.Names[0].Name);
                    var items = sortedCategory.Select(p => p.Names[0].Name);

                    stringBuilder.Append($"**{category.Key}**:\r\n");
                    stringBuilder.Append(string.Join(", ", items));
                    stringBuilder.Append("\r\n\r\n");
                }

                return stringBuilder.ToString();
            }
        }
    }
}
