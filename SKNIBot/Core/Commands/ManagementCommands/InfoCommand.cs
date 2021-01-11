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

            MsgContent.AppendLine("**:id: ID serwera: **`" + ctx.Guild.Id+"`");
            MsgContent.AppendLine(":bust_in_silhouette: **Właściciel: **" + ctx.Guild.Owner.Nickname);
            MsgContent.AppendLine(":calendar: **Serwer utworzony dnia: **" + time);
            MsgContent.AppendLine(":busts_in_silhouette: **Liczba użytkowników: **" + ctx.Guild.MemberCount);
            MsgContent.AppendLine(":arrow_forward: **Kanały: **Voice: `" + Voice + "`|Text: `" + Text + "`|" + "W sumie: " + "**" + Total + "**");
            MsgContent.AppendLine("**:arrow_forward:  Role na serwerze: **" + count);
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
