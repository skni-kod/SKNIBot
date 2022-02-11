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
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.RolesService;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    class RoleCommand : BaseCommandModule
    {
        private AssignRolesService _assignRolesService;

        public RoleCommand(AssignRolesService assignRolesService)
        {
            _assignRolesService = assignRolesService;
        }

        [Command("pokazRole")]
        [Description("Pokazuje role, które można przydzielić sobie na tym serwerze.")]
        public async Task ShowRoles(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var assignRoles = _assignRolesService.GetRoles(ctx.Guild.Id);

            if (assignRoles.Count == 0)
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", "Na tym serwerze nie ma ról, które można sobie przypisać.");
            }
            else
            {
                // Get server roles.
                var serverRoles = ctx.Guild.Roles;

                List<DiscordRole> discordRoles = new List<DiscordRole>();
                foreach (ulong roleId in assignRoles)
                {
                    discordRoles.Add(serverRoles.Where(p => p.Value.Id == roleId).FirstOrDefault().Value);
                }
                discordRoles.RemoveAll(p => p == null);
                List<DiscordRole> sortedRoles = discordRoles.OrderBy(o => o.Name).ToList();
                await PostLongMessageHelper.PostLongMessage(ctx, sortedRoles.Select(p => p.Name).ToList(), null, "**Role dostępne na serwerze to:**");
            }
        }

        [Command("nadajRole")]
        [Description("Dodaje role z listy ról.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task GiveRole(CommandContext ctx, [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();

            if (role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` nie istnieje.");
                return;
            }

            if (HasUserRole(ctx.Member, role.Value))
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Posiadasz już rolę `{message}`.");
                return;
            }

            if (_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                if (!CanBotModifyThisRole(role.Value, ctx.Guild.CurrentMember.Roles.ToList()))
                {
                    await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Moje role są za nisko abym mógł nadać rolę `{message}`.");
                    return;
                }

                await ctx.Member.GrantRoleAsync(role.Value, "Rola nadana przez bota przy użyciu systemu nadawania ról. Działanie zostało zainicjowane przez użytkownika.");
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` nadana.");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Roli `{message}` nie ma na liście.");
            }
        }

        [Command("odbierzRole")]
        [Description("Odbiera role z listy ról.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task RemoveRole(CommandContext ctx, [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();

            if (role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` nie istnieje.");
                return;
            }

            if (!HasUserRole(ctx.Member, role.Value))
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Nie posiadasz roli `{message}`.");
                return;
            }

            if (_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                if (!CanBotModifyThisRole(role.Value, ctx.Guild.CurrentMember.Roles.ToList()))
                {
                    await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Moje role są za nisko abym mógł odebrać rolę `{message}`.");
                    return;
                }
                await ctx.Member.RevokeRoleAsync(role.Value, "Rola odebrana przez bota przy użyciu systemu nadawania ról. Działanie zostało zainicjowane przez użytkownika.");
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` odebrana.");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Roli `{message}` nie ma na liście.");
            }
        }


        [Command("dodajRole")]
        [Description("Dodaje rolę do listy roli jakie mogą sobie przydzielać członkowie serwera.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task AddRole(CommandContext ctx, [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();

            if (role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` nie istnieje.");
                return;
            }

            if (_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` jest już na liście.");
                return;
            }

            // User who triggered is owner, we can add role without problem or user who triggered isn't owner, we need to check if role is lower than the highest role he has
            var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
            if (ctx.User == ctx.Guild.Owner || role.Value.Position < userTheHighestRolePosition)
            {
                // Add role to database
                _assignRolesService.AddRoleToDatabase(ctx.Guild.Id, role.Value.Id);
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` dodana do listy ról.");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Nie możesz dodać roli `{message}`, gdyż jest równa lub wyższa twojej najwyższej roli.");
            }
        }

        [Command("usunRole")]
        [Description("Usuwa rolę z listy roli jakie mogą sobie przydzielać członkowie serwera.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task DeleteRole(CommandContext ctx, [RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();

            if (role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` nie istnieje.");
                return;
            }

            if (!_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Roli `{message}` nie ma na liście.");
                return;
            }

            // User who triggered is owner, we can add role without problem or user who triggered isn't owner, we need to check if role is lower than the highest role he has
            var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
            if (ctx.User == ctx.Guild.Owner || role.Value.Position < userTheHighestRolePosition)
            {
                // Add role to database
                _assignRolesService.RemoveRoleFromDatabase(ctx.Guild.Id, role.Value.Id);
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Rola `{message}` usunięta z listy ról.");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "System ról", $"{ctx.User.Mention} Nie możesz usunąć roli `{message}`, gdyż jest równa lub wyższa twojej najwyższej roli.");
            }
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

        private bool CanBotModifyThisRole(DiscordRole role, List<DiscordRole> botRoles)
        {
            int highestBotRole = GetTheHighestRolePosition(botRoles);
            if (role.Position < highestBotRole)
            {
                return true;
            }
            else
            {
                return false;
            }
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
    }
}
