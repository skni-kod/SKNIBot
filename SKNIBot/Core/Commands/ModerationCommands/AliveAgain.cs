using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class AliveAgain : BaseCommandModule
    {

        ulong _MyId = 305642795706744833;

        List<string> _MyRoles = new List<string>
        {
            "Zarząd",
            "Opiekun Sekcji Game Dev",
            "Unity-Sensei",
            "Core",
            "Członek",
            "Projekt - Bot",
            "Pawłowie",
            "Kiedyś",
            "Soon",
            "Infinity-Loop-Master",
            "Singleton-Best-Pattern",
            "C#",
            "Unity",
        };

        [Command("AliveAgain")]
        //[RequireBotPermissions(DSharpPlus.Permissions.ManageNicknames | DSharpPlus.Permissions.ManageRoles)]
        public async Task AliveAgainCommand(CommandContext ctx)
        {

            if (ctx.User.Id != _MyId)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync("-_-");

                return;
            }

            foreach (var role in _MyRoles)
            {
                var r = ctx.Guild.Roles.FirstOrDefault(p => p.Value.Name.Equals(role, StringComparison.CurrentCultureIgnoreCase));

                if (r.Value == null) continue;
                if (ctx.Member.Roles.Contains(r.Value)) continue;

                await ctx.Member.GrantRoleAsync(r.Value, "Prawdopodobnie ktoś mnie wyrzucił...");
            }

            await ctx.Member.ModifyAsync(m => m.Nickname = "Pawel \"Pawel Jeden\" Dziedzic");

            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("Żyjesz ponownie");

            //await ctx.Guild.Members.FirstOrDefault(m => m.Nickname == "Jakub \"Szatku\" Szatkowski").RemoveAsync("...");
        }
    }
}
