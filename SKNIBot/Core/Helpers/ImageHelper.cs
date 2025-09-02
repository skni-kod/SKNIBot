using System;
using System.IO;
using System.Net;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SKNIBot.Core.Helpers
{
    public static class ImageHelper
    {
        public static Image<Rgba32> DownloadImage(string link)
        {
            bool result = Uri.TryCreate(link, UriKind.Absolute, out var uriResult) 
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                throw new UriFormatException("Given link is not viable URL");
            }

            return DownloadImage(uriResult);
        }

        public static Image<Rgba32> DownloadImage(Uri uri)
        {
            var web = new WebClient();
            var picture = web.DownloadData(uri);
            var stream = new MemoryStream(picture);

            return Image.Load<Rgba32>(stream);
        }
    }
}