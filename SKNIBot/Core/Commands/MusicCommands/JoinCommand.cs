using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.MusicCommands
{
    [CommandsGroup("Music")]
    class JoinCommand
    {
        [Command("dołącz")]
        [Description("Bot dołącza do kanału głosowego, w którym obecnie jesteś.")]
        [Aliases("dolacz", "join")]
        public async Task Join(CommandContext ctx)
        {
            await ctx.RespondAsync("Kappa1.");
            var vnext = ctx.Client.GetVoiceNextClient();
            await ctx.RespondAsync("Kappa2.");
            var currentVoiceChannel = vnext.GetConnection(ctx.Guild);
            if (currentVoiceChannel != null)
            {
                await ctx.RespondAsync("Jestem już podłączony do innego kanału głosowego.");
                return;
            }
            await ctx.RespondAsync("Kappa3.");
            var currentUserChannel = ctx.Member?.VoiceState?.Channel;
            if (currentUserChannel == null)
            {
                await ctx.RespondAsync("Musisz dołączyć do kanału głosowego przed dodaniem bota.");
                return;
            }
            await ctx.RespondAsync("Kappa4.");
            currentVoiceChannel = await vnext.ConnectAsync(currentUserChannel);
            await ctx.RespondAsync("Dołączyłem do twojego kanału głosowego.");
        }
    }
}
