using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.SimpleResponseService;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    class EightBallCommand : BaseCommandModule {
        private Random _rand;
        private SimpleResponseService _simpleResponseService;

        public EightBallCommand(SimpleResponseService simpleResponseService) {
            _rand = new Random();
            _simpleResponseService = simpleResponseService;
        }

        [Command("8Ball")]
        [Description("Magic 8 ball odpowie na wszystkie twoje pytania!")]
        public async Task EightBall(CommandContext ctx, [Description("Twoje pytanie")] [RemainingText] string question)
        {
            await ctx.TriggerTypingAsync();

            SimpleResponseResponse<SimpleResponseElement> answer = _simpleResponseService.GetAnswer("8Ball");

            if (answer.Result == SimpleResponseResult.CommandHasNoResponses)
            {
                await PostEmbedHelper.PostEmbed(ctx, "8Ball", "Brak odpowiedzi w bazie. Najpierw coś dodaj");
                return;
            }

            switch (answer.Responses.Type)
            {
                case Database.Models.StaticDB.SimpleResponseType.Text:
                    await PostEmbedHelper.PostEmbed(ctx, "8Ball", "8Ball mówi: " + answer.Responses.Content);
                    break;
                // Currently there's no way to append video link to embed
                case Database.Models.StaticDB.SimpleResponseType.ImageLink:
                case Database.Models.StaticDB.SimpleResponseType.YoutubeLink:
                case Database.Models.StaticDB.SimpleResponseType.Other:
                    break;
            }
        }

        [Command("8BallList")]
        public async Task EightBallList(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            SimpleResponseResponse<List<SimpleResponseElement>> answers = _simpleResponseService.GetAnswers("8Ball");

            if (answers.Result == SimpleResponseResult.CommandHasNoResponses)
            {
                await PostEmbedHelper.PostEmbed(ctx, "8Ball", "Brak odpowiedzi w bazie. Najpierw coś dodaj");
                return;
            }

            StringBuilder builder = new StringBuilder();

            foreach(var answer in answers.Responses)
            {
                switch (answer.Type)
                {
                    case Database.Models.StaticDB.SimpleResponseType.Text:
                        builder.AppendLine(answer.Content);
                        break;
                    // Currently there's no way to append video link to embed
                    case Database.Models.StaticDB.SimpleResponseType.ImageLink:
                    case Database.Models.StaticDB.SimpleResponseType.YoutubeLink:
                    case Database.Models.StaticDB.SimpleResponseType.Other:
                        break;
                }
            }
            await PostLongMessageHelper.PostLongMessage(ctx, answers.Responses.Select(p => p.Content).ToList(), "\n", "8Ball");
        }
    }
}
