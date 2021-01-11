using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ManagementCommands
{
    [CommandsGroup("Zarządzanie")]
    public class InfoCommands : BaseCommandModule
    {
        public int CountChannels(CommandContext ctx, string Type)
        {
            int Number = 0;
            foreach (var item in ctx.Guild.Channels.Values)
            {
                if (item.Type.ToString() == Type)
                    Number++;
            }
            return Number;
        }
        class Node
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
            int Voice = CountChannels(ctx, "Voice");
            int Text = CountChannels(ctx, "Text");
            int Total = Voice + Text;
            StringBuilder MsgContent = new StringBuilder();

            MsgContent.Append("**:id: ID serwera: **`").Append(ctx.Guild.Id).Append("`").AppendLine();
            MsgContent.Append(":bust_in_silhouette: **Właściciel: **").Append(ctx.Guild.Owner.Nickname).AppendLine();
            MsgContent.Append(":calendar: **Serwer utworzony dnia: **").Append(time).AppendLine();
            MsgContent.Append(":busts_in_silhouette: **Liczba użytkowników: **").Append(ctx.Guild.MemberCount).AppendLine();
            MsgContent.Append(":arrow_forward: **Kanały: **Voice: `").Append(Voice).Append("`|Text: `").Append(Text).Append("`|").Append("W sumie: ").Append("**").Append(Total).Append("**").AppendLine();
            MsgContent.Append("**:arrow_forward:  Role na serwerze: **").Append(count).AppendLine();
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
            var RoleList = string.Join(", ", lRol);
            MsgContent.AppendLine(RoleList);
            await Helpers.PostEmbedHelper.PostEmbed(ctx, ctx.Guild.Name, MsgContent.ToString());
        }
    }
}
