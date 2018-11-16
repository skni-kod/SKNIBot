using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Role")]
    public class RoleCommand : BaseCommandModule
    {
        [Command("DodajRole")]
        public async Task AddRole(CommandContext ctx)
        {

        }

        [Command("UsuńRole")]
        public async Task RemoveRole(CommandContext ctx)
        {

        }

        [Command("WylistujRole")]
        public async Task ListAllRoles(CommandContext ctx)
        {
            using (var databaseContext = new StaticDBContext())
            {
                var roles = databaseContext.Roles.Select(p => p.Name).ToList();
                await ctx.RespondAsync(string.Join(", ", roles));
            }
        }
    }
}
