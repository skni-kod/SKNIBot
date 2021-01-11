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
            string tresc;
            tresc =
                "**:id: ID serwera: **`" + ctx.Guild.Id + "`\n" +
                ":bust_in_silhouette: **Właściciel: **" + ctx.Guild.Owner.Nickname +
                "\n:calendar: **Serwer utworzony dnia: **"+time+
                "\n:busts_in_silhouette: **Liczba użytkowników: **" + ctx.Guild.MemberCount+
                "\n:arrow_forward: **Kanały: **Voice: `"+Voice+"`|Text: `"+Text+"`|"+"W sumie: "+ "**" + Total + "**" +
                "\n**:arrow_forward:  Role na serwerze: **" + count+"\n";
            //"\n**Channls: **"+ctx.Guild.Channels;
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
            var listaRol = string.Join(", ", lRol);
            tresc += listaRol;
            await Helpers.PostEmbedHelper.PostEmbed(ctx, ctx.Guild.Name, tresc);
        }

    }
}
