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

namespace SKNIBot.Core.Commands.GameCommands
{
    [CommandsGroup("Gry")]
    public class ChessCommand
    {
        private Dictionary<string, Image> _images;

        private const string ChessImagesPath = "Images/Chess/";

        public ChessCommand()
        {
            _images = new Dictionary<string, Image>();

            LoadChessImages();
        }

        [Command("szachy")]
        [Description("Dev.")]
        [Aliases("chess", "c")]
        public async Task Chess(CommandContext ctx)
        {
            var f1 = Image.FromFile("Images/Chess/field1.png");
            var f2 = Image.FromFile("Images/Chess/field2.png");

            var board = new Bitmap(512, 512);
            var graphic = Graphics.FromImage(board);

            var odd = false;
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    var field = odd ? f1 : f2;
                    graphic.DrawImage(field, new Point(x * 64, y * 64));

                    odd = !odd;
                }

                odd = !odd;
            }

            var stream = new MemoryStream();
            board.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            await ctx.RespondWithFileAsync(stream, "test.png");
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
    }
}
