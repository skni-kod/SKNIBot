using System;
using System.Collections.Generic;
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
        private const string ArgNamePrefix = "%/";

        private List<string> CodeArgAliases = new List<string> { "kod", "code" };
        private List<string> LangArgAliases = new List<string> { "język", "language", "lang" };
        private List<string> InputArgAliases = new List<string> { "input", "wejście" };
        private List<string> VersionArgAliases = new List<string> { "wersja", "version" };

        private string _helpText = @"Przykład zastosowania komendy:
!kompiluj
%/lang rust
%/code
```
fn main() {
    println!(""Hello, World!"");
}```

Składnia przekazywania argumentów to: %/<nazwa> <wartość>.
Aktualnie wspierane argumenyty `lang`,`code`,`input`,`version`, argumenty code i lang są wymagane."
;


        [Command("kompiluj")]
        [Description("Kompiluje dany kod źródłowy i wyświetla wynik. Lista języków pod adresem https://www.jdoodle.com/compiler-api/docs. Wywołaj komendę 'kompiluj help' aby uzyskać więcej informacji.")]
        [Aliases("compile", "uruchom", "run")]
        public async Task Compile(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var inputMessage = ctx.RawArgumentString;
            if (inputMessage == "help")
            {
                await WriteHelp(ctx);
                return;
            }

            // split message content by prefix and remove empty strings
            var inputSplited = inputMessage.Split(ArgNamePrefix).ToList();
            inputSplited.RemoveAll(s => s.Length == 0);

            string langName = "";
            string code = "";
            string input = "";
            string version = "0";

            // Parse message parts
            for (int i = 0; i < inputSplited.Count; i++)
            {
                var argName = inputSplited[i].Split(new[] { ' ', '\n' }).First().ToLower();
                var argValue = inputSplited[i].Substring(argName.Length).Trim();

                if (LangArgAliases.Contains(argName))
                {
                    langName = argValue;
                }
                else if (CodeArgAliases.Contains(argName))
                {
                    // trim possible ``` in code block
                    code = argValue.Trim('`');
                }
                else if (InputArgAliases.Contains(argName))
                {
                    input = argValue;
                }
                else if (VersionArgAliases.Contains(argName))
                {
                    version = argValue;

                    if (version.Any(c => c < '0' || c > '9'))
                    {
                        await WriteError(ctx, "Pole wersji musi być liczbą");
                        return;
                    }
                }
            }

            if (langName.Length == 0)
            {
                await WriteError(ctx, "Błąd w nazwie języka lub nie została określona.");
                return;
            }

            if (code.Length == 0)
            {
                await WriteError(ctx, "Błąd w polu kodu lub nie został określony.");
                return;
            }

            var result = await GetCompilationResult(ctx, langName, input, code, version);
            if (result != null)
            {
                var embed = GetEmbedResult(result);
                await ctx.RespondAsync(embed: embed);
            }
        }

        private async Task WriteHelp(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor(0f, 0f, 1f));

            embed.AddField("Pomoc", _helpText);
            await ctx.RespondAsync(embed: embed);
        }

        private async Task WriteError(CommandContext ctx, string errorText)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor(1f, 0f, 0f))
                .AddField("Bład", errorText)
                .AddField("Pomoc", "Napisz `!kompiluj help` aby uzyskać więcej informacji");

            await ctx.RespondAsync(embed: embed);
        }

        private async Task<CompilationResultContainer> GetCompilationResult(CommandContext ctx, string language, string input, string code, string version)
        {
            var compileRequest = new CompileRequestContainer
            {
                clientId = SettingsLoader.Container.JDoodle_Client_ID,
                clientSecret = SettingsLoader.Container.JDoodle_Client_Secret,
                language = language,
                script = code,
                stdin = input,
                versionIndex = version
            };

            var compileRequestJSON = JsonConvert.SerializeObject(compileRequest);

            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            try
            {
                var response = webClient.UploadString(ApiEndpoint, compileRequestJSON);
                return JsonConvert.DeserializeObject<CompilationResultContainer>(response);
            }
            catch (WebException ex)
            {

                var response = ex.Response as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await WriteError(ctx, "Serwer zwrócił błąd 400 - Bad Request. Najparawdopodobniej został podany jezyk nie obsługiwany przez jDoodle. Tak 'js' i 'javacript' nie są obsługiwane, trzeba użyć 'nodejs'.");
                }
                else
                {
                    await WriteError(ctx, ex.Message);
                }
            }

            return null;
        }

        private DiscordEmbed GetEmbedResult(CompilationResultContainer compilationResult)
        {
            var output = compilationResult.output == "" ? "Brak" : compilationResult.output;

            var embedResult = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor(0f, 1f, 0f));
            embedResult.AddField("Rezultat", output);

            return embedResult;
        }
    }
}
