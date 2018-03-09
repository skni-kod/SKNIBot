﻿using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;
using Newtonsoft.Json;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands
{
    [CommandsGroup]
    public class LennyCommand
    {
        [Command("lenny")]
        [Description("Display lenny.")]
        public async Task Lenny(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("( ͡° ͜ʖ ͡°)");
        }
    }
}