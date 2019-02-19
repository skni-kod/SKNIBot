using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class RoleCommand : BaseCommandModule
    {
        [Command("pokażRole")]
        [Description("Pokazuje role, które można przydzielić sobie na tym serwerze.")]
        public async Task ShowRoles(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, ctx.Guild.Id);

                // If there's no roles, send message and exit.
                if (dbServer.AssignRoles.Count == 0)
                {
                    await ctx.RespondAsync("Na tym serwerze nie ma ról, które można sobie przypisać.");
                    return;
                }
                // Prepare message.
                string message = "**Role dostępne na serwerze to:**\n";
                // Get server roles.
                var serverRoles = ctx.Guild.Roles;

                List<DiscordRole> discordRoles = new List<DiscordRole>();
                foreach (AssignRole assignRole in dbServer.AssignRoles)
                {
                    discordRoles.Add(serverRoles.Where(p => p.Id.ToString() == assignRole.RoleID).FirstOrDefault());

                }

                List<DiscordRole> sortedRoles = discordRoles.OrderBy(o => o.Name).ToList();
                foreach (DiscordRole sortedRole in sortedRoles)
                {
                    if (sortedRole != null)
                    {
                        if (sortedRole != sortedRoles[0])
                        {
                            message += ", ";
                        }
                        message += sortedRole.Name;
                    }
                    if (message.Length > 1800)
                    {
                        await ctx.RespondAsync(message);
                        message = "";
                    }
                }
                if (message.Length > 0)
                {
                    await ctx.RespondAsync(message);
                }
            }
        }

        [Command("nadajRole")]
        [Description("Dodaje role z listy ról.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.None)]
        public async Task GiveRole(CommandContext ctx, params string[] message)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, ctx.Guild.Id);

                var serverRoles = ctx.Guild.Roles;

                foreach (var serverRole in serverRoles)
                {
                    if (serverRole.Name == ctx.RawArgumentString)
                    {
                        // Check if role is already in database
                        if (IsRoleInDatabase(dbServer, serverRole.Id))
                        {
                            // Check if user already has this role
                            if (HasUserRole(ctx.Member, serverRole))
                            {
                                await ctx.RespondAsync("Posiadasz już tę role.");
                                return;
                            }

                            // User who triggered is owner, we can add role without problem
                            if (ctx.User == ctx.Guild.Owner)
                            {
                                await ctx.Member.GrantRoleAsync(serverRole, "Rola nadana przez bota przy użyciu systemu nadawania ról. Działanie zostało zainicjowane przez użytkownika.");
                                await ctx.RespondAsync("Rola nadana.");
                            }
                            // User who triggered isn't owner, we need to check if role is lower than the highest role he has
                            else
                            {
                                var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
                                // Role is lower than the highest role user has
                                if (serverRole.Position < userTheHighestRolePosition)
                                {
                                    await ctx.Member.GrantRoleAsync(serverRole, "Rola nadana przez bota przy użyciu systemu nadawania ról. Działanie zostało zainicjowane przez użytkownika.");
                                    await ctx.RespondAsync("Rola nadana.");
                                }
                                // Role is equal or higher than the highest role user has
                                else
                                {
                                    await ctx.RespondAsync("Nie możesz nadać sobie tej roli gdyż jest równa lub wyższa twojej najwyższej roli.");
                                }

                            }
                            return;
                        }

                    }
                }
                await ctx.RespondAsync("Roli nie ma na liście.");
            }

        }

        [Command("odbierzRole")]
        [Description("Dodaje role z listy ról.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.None)]
        public async Task RemoveRole(CommandContext ctx, params string[] message)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, ctx.Guild.Id);

                var serverRoles = ctx.Guild.Roles;

                foreach (var serverRole in serverRoles)
                {
                    if (serverRole.Name == ctx.RawArgumentString)
                    {
                        // Check if role is already in database
                        if (IsRoleInDatabase(dbServer, serverRole.Id))
                        {
                            // Check if user already has this role
                            if (!HasUserRole(ctx.Member, serverRole))
                            {
                                await ctx.RespondAsync("Nie posiadasz tej roli.");
                                return;
                            }

                            // User who triggered is owner, we can add role without problem
                            if (ctx.User == ctx.Guild.Owner)
                            {
                                await ctx.Member.RevokeRoleAsync(serverRole, "Rola odebrana przez bota przy użyciu systemu nadawania ról. Działanie zostało zainicjowane przez użytkownika.");
                                await ctx.RespondAsync("Rola odebrana.");
                            }
                            // User who triggered isn't owner, we need to check if role is lower than the highest role he has
                            else
                            {
                                var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
                                // Role is lower than the highest role user has
                                if (serverRole.Position < userTheHighestRolePosition)
                                {
                                    await ctx.Member.RevokeRoleAsync(serverRole, "Rola odebrana przez bota przy użyciu systemu nadawania ról. Działanie zostało zainicjowane przez użytkownika.");
                                    await ctx.RespondAsync("Rola odebrana.");
                                }
                                // Role is the highest role user has
                                else
                                {
                                    await ctx.RespondAsync("Nie możesz odebrać sobie tej roli gdyż to twoja najwyższa rola.");
                                }

                            }
                            return;
                        }

                    }
                }
                await ctx.RespondAsync("Roli nie ma na liście.");
            }

        }

        [Command("dodajRole")]
        [Description("Dodaje rolę do listy roli jakie mogą sobie przydzielać członkowie serwera.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task AddRole(CommandContext ctx, params string[] message)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, ctx.Guild.Id);

                // Get server roles.
                var serverRoles = ctx.Guild.Roles;

                foreach (var serverRole in serverRoles)
                {
                    if (serverRole.Name == ctx.RawArgumentString)
                    {
                        // Check if role is already in database
                        if (IsRoleInDatabase(dbServer, serverRole.Id))
                        {
                            await ctx.RespondAsync("Rola jest już na liście.");
                            return;
                        }

                        // User who triggered is owner, we can add role without problem
                        if (ctx.User == ctx.Guild.Owner)
                        {
                            AssignRole assingRole = new AssignRole(serverRole.Id);
                            assingRole.Server = dbServer;
                            databaseContext.Add(assingRole);
                            databaseContext.SaveChanges();
                            await ctx.RespondAsync("Rola dodana do listy ról.");
                        }
                        // User who triggered isn't owner, we need to check if role is lower than the highest role he has
                        else
                        {
                            var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
                            // Role is lower than the highest role user has
                            if (serverRole.Position < userTheHighestRolePosition)
                            {
                                AssignRole assingRole = new AssignRole(serverRole.Id);
                                assingRole.Server = dbServer;
                                databaseContext.Add(assingRole);
                                databaseContext.SaveChanges();
                                await ctx.RespondAsync("Rola dodana do listy ról.");
                            }
                            // Role is equal or higher than the highest role user has
                            else
                            {
                                await ctx.RespondAsync("Nie możesz dodać tej roli gdyż jest równa lub wyższa twojej najwyższej roli.");
                            }

                        }

                        return;
                    }
                }
                await ctx.RespondAsync("Podana rola nie istnieje.");
            }
        }

        [Command("usuńRole")]
        [Description("Usuwa rolę z listy roli jakie mogą sobie przydzielać członkowie serwera.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task DeleteRole(CommandContext ctx, params string[] message)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, ctx.Guild.Id);

                // Get server roles.
                var serverRoles = ctx.Guild.Roles;

                foreach (var serverRole in serverRoles)
                {
                    if (serverRole.Name == ctx.RawArgumentString)
                    {

                        // Check if role is already in database
                        if (!IsRoleInDatabase(dbServer, serverRole.Id))
                        {
                            await ctx.RespondAsync("Roli nie ma na liście.");
                            return;
                        }

                        // User who triggered is owner, we can add role without problem
                        if (ctx.User == ctx.Guild.Owner)
                        {
                            dbServer.AssignRoles.RemoveAll(p => p.RoleID == serverRole.Id.ToString());
                            databaseContext.SaveChanges();
                            await ctx.RespondAsync("Rola usunięta z listy ról.");
                        }
                        // User who triggered isn't owner, we need to check if role is lower than the highest role he has
                        else
                        {
                            var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
                            // Role is lower than the highest role user has
                            if (serverRole.Position < userTheHighestRolePosition)
                            {
                                dbServer.AssignRoles.RemoveAll(p => p.RoleID == serverRole.Id.ToString());
                                databaseContext.SaveChanges();
                                await ctx.RespondAsync("Rola usunięta z listy ról.");
                            }
                            // Role is equal or higher than the highest role user has
                            else
                            {
                                await ctx.RespondAsync("Nie możesz usunąć tej roli gdyż jest równa lub wyższa twojej najwyższej roli.");
                            }

                        }

                        return;
                    }
                }
                await ctx.RespondAsync("Podana rola nie istnieje.");
            }
        }

        private Server GetServerFromDatabase(DynamicDBContext databaseContext, ulong GuildId)
        {
            Server dbServer = databaseContext.Servers.Where(p => p.ServerID == GuildId.ToString()).Include(p => p.AssignRoles).FirstOrDefault();

            //If server is not present in database add it.
            if (dbServer == null)
            {
                dbServer = new Server(GuildId);
                dbServer.AssignRoles = new List<AssignRole>();
                databaseContext.Add(dbServer);
                databaseContext.SaveChanges();
            }
            return dbServer;
        }

        private bool HasUserRole(DiscordMember member, DiscordRole role)
        {
            foreach (var memberRole in member.Roles)
            {
                if (memberRole == role)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetTheHighestRolePosition(List<DiscordRole> roles)
        {
            int position = 0;
            foreach (var role in roles)
            {
                if (role.Position > position)
                {
                    position = role.Position;
                }
            }

            return position;
        }

        private bool IsRoleInDatabase(Server server, ulong roleId)
        {
            if (server.AssignRoles == null || server.AssignRoles.Count == 0)
            {
                return false;
            }
            if (server.AssignRoles.FirstOrDefault(p => p.RoleID == roleId.ToString()) == null)
            {
                return false;
            }
            return true;
        }
    }
}
