using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class EmbedCommand : BaseCommandModule
    {
        private const string EmbedElementPrefix = "%/";
        private List<string> TitleArgAliases = new List<string> { "tytuł", "tytul", "title" };
        private List<string> ContentArgAliases = new List<string> { "zawartość", "zawartosc", "content" };
        
        private string _EmbedHelpText = @"Przykład zastosowania komendy:
!embed
%/title To jest nasza wiadomość
%/content
A tutaj możemy wstawić jej treść

Składnia przekazywania argumentów to: %/<nazwa> <wartość>.
Aktualnie wspierane argumenty `title`,`content`. Nie podanie żadnego z argumentów, spowoduje umieszczenie wiadomości w treści embedu"
;
        
        [Command("embed")]
        [Description("Każ mi coś powiedzieć!")]
        [Aliases("mowembed", "sayembed")]
        public async Task SayEmbed(CommandContext ctx, [Description("Co chcesz powiedzieć?")] [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var inputMessage = ctx.RawArgumentString;
            if (inputMessage == "help")
            {
                await WriteHelp(ctx);
                return;
            }
            
            var inputSplited = inputMessage.Split(EmbedElementPrefix).ToList();
            inputSplited.RemoveAll(s => s.Length == 0);

            string title = "";
            string content = "";

            foreach (var argument in inputSplited)
            {
                var argName = argument.Split(new[] { ' ', '\n' }).First().ToLower();
                var argValue = argument.Substring(argName.Length).Trim();

                if (TitleArgAliases.Contains(argName))
                {
                    title = argValue;
                }
                
                else if (ContentArgAliases.Contains(argName))
                {
                    content = argValue;
                }
                else
                {
                    content = message;
                }
            }
            
            await ctx.Channel.DeleteMessageAsync(ctx.Message);
            
            await PostEmbedHelper.PostEmbed(ctx, title, content);
        }
        
        private async Task WriteHelp(CommandContext ctx)
        {
            await PostEmbedHelper.PostEmbed(ctx, "Pomoc", _EmbedHelpText,color: "#0000FF");
        }
    }
}
