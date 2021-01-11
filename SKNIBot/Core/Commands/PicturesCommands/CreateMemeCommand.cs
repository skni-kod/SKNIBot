using System;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.ImageSharp.Processing.Drawing.Pens;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    class CreateMemeCommand : BaseCommandModule
    {
        private readonly Font _dummyFont;

        private const int MinFontSize = 15;
        private const int MaxFontSize = 70;

        private const int HorizontalSpacing = 8;
        private const int VerticalSpacing = 10;
        private const float ScreenHeightUpPercent = 0.4f;
        private const int OutlineSize = 5;

        private readonly Pen<Rgba32> _outlinePen;

        private TextGraphicsOptions _textOptions;

        public CreateMemeCommand()
        {
            FontCollection collection = new FontCollection();
            FontFamily family = collection.Install("Fonts/liberation-mono/LiberationMono-Regular.ttf");
            _dummyFont = family.CreateFont(20);

            _textOptions = new TextGraphicsOptions()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            _outlinePen = Pens.Solid(Rgba32.Black, OutlineSize);
        }

        [Command("meme")]
        [Aliases("mem")]
        [Description("Stwórz mem z podanym obrazkiem")]
        public async Task CreateMeme(CommandContext ctx, string picName, string upText, string downText = null)
        {
            string pictureLink;
            using (var databaseContext = new StaticDBContext())
            {
                await ctx.TriggerTypingAsync();

                pictureLink = databaseContext.Media
                    .Where(vid => vid.Command.Name == "Picture" && vid.Names.Any(p => p.Name == picName))
                    .Select(p => p.Link)
                    .FirstOrDefault();

                if (pictureLink == null)
                {
                    await ctx.RespondAsync("Nieznany parametr, wpisz !pic list aby uzyskać listę dostępnych.");
                    return;
                }
            }

            MemoryStream mem = new MemoryStream();

            using (Image<Rgba32> img = ImageHelper.DownloadImage(pictureLink))
            {
                DrawTextOnImage(img, upText, downText);
                img.SaveAsJpeg(mem);
            }

            mem.Position = 0;

            await ctx.RespondWithFileAsync("MEMEM.jpg", mem);
        }

        [Command("memeUrl")]
        [Aliases("memLink")]
        [Description("Stwórz mem z podanym obrazkiem")]
        public async Task CreateMemeWithUrl(CommandContext ctx, string picUrl, string upText, string downText = null)
        {
            await ctx.TriggerTypingAsync();

            MemoryStream mem = new MemoryStream();

            using (Image<Rgba32> img = ImageHelper.DownloadImage(picUrl))
            {
                DrawTextOnImage(img, upText, downText);

                img.SaveAsJpeg(mem);
            }

            mem.Position = 0;

            await ctx.RespondWithFileAsync("MEMEM.jpg", mem);
        }

        void DrawTextOnImage(Image<Rgba32> img, string upText, string downText)
        {
            _textOptions.WrapTextWidth = img.Width - HorizontalSpacing * 2;

            (int fontSize, RectangleF sizeRect) = GetAdjustedFont(upText, _textOptions.WrapTextWidth, img.Height * ScreenHeightUpPercent, MaxFontSize, MinFontSize);
            Font font = new Font(_dummyFont, fontSize);

            var startPoint = new PointF(HorizontalSpacing, VerticalSpacing);
            DrawTextOnPosition(img, upText, font, startPoint);

            if (downText != null)
            {
                (fontSize, sizeRect) = GetAdjustedFont(downText, _textOptions.WrapTextWidth,
                    img.Height * ScreenHeightUpPercent, MaxFontSize, MinFontSize);
                Font fontDown = new Font(_dummyFont, fontSize);

                float y = img.Height - sizeRect.Height;
                startPoint = new PointF(HorizontalSpacing, y);

                DrawTextOnPosition(img, downText, fontDown, startPoint);
            }
        }

        void DrawTextOnPosition(Image<Rgba32> img, string text, Font font, PointF startPoint)
        {
            img.Mutate(c => c
                .DrawText(_textOptions, text.ToUpper(), font, _outlinePen, startPoint)
                .DrawText(_textOptions, text.ToUpper(), font, Rgba32.White, startPoint));
        }

        public (int, RectangleF) GetAdjustedFont(string graphicString, float containerWidth, float containerHeight, int maxFontSize,
            int minFontSize)
        {
            var sizeRect = new RectangleF();
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                var testFont = new Font(_dummyFont, adjustedSize);
                var renderOptions = new RendererOptions(testFont)
                {
                    WrappingWidth = containerWidth
                };

                // Test the string with the new size
                sizeRect = TextMeasurer.MeasureBounds(graphicString, renderOptions);
                float volume = sizeRect.Height * sizeRect.Width;

                if (volume < containerWidth * containerHeight)
                {
                    // Good font, return it
                    return (adjustedSize, sizeRect);
                }
            }
            
            return (minFontSize, sizeRect);
        }
    }
}