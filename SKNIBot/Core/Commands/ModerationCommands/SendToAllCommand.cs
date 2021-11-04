using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class SendToAllCommand : BaseCommandModule
    {
        [Command("SendToAll")]
        [RequirePermissions(Permissions.ManageMessages)]
        [Description("Wyślij wiadomość do wszystkich członków serwera z daną rolą. Wygląd wiadmości możesz przetestować poprzez komendę !mów lub !mówd.")]
        public async Task SendToAll(CommandContext ctx, [Description("Rola do której mają zostać wysłane wiadomości.")] DiscordRole role, [RemainingText, Description("Treść wiadomości.")] string message)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("To mała wiadomość dla ciebie, ale stos wiadomości dla mnie");
            await ctx.RespondAsync("Apokalipsa zaczyna się");
            await ctx.RespondAsync("Wyszukiwanie ofiar");

            var members = ctx.Guild.Members.Where(m => !m.Value.IsBot && m.Value.Roles.Contains(role));

            await ctx.RespondAsync($"Znaleziono {members.Count()} ofiar");

            await ctx.RespondAsync("Wysyłanie orędzia");

            var sentMessagesCount = 0;

            DiscordMessage progressBarMessage = await ctx.RespondAsync("`----------" + " 0 %`");
            foreach (var member in members)
            {
                try
                {
                    var dm = await member.Value.CreateDmChannelAsync();
                    DiscordEmbed embed = new DiscordEmbedBuilder
                    {
                        Title = $"Orędzie serwera {ctx.Guild.Name}",
                        Description = message,
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            Text = $"Depesza przekazana na polecenie {ctx.Member.DisplayName} ({ctx.Member.Username}#{ctx.Member.Discriminator})",
                            IconUrl = ctx.Member.AvatarUrl
                        },
                        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = ctx.Guild.IconUrl }
                    };

                    await dm.SendMessageAsync(embed);
                    sentMessagesCount++;

                    try
                    {
                        await progressBarMessage.ModifyAsync(CreateProgressBar(members.Count(), sentMessagesCount));
                    }
                    catch { }

                    Bot.DiscordClient.Logger.Log(LogLevel.Information, "SKNI Bot",
                        $"Wysłano wiadomość do {member.Value.DisplayName} ({member.Value.Username}#{member.Value.Discriminator})", DateTime.Now);        
                }
                catch (Exception ex)
                {
                    await ctx.RespondAsync($"Nie udało się wysłać orędzia do {member.Value.DisplayName} ({member.Value.Username}#{member.Value.Discriminator}): {ex}");
                }
            }

            try
            {
                await progressBarMessage.ModifyAsync(CreateProgressBar(members.Count(), members.Count()));
            }
            catch { }

            await ctx.RespondAsync($"Zrobiłem swoje, teraz ty mierz się z gniewem ludu. Wysłano {sentMessagesCount} wiadomości.");
        }

        private string CreateProgressBar(int numberOfRecivers, int value)
        {
            int length = 10;
            float part = (float)value / (float)numberOfRecivers;
            StringBuilder builder = new StringBuilder();
            builder.Append('`');
            for (int i = 0; i < part * 10; ++i)
            {
                builder.Append('=');
            }
            for(int i = builder.Length; builder.Length <= length; ++i)
            {
                builder.Append('-');
            }
            builder.Append(" " + part * 100 + " %");
            builder.Append('`');
            return builder.ToString();
        }
    }
}
