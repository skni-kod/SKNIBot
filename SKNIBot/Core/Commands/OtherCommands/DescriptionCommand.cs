﻿using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    public class DescriptionCommand
    {
        [Command("opis")]
        [Description("Zmienia opis bota.")]
        [RequireRolesAttribute("Projekt - Bot")]
        public async Task Description(CommandContext ctx, [Description("Nowy opis.")] string description = null)
        {
            await ctx.Client.UpdateStatusAsync(new DiscordGame(description));
        }
    }
}
