using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class GraphCommand : BaseCommandModule
    {
        private Random _random;

        private const int MaxGraphLength = 50;
        private const int MaxSingleRunLength = 10;
        private const int GraphsCount = 5;
        private const char UpChar = '⎻';
        private const char DownChar = '_';

        public GraphCommand()
        {
            _random = new Random();
        }

        [Command("graf")]
        [Description("Rysuje losowy przebieg.")]
        [Aliases("Przebieg")]
        public async Task Graph(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var output = new StringBuilder();
            output.Append("```");

            var sequence = new List<int>();
            while (sequence.Sum() <= MaxGraphLength)
            {
                sequence.Add(_random.Next(1, MaxSingleRunLength));
            }

            for (var i = 0; i < GraphsCount; i++)
            {
                output.Append(GetNewGraph(sequence, i));
                output.Append("\r\n\r\n");
            }

            output.Append("```");
            await ctx.RespondAsync(output.ToString());
        }

        private string GetNewGraph(List<int> sequence, int index)
        {
            var output = new StringBuilder();
            var currentUp = index % 2 == 0;

            sequence.Add(sequence[0]);
            sequence.RemoveAt(0);

            foreach (var s in sequence)
            {
                var currentChar = currentUp ? UpChar : DownChar;
                output.Append(new string(currentChar, s));

                output.Append("|");
                currentUp = !currentUp;
            }

            output.Remove(output.Length - 1, 1);
            return output.ToString();
        }
    }
}
