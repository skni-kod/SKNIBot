using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.SimpleResponseService;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    class JokeCommand : BaseCommandModule
    {
        private SimpleResponseService _simpleResponseService;

        public JokeCommand(SimpleResponseService simpleResponseService)
        {
            _simpleResponseService = simpleResponseService;
        }

        [Command("żart")]
        [Description("Żarty i suchary w postaci tekstu i obrazków.")]
        [Aliases("suchar", "joke", "itsJoke")]
        public async Task Joke(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            SimpleResponseResponse<SimpleResponseElement> answer = _simpleResponseService.GetAnswer("Joke");

            if(answer.Result == SimpleResponseResult.CommandHasNoResponses)
            {
                await PostEmbedHelper.PostEmbed(ctx, "Żart", "Brak odpowiedzi w bazie. Najpierw coś dodaj");
                return;
            }

            switch (answer.Responses.Type)
            {
                case Database.Models.StaticDB.SimpleResponseType.Text:
                    if(member == null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Żart", answer.Responses.Content);
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Żart", answer.Responses.Content + " " + member.Mention);
                    }
                    break;
                case Database.Models.StaticDB.SimpleResponseType.ImageLink:
                    if (member == null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Żart", null, answer.Responses.Content);
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Żart", member.Mention, answer.Responses.Content);
                    }
                    break;
                // Currently there's no way to append video link to embed
                case Database.Models.StaticDB.SimpleResponseType.YoutubeLink:
                case Database.Models.StaticDB.SimpleResponseType.Other:
                    if (member == null)
                    {
                        await ctx.RespondAsync(answer.Responses.Content);
                    }
                    else
                    {
                        await ctx.RespondAsync(answer.Responses.Content + " " + member.Mention);
                    }
                    break;
            }
        }
    }
}
