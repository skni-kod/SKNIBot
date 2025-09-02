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
using SixLabors.ImageSharp.Drawing.Processing;
using SKNIBot.Core.Helpers;
using DSharpPlus.Entities;

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

        private readonly Pen _outlinePen;

        public CreateMemeCommand()
        {
            FontCollection collection = new FontCollection();
            FontFamily family = collection.Add("Fonts/liberation-mono/LiberationMono-Regular.ttf");
            _dummyFont = family.CreateFont(20);

            _outlinePen = Pens.Solid(Color.Black, OutlineSize);
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
                    .Where(vid => vid.Command.Name == "Picture" && vid.IsDeleted == false && vid.Names.Any(p => p.Name == picName && p.IsDeleted == false))
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

            await ctx.RespondAsync(new DiscordMessageBuilder().AddFile("MEMEM.jpg", mem));
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

            await ctx.RespondAsync(new DiscordMessageBuilder().AddFile("MEMEM.jpg", mem));
        }

        void DrawTextOnImage(Image<Rgba32> img, string upText, string downText)
        {
            float wrapWidth = img.Width - HorizontalSpacing * 2;

            (int fontSize, RectangleF sizeRect) = GetAdjustedFont(upText, wrapWidth, img.Height * ScreenHeightUpPercent, MaxFontSize, MinFontSize);
            Font font = new Font(_dummyFont, fontSize);

            var startPoint = new PointF(img.Width / 2f, VerticalSpacing);  // center X, top padding

            DrawTextOnPosition(img, upText, font, startPoint);

            if (downText != null)
            {
                (fontSize, sizeRect) = GetAdjustedFont(downText, wrapWidth, img.Height * ScreenHeightUpPercent, MaxFontSize, MinFontSize);
                Font fontDown = new Font(_dummyFont, fontSize);

                float y = img.Height - sizeRect.Height - VerticalSpacing; // trochę odstępu od dołu
                startPoint = new PointF(img.Width / 2f, y);

                DrawTextOnPosition(img, downText, fontDown, startPoint);
            }
        }

        void DrawTextOnPosition(Image<Rgba32> img, string text, Font font, PointF startPoint)
        {
            var textOptions = new RichTextOptions(font)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Origin = startPoint
            };

            img.Mutate(ctx =>
            {
                int outlineThickness = 2;

                for (int x = -outlineThickness; x <= outlineThickness; x++)
                {
                    for (int y = -outlineThickness; y <= outlineThickness; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        var offsetOptions = new RichTextOptions(font)
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top,
                            Origin = new PointF(startPoint.X + x, startPoint.Y + y)
                        };

                        ctx.DrawText(offsetOptions, text.ToUpper(), Color.Black);
                    }
                }

                ctx.DrawText(textOptions, text.ToUpper(), Color.White);
            });
        }
        
        public (int, RectangleF) GetAdjustedFont(string graphicString, float containerWidth, float containerHeight, int maxFontSize, int minFontSize)
        {
            var sizeRect = new RectangleF();

            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                var testFont = new Font(_dummyFont, adjustedSize);

                var textOptions = new TextOptions(testFont)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Origin = new PointF(0, 0)
                };

                var bounds = TextMeasurer.MeasureBounds(graphicString, textOptions);
                sizeRect = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);

                float volume = sizeRect.Height * sizeRect.Width;

                if (volume < containerWidth * containerHeight)
                {
                    return (adjustedSize, sizeRect);
                }
            }
            return (minFontSize, sizeRect);
        }
    }
}
