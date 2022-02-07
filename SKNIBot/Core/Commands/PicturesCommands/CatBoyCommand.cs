using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.SimpleResponseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    class CatBoyCommand : BaseCommandModule
    {
        private SimpleResponseService _simpleResponseService;

        public CatBoyCommand(SimpleResponseService simpleResponseService)
        {
            _simpleResponseService = simpleResponseService;
        }

        [Command("catboy")]
        [Description("Wyświetla słodkie catboy.")]
        public async Task Neko(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            SimpleResponseResponse<SimpleResponseElement> answer = _simpleResponseService.GetAnswer("CatBoy");

            if (answer.Result == SimpleResponseResult.CommandHasNoResponses)
            {
                await PostEmbedHelper.PostEmbed(ctx, "CatBoy", "Brak odpowiedzi w bazie. Najpierw coś dodaj");
                return;
            }

            switch (answer.Responses.Type)
            {
                case Database.Models.StaticDB.SimpleResponseType.Text:
                    if (member == null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "CatBoy", answer.Responses.Content);
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "CatBoy", answer.Responses.Content + " " + member.Mention);
                    }
                    break;
                case Database.Models.StaticDB.SimpleResponseType.ImageLink:
                    if (member == null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "CatBoy", null, answer.Responses.Content);
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "CatBoy", member.Mention, answer.Responses.Content);
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
