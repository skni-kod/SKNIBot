using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Processing.Text;
using DSharpPlus;
using SixLabors.Shapes;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Drawing.Pens;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    class CreateMemeCommand : BaseCommandModule
    {

        //FontFamily _fontFamily;
        //Pen _outlinePen;
        Font _font;

        int _minFontSize = 15;
        int _maxFontSize = 70;

        int _horizontalSpacing = 8;
        int _verticalSpacing = 10;
        float _screenHeightUpPercent = 0.4f;
        int _outlineSize = 5;

        public CreateMemeCommand()
        {
            //_fontFamily = new FontFamily("Liberation Mono");
            //_outlinePen = new Pen(Color.Black)
            //{
            //    Width = 4
            //};
            _font = SystemFonts.CreateFont("Liberation Mono", 20);
        }

        [Command("meme")]
        [Aliases("mem")]
        [Description("Stwórz mem z podanym obrazkiem")]
        public async Task CreateMeme(CommandContext ctx, string picName, string upText, string downText = null)
        {
            using (var databaseContext = new StaticDBContext())
            {
                await ctx.TriggerTypingAsync();

                var pictureLink = databaseContext.Media
                   .Where(vid => vid.Command.Name == "Picture" && vid.Names.Any(p => p.Name == picName))
                   .Select(p => p.Link)
                   .FirstOrDefault();

                if (pictureLink == null)
                {
                    await ctx.RespondAsync("Nieznany parametr, wpisz !pic list aby uzyskać listę dostępnych.");
                    return;
                }

                //Download picture
                WebClient web = new WebClient();
                var picture = web.DownloadData(pictureLink);
                var stream = new MemoryStream(picture);

                MemoryStream mem = new MemoryStream();

                using (Image<Rgba32> img = Image.Load(stream))
                {


                    TextGraphicsOptions options = new TextGraphicsOptions(true)
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        WrapTextWidth = img.Width - _horizontalSpacing * 2
                    };

                    var pen = Pens.Solid(Rgba32.Black, _outlineSize);
                    int fontSize = GetAdjustedFont(upText, (int)options.WrapTextWidth, (int)(img.Height * _screenHeightUpPercent), _maxFontSize, _minFontSize);
                    Font font = new Font(_font, fontSize);

                    img.Mutate(c => c
                        .DrawText(options, upText.ToUpper(), font, pen, new PointF(_horizontalSpacing, _verticalSpacing))
                        .DrawText(options, upText.ToUpper(), font, Rgba32.White, new PointF(_horizontalSpacing, _verticalSpacing)));

                    if(downText != null)
                    {

                        int fontSizeDown = GetAdjustedFont(downText, (int)options.WrapTextWidth, (int)(img.Height * _screenHeightUpPercent), _maxFontSize, _minFontSize);
                        Font fontDown = new Font(_font, fontSize);

                        float y = (1 - _screenHeightUpPercent) * img.Height;

                        img.Mutate(c => c
                            .DrawText(options, downText.ToUpper(), font, pen, new PointF(_horizontalSpacing, y))
                            .DrawText(options, downText.ToUpper(), font, Rgba32.White, new PointF(_horizontalSpacing, y)));
                    }

                    img.SaveAsJpeg(mem);
                }

                mem.Position = 0;

                await ctx.RespondWithFileAsync("MEMEM.jpg", mem);
            }
        }

        public int GetAdjustedFont(string graphicString, int containerWidth, int containerHeight, int maxFontSize, int minFontSize)
        {
            Font testFont = null;
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(_font, adjustedSize);

                // Test the string with the new size
                SizeF adjustedSizeNew = TextMeasurer.Measure(graphicString, new RendererOptions(testFont));
                float volume = adjustedSizeNew.Height * adjustedSizeNew.Width;

                if (volume < containerWidth * containerHeight)
                {
                    // Good font, return it
                    return adjustedSize;
                }
            }

            return minFontSize;
        }
    }
}
