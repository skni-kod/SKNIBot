using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ManagementCommands
{
    [CommandsGroup("Zarządzanie")]
    public class InfoCommands : BaseCommandModule
    {
        private int CountChannels(CommandContext ctx, string Type)
        {
            int Number = 0;
            foreach (var item in ctx.Guild.Channels.Values)
            {
                if (item.Type.ToString() == Type)
                    Number++;
            }
            return Number;
        }
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
        [Command("oserwerze")]
        [Description("Pokazuje info o serwerze.")]
        public async Task Info(CommandContext ctx)
        {
            int count = ctx.Guild.Roles.Count;
            string time = ctx.Guild.CreationTimestamp.ToString();
            int size = time.Length;
            time = time.Remove(size - 6);
            Node[] role = new Node[count];
            int voice = CountChannels(ctx, "Voice");
            int text = CountChannels(ctx, "Text");
            int total = voice + text;
            StringBuilder msgContent = new StringBuilder()
            .AppendLine("**:globe_with_meridians: [Strona koła](https://kod.prz.edu.pl/#/)**")
            .Append("**:id: ID serwera: **`").Append(ctx.Guild.Id).Append("`").AppendLine()
            .Append(":bust_in_silhouette: **Właściciel: **").Append(ctx.Guild.Owner.Nickname).AppendLine()
            .Append(":calendar: **Serwer utworzony dnia: **").Append(time).AppendLine()
            .Append(":busts_in_silhouette: **Liczba użytkowników: **").Append(ctx.Guild.MemberCount).AppendLine()
            .Append(":arrow_forward: **Kanały: **Voice: `").Append(voice).Append("`|Text: `").Append(text).Append("`|").Append("W sumie: ").Append("**").Append(total).Append("**").AppendLine()
            .Append("**:arrow_forward:  Role na serwerze: **").Append(count).AppendLine();
            int index = 0;
            foreach (var item in ctx.Guild.Roles.Values)
            {
                Node tmp=new Node(item.Position, item.Name);
                role[index] = tmp;
                index++;
            }
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
            for(int j = 0; j < count; j++)
                lRol[j] = role[j].Name;
            var roleList = string.Join(", ", lRol);
            msgContent.AppendLine(roleList);
            var embed = new DSharpPlus.Entities.DiscordEmbedBuilder()
                .WithTitle(ctx.Guild.Name)
                .WithThumbnail(ctx.Guild.IconUrl)
                .WithDescription(msgContent.ToString())
                .WithColor(Helpers.ColorHelper.RandomColor());
            await ctx.RespondAsync(embed: embed);
        }
    }
}
