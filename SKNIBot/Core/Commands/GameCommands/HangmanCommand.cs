using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using System;
using SKNIBot.Core.Const.GamesConst;
using System.Text;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class HangmanCommand
    {
        private const int Stages = 9;

        private bool _gameStarted;
        private int _actualStage;
        private string _word;
        private string _guessWord;

        public HangmanCommand()
        {
            _gameStarted = false;
            _actualStage = 0;
            _word = "";
            _guessWord = "";
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
                StartGame(type);
            }
            else if(CheckLetter(type[0]))
            {
                _guessWord = AddLetters(type[0]);
            }
            else
            {
                _actualStage++;
            }
            output += _guessWord += "\n";
            for (int j = 0; j < HangmanConst.stages[_actualStage - 1].Length; j++)
            {
                output += HangmanConst.stages[_actualStage - 1][j] += "\n";
            }

            if(_guessWord == _word)
            {
                _gameStarted = false;
                output += "Wygrana";
            }
            if(_actualStage == Stages)
            {
                _gameStarted = false;
                output += "Przegrana";
            }
            await ctx.RespondAsync(output);
        }

        public void StartGame(string type = null)
        {
            _gameStarted = true;
            _actualStage = 1;
            _word = GetWord(type);
            _guessWord = "";
            for (int i = 0; i < _word.Length; i++)
            {
                _guessWord += "◯";
            }
        }

        public string GetWord(string Category = null)
        {
            return "słowo";
        }

        public bool CheckLetter(char letter)
        {
            if(_word.Contains(letter.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string AddLetters(char letter)
        {
            char[] guessWord = _guessWord.ToCharArray();
            for (int i = 0; i < _word.Length; i++)
            { 
                if(_word[i] == letter)
                {
                    guessWord[i] =_word[i];
                }        
            }
            StringBuilder returnValue = new StringBuilder();
            returnValue.Append(guessWord);
            return returnValue.ToString();
        }
    }
}
