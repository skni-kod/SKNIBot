using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Const.GamesConst;
using SKNIBot.Core.Database;
using System.Linq;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class HangmanCommand
    {
        private const int Stages = 9;

        private Random _random;

        /// <summary>
        /// Czu gra rozpoczęta
        /// </summary>
        private bool _gameStarted;
        /// <summary>
        /// Aktualny poziom
        /// </summary>
        private int _actualStage;
        /// <summary>
        /// Słowo do odgadnięcia
        /// </summary>
        private string _word;
        /// <summary>
        /// Aktualnie odgadywane słowo
        /// </summary>
        private string _guessWord;

        /// <summary>
        /// Podane wcześniej litery
        /// </summary>
        private List<char> _guessedLetters;

        public HangmanCommand()
        {
            _gameStarted = false;
            _actualStage = 0;
            _word = "";
            _guessWord = "";
            _guessedLetters = new List<char>();
            _random = new Random();
        }

        [Command("wisielec")]
        [Description("Gra w wisielca. Możliwe kategorie `państwa`, `zwierzęta`, `rzeczy` lub brak kategrii.")]
        [Aliases("hangman")]
        public async Task Hangman(CommandContext ctx, [Description("Kategoria")] string type = null)
        {
            await ctx.TriggerTypingAsync();

            var output = "";
            //Jeżeli gra nie jest rozpoczęta, rozpocznij
            if (_gameStarted == false)
            {
                StartGame(type);
            }
            //Gdy brak litery
            else if (type == null)
            {
                output += "Podaj literę \n";
            }
            else if (type == _word)
            {
                _guessWord = _word;
            }
            //W innym wypadku sprawdź czy dana litera występuje
            else if (type.Length == 1)
            {
                if (CheckLetter(type[0]))
                {
                    _guessWord = AddLetters(type[0]);
                }
                //W razie pomyłki zwiększ licznik błędów
                else
                {
                    _actualStage++;
                }
                //Dodaj literę do listy
                if (!_guessedLetters.Contains(type[0]))
                {
                    _guessedLetters.Add(type[0]);
                }
            }

            //Generuj wyjście
            output += GenerateOutput();

            //Generuj wyjście
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#5588EE")
            };
            embed.AddField("Hangman", output);

            await ctx.RespondAsync("", false, embed);
        }

        /// <summary>
        /// Rozpoczyna grę
        /// </summary>
        /// <param name="category">Kategoria</param>
        private void StartGame(string category = null)
        {
            _gameStarted = true;
            _actualStage = 1;
            _word = GetWord(category);
            var word = _word.ToCharArray();
            _guessWord = "";
            _guessedLetters = new List<char>();
            for (var i = 0; i < word.Length; i++)
            {
                if (word[i] != ' ')
                {
                    _guessWord += "◯";
                }
                else
                {
                    _guessWord += " ";
                }

            }
        }

        /// <summary>
        /// Losuje słowo
        /// </summary>
        /// <param name="Category">kategoria</param>
        /// <returns></returns>
        private string GetWord(string Category = null)
        {

            using (var databaseContext = new DatabaseContext())
            {
                IQueryable<string> wordList = null;

                if (Category == null)
                {
                    wordList = databaseContext.HangmanWords
                        .Select(p => p.Word);
                }
                else if (Category == "państwa")
                {
                    wordList = databaseContext.HangmanWords
                        .Where(p => p.HangmanCategoryID == 1)
                        .Select(p => p.Word);
                }
                else if (Category == "zwierzęta")
                {
                    wordList = databaseContext.HangmanWords
                        .Where(p => p.HangmanCategoryID == 2)
                        .Select(p => p.Word);
                }
                else if (Category == "rzeczy")
                {
                    wordList = databaseContext.HangmanWords
                        .Where(p => p.HangmanCategoryID == 3)
                        .Select(p => p.Word);
                }
                var words = wordList.ToList();

                var wordIndex = _random.Next(0, words.Count);
                var word = words[wordIndex];
                return word;
            }
        }

        /// <summary>
        /// Sprawdza czy litera występuje w słowie
        /// </summary>
        /// <param name="letter">Litera do sprawdzenia</param>
        /// <returns></returns>
        private bool CheckLetter(char letter)
        {
            return _word.ToLower().Contains(letter.ToString());
        }

        /// <summary>
        /// Dodaje literę do ciągu znaków w miejscu w którym występuje w haśle
        /// </summary>
        /// <param name="letter">Litera</param>
        /// <returns></returns>
        private string AddLetters(char letter)
        {
            var guessWord = _guessWord.ToCharArray();
            var word = _word.ToLower();
            for (var i = 0; i < _word.Length; i++)
            {
                if (word[i] == letter)
                {
                    guessWord[i] = _word[i];
                }
            }
            var returnValue = new StringBuilder();
            returnValue.Append(guessWord);
            return returnValue.ToString();
        }

        /// <summary>
        /// Dodaje do wyjścia szybienicę
        /// </summary>
        /// <param name="output"></param>
        private string GenerateOutput()
        {
            string output = "";
            output += _guessWord;
            output += "\n";
            for (var j = 0; j < HangmanConst.Stages[_actualStage - 1].Length; j++)
            {
                output += HangmanConst.Stages[_actualStage - 1][j];
                output += "\n";
            }
            for (var j = 0; j < _guessedLetters.Count; j++)
            {
                output += _guessedLetters[j];
                output += " ";
            }
            output += "\n";
            //W razie wygranej
            if (_guessWord.Equals(_word))
            {
                _gameStarted = false;
                output += "Wygrana \nSłowo: " + _word;
            }
            //W razie przegranej
            if (_actualStage == Stages)
            {
                _gameStarted = false;
                output += "Przegrana \nSłowo: " + _word;
            }
            return output;
        }
    }
}
