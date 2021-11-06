using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class SayCommand : BaseCommandModule
    {
        [Command("mów")]
        [Description("Każ mi coś powiedzieć!")]
        [Aliases("mow", "powiedz", "say")]
        public async Task Say(CommandContext ctx, [Description("Co chcesz powiedzieć?")] [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();
            await PostEmbedHelper.PostEmbed(ctx, "Bocik przemówił", message);
        }

        [Command("mówd")]
        [Description("Każ mi coś powiedzieć!")]
        [Aliases("mowd", "powiedzd", "sayd")]
        public async Task SayD(CommandContext ctx, [Description("Co chcesz powiedzieć?")] [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();
            await ctx.Channel.DeleteMessageAsync(ctx.Message);
            await PostEmbedHelper.PostEmbed(ctx, "Bocik przemówił", message);
        }
    }
}
