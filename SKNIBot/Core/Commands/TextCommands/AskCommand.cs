using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Różne")]
    class AskCommand : BaseCommandModule
    {
        [Command("ask")]
        [Description("Zapytaj o coś bota.")]
        public async Task Avatar(CommandContext ctx, [Description("Pytanie")] string question = null)
        {
            await ctx.TriggerTypingAsync();

            if(question == null)
            {
                await ctx.RespondAsync("Składnia to `!ask {pytanie}`");
            }
            else if(!question.Contains("?"))
            {
                await ctx.RespondAsync("A może jednak zapytasz zamiast stawiać twierdzenia?");
            }
            else
            {
                var client = new WebClient();

                var response = client.DownloadString("https://yesno.wtf/api");
                var answer = JsonConvert.DeserializeObject<AskContainer>(response);
                var answerPicture = client.DownloadData(answer.image);
                var stream = new MemoryStream(answerPicture);

                await ctx.RespondWithFileAsync("answer.gif", stream);
            }
        }
    }
}
