using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Net;
using System.IO;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup("Różne")]
    public class AvatarCommand
    {
        [Command("awatar")]
        [Description("Pokazuje awatar użytkownika.")]
        public async Task Avatar(CommandContext ctx, [Description("Użytkownik, którego awatar chcesz.")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            string url;
            if (member == null)
            {
                url = ctx.User.AvatarUrl;
            }
            else
            {             
                url = member.AvatarUrl;
            }
            var client = new WebClient();
            var catPicture = client.DownloadData(url);
            var stream = new MemoryStream(catPicture);

            await ctx.RespondWithFileAsync(stream, "awatar.jpg");
        }

    }
}
