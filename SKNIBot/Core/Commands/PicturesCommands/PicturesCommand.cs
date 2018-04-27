using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class PicturesCommand
    {
        [Command("picture")]
        [Description("Wyświetl obrazek!")]
        [Aliases("pic")]
        public async Task Picture(CommandContext ctx, [Description("Wpisz !pic list aby uzyskać listę dostępnych opcji.")] string pictureName = null, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                if (pictureName == "list")
                {
                    await ctx.RespondAsync($"Dostępne obrazki:\r\n\r\n{GetAvailableParameters()}");
                    return;
                }

                // String.Equals doesn't work in SQLite provider (comparison is case sensitive) so it must be replaced with DbFunctions.Like().
                var pictureLink = databaseContext.Media
                    .Where(vid => vid.Command.Name == "Picture" && vid.Names.Any(p => DbFunctions.Like(p.Name, pictureName)))
                    .Select(p => p.Link)
                    .FirstOrDefault();

                if (pictureLink == null)
                {
                    await ctx.RespondAsync("Nieznany parametr, wpisz !pic list aby uzyskać listę dostępnych.");
                    return;
                }

                var response = pictureLink;
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
                    .Where(p => p.Command.Name == "Picture")
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
