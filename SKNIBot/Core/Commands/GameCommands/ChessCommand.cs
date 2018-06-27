using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Proxima.Core;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.Session;
using Color = Proxima.Core.Commons.Colors.Color;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class ChessCommand
    {
        private Dictionary<string, Image> _images;
        private List<Position> _selectedPositions;
        private GameSession _gameSession;

        private const string ChessImagesPath = "Images/Chess/";

        public ChessCommand()
        {
            ProximaCore.Init();

            _images = new Dictionary<string, Image>();
            _selectedPositions = new List<Position>();

            CreateSession();
            LoadChessImages();
        }

        [Command("szachy")]
        [Description("Dev.")]
        [Aliases("chess", "c")]
        public async Task Chess(CommandContext ctx, string action = null)
        {
            if (action == "new")
            {
                CreateSession();
            }
            else
            {
                _gameSession.UpdateRemainingTime(Color.White, 200);
                _gameSession.UpdateRemainingTime(Color.Black, 200);

                var from = action.Substring(0, 2);
                var to = action.Substring(2, 2);

                var fromPosition = PositionConverter.ToPosition(from);
                var toPosition = PositionConverter.ToPosition(to);

                var moveValidationBitboard = new Bitboard(_gameSession.Bitboard);
                moveValidationBitboard.Calculate(GeneratorMode.CalculateAttacks | GeneratorMode.CalculateMoves, false);

                var move = moveValidationBitboard.Moves.FirstOrDefault(p => p.From == fromPosition && p.To == toPosition);
                if (move == null)
                {
                    await ctx.RespondAsync("Invalid move");
                    return;
                }

                var validationBitboardAfterMove = moveValidationBitboard.Move(move);
                validationBitboardAfterMove.Calculate(false);

                if (validationBitboardAfterMove.IsCheck(Color.White))
                {
                    await ctx.RespondAsync("Invalid move");
                    return;
                }

                _gameSession.Move(Color.White, fromPosition, toPosition);

                _selectedPositions.Clear();
                _selectedPositions.Add(fromPosition);
                _selectedPositions.Add(toPosition);

                await ctx.RespondWithFileAsync(GetBoardImage(), "board.png");

                var aiMove = _gameSession.MoveAI(Color.Black);

                _selectedPositions.Clear();
                _selectedPositions.Add(aiMove.PVNodes[0].From);
                _selectedPositions.Add(aiMove.PVNodes[0].To);

                await ctx.RespondWithFileAsync(GetBoardImage(), "board.png");
            }

        }

        private void LoadChessImages()
        {
            var imagesList = Directory.GetFiles(ChessImagesPath);
            foreach (var imagePath in imagesList)
            {
                var pureName = imagePath.Split('/').Last().Split('.').First();
                _images.Add(pureName, Image.FromFile(imagePath));
            }
        }

        private void CreateSession()
        {
            _gameSession = new GameSession(0);
            _gameSession.OnThinkingOutput += GameSession_OnThinkingOutput;
        }

        private void GameSession_OnThinkingOutput(object sender, Proxima.Core.AI.ThinkingOutputEventArgs e)
        {
            Console.WriteLine($"{e.AIResult.Depth}: TN:{e.AIResult.Stats.TotalNodes} NPS:{e.AIResult.NodesPerSecond}");
        }

        private Stream GetBoardImage()
        {
            var board = new Bitmap(527, 535);
            var graphic = Graphics.FromImage(board);

            var odd = false;
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    var field = odd ? _images["Field1"] : _images["Field2"];
                    graphic.DrawImage(field, new Point(15 + x * 64, y * 64));

                    odd = !odd;
                }

                odd = !odd;
            }

            for (var x = 1; x <= 8; x++)
            {
                graphic.DrawString(((char)(x + 'a' - 1)).ToString(), new Font(new FontFamily("Liberation Mono"), 12, FontStyle.Bold), new SolidBrush(System.Drawing.Color.White), x * 64 - 25, 513);
                graphic.DrawString((8 - x + 1).ToString(), new Font(new FontFamily("Liberation Mono"), 12, FontStyle.Bold), new SolidBrush(System.Drawing.Color.White), 0, (x - 1) * 64 + 20);
            }

            var friendlyBoard = new FriendlyBoard(_gameSession.Bitboard);
            foreach (var piece in friendlyBoard.Pieces)
            {
                var image = _images[GetImageNameByPiece(piece.Type, piece.Color)];
                graphic.DrawImage(image, new Point(15 + (piece.Position.X - 1) * 64, (8 - piece.Position.Y) * 64));
            }

            foreach (var selected in _selectedPositions)
            {
                graphic.DrawImage(_images["InternalSelection"], new Point(15 + (selected.X - 1) * 64, (8 - selected.Y) * 64));
            }

            var stream = new MemoryStream();
            board.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            return stream;
        }

        private string GetImageNameByPiece(PieceType type, Color color)
        {
            return color.ToString() + type.ToString();
        }
    }
}
