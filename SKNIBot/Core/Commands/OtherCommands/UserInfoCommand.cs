using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    public class UserInfoCommands : BaseCommandModule
    {
        private class Node
             {
                 public int Number;
                 public string Name;
                 public Node(int a, string b)
                 {
                     Number = a;
                     Name = b;
                 }
             }

        [Command("userInfo")]
        [Description(
            "Zwraca podstawowe informacje o podanym użytkowniku. Uruchomiona bez parametrów zwraca informacje o wywołującym ją użytkowniku")]
        public async Task SendUserInfo(CommandContext ctx,
            [Description("Nazwa użytkownika bądź wzmianka (opcjonalne)")] DiscordMember member = null)
        {
            // Guard for no-parameter execution
            if (member == null)
            {
                member = ctx.Member;
            }

            // Supressing the warnings
            if (member != null)
            {
                // Getting basic information about given user
                string username = new StringBuilder()
                    .Append(member.Username)
                    .Append(" #")
                    .Append(member.Discriminator)
                    .ToString();
                ulong userId = member.Id;
                string cDate = member.CreationTimestamp.UtcDateTime.ToString(CultureInfo.CurrentCulture);
                string jDate = member.JoinedAt.UtcDateTime.ToString(CultureInfo.CurrentCulture);

                //Roles
                int index = 0;
                int count = member.Roles.Count();
                Node[] role = new Node[count];
                foreach (var item in member.Roles)
                {
                    Node tmp = new Node(item.Position, item.Name);
                    role[index] = tmp;
                    index++;
                }

                // I dont like this. Copied from InfoCommands
                for (int j = 0; j <= role.Length - 2; j++)
                {
                    for (int i = 0; i <= role.Length - 2; i++)
                    {
                        if (role[i].Number < role[i + 1].Number)
                        {
                            Node temp = role[i + 1];
                            role[i + 1] = role[i];
                            role[i] = temp;
                        }
                    }
                }

                string[] lRol = new string[count];
                for (int j = 0; j < count; j++)
                    lRol[j] = role[j].Name;
                var roleList = string.Join(", ", lRol);

                // Building final message
                var msgContent = new StringBuilder()
                    .Append("** :floppy_disk: ID użytkownika: **").Append(userId).AppendLine()
                    .Append("** :calendar_spiral: Data utworzenia konta: **").Append(cDate).AppendLine()
                    .Append("** :calendar: Data dołączenia na serwer: **").Append(jDate).AppendLine()
                    .Append("** :office_worker: Role: **").Append(roleList).AppendLine();

                // Preparing response
                var embed = new DiscordEmbedBuilder()
                    .WithTitle(username)
                    .WithThumbnail(member.AvatarUrl)
                    .WithDescription(msgContent.ToString())
                    .WithColor(Helpers.ColorHelper.RandomColor());
                await ctx.RespondAsync(embed: embed);
            }
            else
            {
                await ctx.RespondAsync("Wystąpił problem w rozpoznaniu użytkownika. Spróbuj pownownie!");
            }
        }
        
    }
}
