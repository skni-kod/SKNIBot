using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const.GamesConst;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class HangmanCommand
    {
        private const int Stages = 9;

        private Random _random;
        private List<string> _words = new List<string> { "słowo", "shuka", "naleśniki" };

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
            _random = new Random();
        }

        [Command("wisielec")]
        [Description("Gra w wisielca.")]
        [Aliases("hangman")]
        public async Task Hangman(CommandContext ctx, [Description("Kategoria")] string type = null)
        {
            await ctx.TriggerTypingAsync();

            string output = "";
            //Jeżeli gra nie jest rozpoczęta, rozpocznij
            if (_gameStarted == false)
            {
                StartGame(type);
            }
            //Gdy brak litery
            else if(type == null)
            {
                output += "Podaj literę \n";
            }
            //W innym wypadku sprawdź czy dana litera występuje
            else if (CheckLetter(type[0]))
            {
                _guessWord = AddLetters(type[0]);
            }
            //W razie pomyłki zwiększ licznik błędów
            else
            {
                _actualStage++;
            }
            //Generuj wyjście
            output += _guessWord;
            output += "\n";
            for (int j = 0; j < HangmanConst.Stages[_actualStage - 1].Length; j++)
            {
                output += HangmanConst.Stages[_actualStage - 1][j];
                output += "\n";
            }
            //W razie wygranej
            if(_guessWord.Equals(_word))
            {
                _gameStarted = false;
                output += "Wygrana";
            }
            //W razie przegranej
            if(_actualStage == Stages)
            {
                _gameStarted = false;
                output += "Przegrana";
            }

            await ctx.RespondAsync(output);
        }

        /// <summary>
        /// Rozpoczyna grę
        /// </summary>
        /// <param name="type">Kategoria</param>
        public void StartGame(string category = null)
        {
            _gameStarted = true;
            _actualStage = 1;
            _word = GetWord(category);
            _guessWord = "";
            for (int i = 0; i < _word.Length; i++)
            {
                _guessWord += "◯";
            }
        }

        /// <summary>
        /// Losuje słowo
        /// </summary>
        /// <param name="Category">kategoria</param>
        /// <returns></returns>
        public string GetWord(string Category = null)
        {
            var wordIndex = _random.Next(0, _words.Count);
            var word = _words[wordIndex];
            return word;
        }

        /// <summary>
        /// Sprawdza czy litera występuje w słowie
        /// </summary>
        /// <param name="letter">Litera do sprawdzenia</param>
        /// <returns></returns>
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

        /// <summary>
        /// Dodaje literę do ciągu znaków w miejscu w którym występuje w haśle
        /// </summary>
        /// <param name="letter">Litera</param>
        /// <returns></returns>
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
