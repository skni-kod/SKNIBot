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
    class LeaveCommand : BaseCommandModule
    {
        [Command("wyjdź")]
        [Description("Bot dołącza do kanału głosowego, w którym obecnie jesteś.")]
        [Aliases("wyjdz", "leave")]
        public async Task Leave(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();

            var currentVoiceChannel = vnext.GetConnection(ctx.Guild);
            if (currentVoiceChannel == null)
            {
                await ctx.RespondAsync("Nie jestem w kanale głosowym");
                return;
            }

            currentVoiceChannel.Disconnect();
            await ctx.RespondAsync("Wyszedłem z kanału głosowego");
        }
    }
}
