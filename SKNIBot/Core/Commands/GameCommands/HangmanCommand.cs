using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using System;
using SKNIBot.Core.Const.GamesConst;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class HangmanCommand
    {
        private const int Stages = 9;

        private bool _gameStarted;
        private int _actualStage;
        private string _word;

        public HangmanCommand()
        {
            _gameStarted = false;
            _actualStage = 0;
            _word = "";
        }
        [Command("wisielec")]
        [Description("Gra w wisielca.")]
        [Aliases("hangman")]
        public async Task Czesc(CommandContext ctx, [Description("Kategoria")] string type = null)
        {
            await ctx.TriggerTypingAsync();
            string output = "";
            if(_gameStarted == false)
            {
                _gameStarted = true;
                _actualStage = 1;
            }
            //losowanie i wyświetlenie słowa
            _actualStage++; //Inkrementować gdy nie ma słowa
            for (int j = 0; j < HangmanConst.stages[_actualStage - 1].Length; j++)
            {
                output += HangmanConst.stages[_actualStage - 1][j] += "\n";
            }
            //W razie przegranej
            if(_actualStage == Stages)
            {
                _gameStarted = false;
            }
            await ctx.RespondAsync(output);
        }
    }
}
