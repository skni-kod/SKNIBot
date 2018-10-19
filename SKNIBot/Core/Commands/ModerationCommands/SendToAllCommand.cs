using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class SendToAllCommand : BaseCommandModule
    {
        [Command("SendToAll")]
        [RequirePermissions(Permissions.ManageMessages)]
        [Description("Wyślij wiadomość do wszystkich członków serwera z daną rolą. Wygląd wiadmości możesz przetestować poprzez komendę !mów lub !mówd.")]
        public async Task SendToAll(CommandContext ctx, [Description("Rola do której mają zostać wysłane wiadomości.")] string role, [Description("Treść wiadomości.")] string message)
        {
            await ctx.RespondAsync("Rozpoczynam wysyłanie...");
            await ctx.TriggerTypingAsync();

            var sentMessagesCount = 0;
            foreach (var member in ctx.Guild.Members.Where(m => !m.IsBot && m.Roles.Select(r => r.Mention).Contains(role)))
            {
                try
                {
                    var dm = await member.CreateDmChannelAsync();
                    await dm.SendMessageAsync(message);

                    Bot.DiscordClient.DebugLogger.LogMessage(LogLevel.Info, "SKNI Bot", "Wysłano wiadomość do " + member.DisplayName, DateTime.Now);
                    sentMessagesCount++;
                }
                catch (Exception ex)
                {
                    await ctx.RespondAsync($"Nie udało się wysłać wiadomości do {member.DisplayName}: {ex}");
                }
            }

            await ctx.RespondAsync($"Koniec! Wysłano {sentMessagesCount} wiadomości.");
        }
    }
}
