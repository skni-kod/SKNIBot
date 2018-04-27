using System.Data.Entity;
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
        public async Task YouTube(CommandContext ctx, [Description("Wpisz !yt list aby uzyskać listę dostępnych opcji.")] string videoName = null, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                if (videoName == "list")
                {
                    await ctx.RespondAsync($"Dostępne filmy:\r\n\r\n{GetAvailableParameters()}");
                    return;
                }

                // String.Equals doesn't work in SQLite provider (comparison is case sensitive) so it must be replaced with DbFunctions.Like().
                var videoLink = databaseContext.Media
                    .Where(vid => vid.Command.Name == "YouTube" && vid.Names.Any(p => DbFunctions.Like(p.Name, videoName)))
                    .Select(p => p.Link)
                    .FirstOrDefault();

                if (videoLink == null)
                {
                    await ctx.RespondAsync("Nieznany parametr, wpisz !yt list aby uzyskać listę dostępnych.");
                    return;
                }

                var response = videoLink;
                if (member != null)
                {
                    response += $" {member.Mention}";
                }

                await ctx.RespondAsync(response);
            }
        }

        private string GetAvailableParameters()
        {
            using (var databaseContext = new StaticDBContext())
            {
                var stringBuilder = new StringBuilder();
                var categories = databaseContext.Media
                    .Where(p => p.Command.Name == "YouTube")
                    .Select(p =>
                        new
                        {
                            CategoryName = p.Category.Name,
                            MediaName = p.Names.FirstOrDefault().Name
                        })
                    .AsEnumerable()
                    .GroupBy(p => p.CategoryName)
                    .OrderBy(p => p.Key)
                    .ToList();

                foreach (var category in categories)
                {
                    var sortedCategory = category.OrderBy(p => p.MediaName);
                    var items = sortedCategory.Select(p => p.MediaName);

                    stringBuilder.Append($"**{category.Key}**:\r\n");
                    stringBuilder.Append(string.Join(", ", items));
                    stringBuilder.Append("\r\n\r\n");
                }

                return stringBuilder.ToString();
            }
        }
    }
}
