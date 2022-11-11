using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using Proxima.Core;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.Session;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Text;
using Color = Proxima.Core.Commons.Colors.Color;

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class ChessCommand : BaseCommandModule
    {
        private Dictionary<string, Image<Rgba32>> _images;
        private List<Position> _selectedPositions;
        private List<ulong> _messageIds;
        private GameSession _gameSession;

        private const string ChessImagesPath = "Images/Chess/";

        public ChessCommand()
        {
            ProximaCore.Init();

            _images = new Dictionary<string, Image<Rgba32>>();
            _selectedPositions = new List<Position>();
            _messageIds = new List<ulong>();

            CreateSession();
            LoadChessImages();
        }

        [Command("szachy")]
        [Description("Gra w szachy.")]
        [Aliases("s", "c", "chess")]
        public async Task Chess(CommandContext ctx, [Description("`new` aby rozpocząć grę lub ruch (np. `e2e4`)")]string action = null)
        {
            if (action == null)
            {
                await ctx.RespondAsync("Użycie: `!szachy new` aby rozpocząć nową grę lub `!szachy e2e4` aby wykonać ruch.");
            }
            else if (action == "new")
            {
                _selectedPositions.Clear();
                _messageIds.Clear();

                CreateSession();

                var boardMessage = await ctx.RespondAsync(new DiscordMessageBuilder().WithContent("**Nowa gra utworzona:**").WithFile("board.png", GetBoardImage()));
                _messageIds.Add(boardMessage.Id);
            }
            else
            {
                _gameSession.UpdateRemainingTime(Color.White, 200);
                _gameSession.UpdateRemainingTime(Color.Black, 200);

                Position fromPosition, toPosition;
                try
                {
                    var from = action.Substring(0, 2);
                    var to = action.Substring(2, 2);

                    fromPosition = PositionConverter.ToPosition(from);
                    toPosition = PositionConverter.ToPosition(to);
                }
                catch (Exception e)
                {
                    await ctx.RespondAsync("Nieprawidłowy ruch");
                    return;
                }

                var moveValidationBitboard = new Bitboard(_gameSession.Bitboard);
                moveValidationBitboard.Calculate(GeneratorMode.CalculateAttacks | GeneratorMode.CalculateMoves, false);

                var move = moveValidationBitboard.Moves.FirstOrDefault(p => p.From == fromPosition && p.To == toPosition);
                if (move == null)
                {
                    await ctx.RespondAsync("Nieprawidłowy ruch");
                    return;
                }

                var validationBitboardAfterMove = moveValidationBitboard.Move(move);
                validationBitboardAfterMove.Calculate(false);

                if (validationBitboardAfterMove.IsCheck(Color.White))
                {
                    await ctx.RespondAsync("Invalid move");
                    return;
                }


                foreach (var msgIdToDelete in _messageIds)
                {
                    var messageToDelete = await ctx.Channel.GetMessageAsync(msgIdToDelete);
                    await ctx.Channel.DeleteMessageAsync(messageToDelete);
                }

                _messageIds.Clear();

                _gameSession.Move(Color.White, fromPosition, toPosition);

                _selectedPositions.Clear();
                _selectedPositions.Add(fromPosition);
                _selectedPositions.Add(toPosition);

                var playerBoardMsg = await ctx.RespondAsync(new DiscordMessageBuilder().WithContent("**Ruch gracza:**").WithFile("board.png", GetBoardImage()));
                _messageIds.Add(playerBoardMsg.Id);

                var thinkingMessage = await ctx.RespondAsync("Myślę...");
                var aiMove = _gameSession.MoveAI(Color.Black);

                _selectedPositions.Clear();
                _selectedPositions.Add(aiMove.PVNodes[0].From);
                _selectedPositions.Add(aiMove.PVNodes[0].To);

                await thinkingMessage.DeleteAsync();

                var aiBoardMsg = await ctx.RespondAsync(new DiscordMessageBuilder().WithContent("**Ruch AI:**").WithFile("board.png", GetBoardImage()));
                _messageIds.Add(aiBoardMsg.Id);
            }

        }

        private void LoadChessImages()
        {
            var imagesList = Directory.GetFiles(ChessImagesPath);
            foreach (var imagePath in imagesList)
            {
                var pureName = imagePath.Split('/').Last().Split('.').First();
                _images.Add(pureName, Image.Load(imagePath));
            }
        }

        private void CreateSession()
        {
            if (_gameSession != null)
            {
                _gameSession.OnThinkingOutput -= GameSession_OnThinkingOutput;
            }

            _gameSession = new GameSession(1);
            _gameSession.OnThinkingOutput += GameSession_OnThinkingOutput;
        }

        private void GameSession_OnThinkingOutput(object sender, Proxima.Core.AI.ThinkingOutputEventArgs e)
        {
            Bot.DiscordClient.Logger.LogInformation($"{e.AIResult.Depth}: TN:{e.AIResult.Stats.TotalNodes} NPS:{e.AIResult.NodesPerSecond}");
        }

        private Stream GetBoardImage()
        {
            var board = new Image<Rgba32>(527, 535);

            var odd = false;
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    var field = odd ? _images["Field1"] : _images["Field2"];
                    board.Mutate(p => p.DrawImage(field, PixelBlenderMode.Overlay, 1, new SixLabors.Primitives.Point(15 + x * 64, y * 64)));

                    odd = !odd;
                }

                odd = !odd;
            }

            for (var x = 1; x <= 8; x++)
            {
                board.Mutate(p => p.DrawText(((char)(x + 'a' - 1)).ToString(), SystemFonts.CreateFont("Liberation Mono", 20, FontStyle.Bold), new Rgba32(255, 255, 255), new SixLabors.Primitives.PointF(x * 64 - 25, 513)));
                board.Mutate(p => p.DrawText((8 - x + 1).ToString(), SystemFonts.CreateFont("Liberation Mono", 20, FontStyle.Bold), new Rgba32(255, 255, 255), new SixLabors.Primitives.PointF(0, (x - 1) * 64 + 20)));
            }

            var friendlyBoard = new FriendlyBoard(_gameSession.Bitboard);
            foreach (var piece in friendlyBoard.Pieces)
            {
                var image = _images[GetImageNameByPiece(piece.Type, piece.Color)];
                board.Mutate(p => p.DrawImage(image, 1, new SixLabors.Primitives.Point(15 + (piece.Position.X - 1) * 64, (8 - piece.Position.Y) * 64)));
            }

            foreach (var selected in _selectedPositions)
            {
                board.Mutate(p => p.DrawImage(_images["InternalSelection"], 1, new SixLabors.Primitives.Point(15 + (selected.X - 1) * 64, (8 - selected.Y) * 64)));
            }

            var stream = new MemoryStream();
            board.Save(stream, new PngEncoder());
            stream.Position = 0;

            return stream;
        }

        private string GetImageNameByPiece(PieceType type, Color color)
        {
            return color.ToString() + type.ToString();
        }
    }
}