using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Helpers
{
    class PostLongMessageHelper
    {
        public static async Task PostLongMessage(CommandContext ctx, List<string> strings, string header = null)
        {
            StringBuilder response = new StringBuilder(2000);
            foreach (string s in strings)
            {
                response.Append(s);

                if (response.Length > 1800)
                {
                    await PostEmbedHelper.PostEmbed(ctx, header, response.ToString());
                    response.Clear();
                    continue;
                }
                if (strings.IndexOf(s) != strings.Count - 1)
                {
                    response.Append(", ");
                }
            }
            if (response.Length > 0)
            {
                await PostEmbedHelper.PostEmbed(ctx, header, response.ToString());
                return;
            }
        }
    }
}
