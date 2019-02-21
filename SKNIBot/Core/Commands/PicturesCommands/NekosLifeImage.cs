using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Const.PicturesConst.NekosLife;
using SKNIBot.Core.Containers.PicturesContainers.NekosLife;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class NekosLifeImage : BaseCommandModule
    {
        private const string footerText = "Powered by nekos.life";

        [Command("catgirl")]
        [Description("Wyświetla słodkie catgirl.")]
        public async Task CatGirl(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Neko, "Cat girl", member);
        }

        [Command("cuddle")]
        [Description("Wyświetla obrazki cuddle.")]
        public async Task Cuddle(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Cuddle, "Cuddle", member);
        }

        [Command("hug")]
        [Description("Wyświetla obrazki hug.")]
        public async Task Hug(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Hug, "Hug", member);
        }

        [Command("lizard")]
        [Description("Wyświetla obrazki pat.")]
        public async Task Lizard(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Lizard, "Lizard", member);
        }

        [Command("pat")]
        [Description("Wyświetla obrazki pat.")]
        public async Task Pat(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Pat, "Pat", member);
        }

        [Command("tickle")]
        [Description("Wyświetla obrazki tickle.")]
        public async Task Tickle(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();
            await SendImage(ctx, NekosLifePicturesEndpoints.Tickle, "Tickle", member);
        }

        public async Task SendImage(CommandContext ctx, string endpoint, string title, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var client = new WebClient();
            var url = client.DownloadString(endpoint);
            var pictureContainer = JsonConvert.DeserializeObject<NekosFileImage>(url);

            await PostEmbedHelper.PostEmbed(ctx, title, member?.Mention, pictureContainer.Url, footerText);
        }

        public string GetExtension(string url)
        {
            var array = url.Split('.');
            return array[array.Length - 1];
        }
    }
}
