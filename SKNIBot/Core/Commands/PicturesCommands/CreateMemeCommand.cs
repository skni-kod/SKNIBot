/*using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    [CommandsGroup("Obrazki")]
    class CreateMemeCommand
    {

        FontFamily _fontFamily;
        Pen _outlinePen;

        int _minFontSize = 15;
        int _maxFontSize = 70;

        int _horizontalSpacing = 8;
        float _screenHeightUpPercent = 0.4f;

        public CreateMemeCommand()
        {
            _fontFamily = new FontFamily("Liberation Mono");
            _outlinePen = new Pen(Color.Black)
            {
                Width = 4
            };
        }

        [Command("meme")]
        [Aliases("mem")]
        [Description("Stwórz mem z podanym obrazkiem")]
        public async Task CreateMeme(CommandContext ctx, string picName, string upText)
        {
            using (var databaseContext = new StaticDBContext())
            {
                await ctx.TriggerTypingAsync();

                var pictureLink = databaseContext.Media
                   .Where(vid => vid.Command.Name == "Picture" && vid.Names.Any(p => DbFunctions.Like(p.Name, picName)))
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

                //Create image and rect for it
                Image img = Bitmap.FromStream(stream);

                var upPosition = new RectangleF(0, 0, img.Width - _horizontalSpacing, img.Height * _screenHeightUpPercent);

                Graphics g = GetGraphicsFromImage(img);

                StringFormat format = new StringFormat(StringFormat.GenericDefault);
                format.Alignment = StringAlignment.Center;

                //g.DrawString(upText, _font, Brushes.Black, upPosition, format);

                await ctx.TriggerTypingAsync();

                //Create GraphicsPath, adjust font size and add string to path
                GraphicsPath path = new GraphicsPath();
                float fontSize = GetAdjustedFont(g, upText, _fontFamily.GetName(0), FontStyle.Bold, (int)upPosition.Width, (int)upPosition.Height, _maxFontSize, _minFontSize);
                path.AddString(upText.ToUpper(), _fontFamily, (int)FontStyle.Bold, fontSize, upPosition, format);

#if DEBUG
                await ctx.RespondAsync($"Width: {img.Width} Height: {img.Height} Size: {fontSize}");
#endif
                //Draw String
                g.DrawPath(_outlinePen, path);
                g.FillPath(Brushes.White, path);
                g.Flush();

                //Save image and upload it
                MemoryStream mem = new MemoryStream();
                img.Save(mem, ImageFormat.Jpeg);
                mem.Position = 0;

                await ctx.RespondWithFileAsync(mem, "MEMEM.jpg");
            }
        }

        public int GetAdjustedFont(Graphics graphicRef, string graphicString, string originalFontName, FontStyle style, int containerWidth, int containerHeight, int maxFontSize, int minFontSize)
        {
            Font testFont = null;      
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(originalFontName, adjustedSize, style);

                // Test the string with the new size
                SizeF adjustedSizeNew = graphicRef.MeasureString(graphicString, testFont);
                float volume = adjustedSizeNew.Height * adjustedSizeNew.Width;

                if(volume < containerWidth * containerHeight)
                {
                    // Good font, return it
                    return adjustedSize;
                }
            }

            return minFontSize;
        }

        Graphics GetGraphicsFromImage(Image img)
        {
            Graphics g = Graphics.FromImage(img);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            return g;
        }
    }
}
*/