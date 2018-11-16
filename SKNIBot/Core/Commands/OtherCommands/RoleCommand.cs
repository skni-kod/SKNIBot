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
        [Command("DodajRolę")]
        [Aliases("DodajRole", "PrzypiszRole", "PrzypiszRolę")]
        [Description("Dodaj rolę związaną z technologią którą znasz.")]
        public async Task AddRole(CommandContext ctx, string name)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                var role = ctx.Guild.Roles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

                if (role == null)
                {
                    await ctx.RespondAsync("Nie ma takiej roli!");
                    return;
                }

                if (!databaseContext.Roles.Any(p => p.RoleId == role.Id.ToString()))
                {
                    await ctx.RespondAsync("Nie możesz przypisać tej roli!");
                    return;
                }

                if (ctx.Member.Roles.Contains(role))
                {
                    await ctx.RespondAsync("Jesteś już przypisany do tej roli!");
                    return;
                }

                await ctx.Member.GrantRoleAsync(role, "Przypisana przez Bocika");
                await ctx.RespondAsync("Rola przypisana!");
            }
        }

        [Command("UsuńRolę")]
        [Aliases("UsuńRole", "UsunRole")]
        [Description("Usuń przypisaną rolę.")]
        public async Task RemoveRole(CommandContext ctx, string name)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                var role = ctx.Guild.Roles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

                if (role == null)
                {
                    await ctx.RespondAsync("Nie ma takiej roli!");
                    return;
                }

                if (!databaseContext.Roles.Any(p => p.RoleId == role.Id.ToString()))
                {
                    await ctx.RespondAsync("Nie możesz zostać usunięty z tej roli!");
                    return;
                }

                if (!ctx.Member.Roles.Contains(role))
                {
                    await ctx.RespondAsync("Nie jesteś przypisany do tej roli!");
                    return;
                }

                await ctx.Member.RevokeRoleAsync(role, "Usunięta przez Bocika");
                await ctx.RespondAsync("Rola usunięta!");
            }
        }

        [Command("WylistujRole")]
        [Aliases("Role")]
        [Description("Wylistuj wszystkie role które możesz przypisać sobie za pomocą komend `!DodajRole`.")]
        public async Task ListAllRoles(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                var roles = ctx.Guild.Roles.Where(p => databaseContext.Roles.Select(r => r.RoleId).Contains(p.Id.ToString())).ToList();
                await ctx.RespondAsync(string.Join(", ", roles.Select(p => p.Name)));
            }
        }
    }
}
