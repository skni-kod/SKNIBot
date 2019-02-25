using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    public class RepoDestroyed : BaseCommandModule
    {
        [Command("repoDestroyed")]
        [Description("Użyj jak rozwalisz repo.")]
        public async Task Destroyed(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await PostEmbedHelper.PostEmbed(ctx, "Repo destroyed - Abaddon the ship", null, $"https://media1.tenor.com/images/c2ba5bb955513c24cd503acbfa845901/tenor.gif");
            await PostEmbedHelper.PostEmbed(ctx, "Repo destroyed - Abaddon the ship", null, $"https://media1.tenor.com/images/575a65c9337f6830c0aa0c155cd4213b/tenor.gif");
            await PostEmbedHelper.PostEmbed(ctx, "Repo destroyed - Abaddon the ship", null, $"https://media1.tenor.com/images/882a987a82bc2365d282b08d6686f13a/tenor.gif");
        }
    }
}
