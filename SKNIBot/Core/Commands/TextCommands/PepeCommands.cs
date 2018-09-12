using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Różne")]
    class PepeCommands : BaseCommandModule
    {
        string _rain =
@" \` \ \` \` \ \`\ \` \` \ \`\ \` \ \` \` \`\ \` \`\ \` \`
\` \` \` \` \ \` \` \ \`   \` \ \` \ \`\` \` \ \ \`  \` \ \` 
\ \ \` \  \` \   \`  \` \` \\ \` \` \ \`\ \` \`   \` \` \`
\` \ \`\`   \`  \` {0} {1} \` \ \`\  \ \ \` \ \` \\ \`
\ \`  \ \`  \ \` \          \ \`\`   \`  \  \`\ \`
";

        string _rainWithText =
@" \` \ \` \` \ \`\ \` \` \ \`\ \` \ \` \` \`\ \` \`\ \` \` \` \` \ \ \` \ \\ 
\` \` \` \` \ \` \ \ \`   \` \ \` \ \`\` \` \ \ \`  \` \ \` \` \` \ \ \` \ \\ 
\ \ \` \  \` \   \`  \` \` \\ \` \` \ \`\ \` \`   \` \` \`\` \` \ \ \` \ \\ 
\` \ \`\`   \`  \` {0} {1} {2} \` \ \`\  \ \ \` \ \` \\ \`
\ \`  \ \`  \ \` \          \ \`\`   \`  \  \`\ \`\` \` \ \ \` \ \\ 
";
        [Description(";_;")]
        [Command("PepoSad")]
        public async Task PepeRain(CommandContext ctx, string text = null)
        {
            await ctx.TriggerTypingAsync();


            var umbrella = DiscordEmoji.FromName(ctx.Client, ":umbrella:");
            var feelsBadMan = DiscordEmoji.FromName(ctx.Client, ":FeelsBadMan:");

            var respond = "";
            if (text == null)
            {
                respond = string.Format(_rain, umbrella, feelsBadMan);
            }
            else
            {
                respond = string.Format(_rainWithText, umbrella, feelsBadMan, text);

            }

            await ctx.RespondAsync(respond);
        }
    }
}
