using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.CompilationContainers;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Różne")]
    public class CompileCommand : BaseCommandModule
    {
        private const string ApiEndpoint = "https://api.jdoodle.com/v1/execute";
        private const char ArgNamePrefix = '%';

        [Command("kompiluj")]
        [Description("Kompiluje dany kod źródłowy i wyświetla wynik. Lista języków pod adresem https://www.jdoodle.com/compiler-api/docs")]
        [Aliases("compile", "uruchom", "run")]
        public async Task Compile(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var inputMessage = ctx.RawArgumentString;

            var inputSplited = inputMessage.Split(ArgNamePrefix).ToList();
            inputSplited.RemoveAll(s => s.Length == 0);


            for (int i = 0; i < inputSplited.Count; i++)
            {
                await ctx.RespondAsync(inputSplited[i]);
            }
        }

        private CompilationResultContainer GetCompilationResult(string language, string input, string code)
        {
            var compileRequest = new CompileRequestContainer
            {
                clientId = SettingsLoader.Container.JDoodle_Client_ID,
                clientSecret = SettingsLoader.Container.JDoodle_Client_Secret,
                language = language,
                script = code,
                stdin = input,
                versionIndex = "0"
            };

            var compileRequestJSON = JsonConvert.SerializeObject(compileRequest);

            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            var response = webClient.UploadString(ApiEndpoint, compileRequestJSON);

            return JsonConvert.DeserializeObject<CompilationResultContainer>(response);
        }

        private DiscordEmbed GetEmbedResult(CompilationResultContainer compilationResult)
        {
            var output = compilationResult.output == "" ? "Brak" : compilationResult.output;

            var embedResult = new DiscordEmbedBuilder();
            embedResult.AddField("Rezultat", output);

            return embedResult;
        }
    }
}
