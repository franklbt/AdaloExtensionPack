using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using SkiaSharp;

namespace AdaloExtensionPack.Core.Fonts
{
    public class FontsService : IFontsService
    {
        public byte[] RenderText(string text, SKTypeface font, string color, int size)
        {
            var parsed = SKColor.TryParse(color, out var skColor);
            var paint = new SKPaint
            {
                TextSize = size,
                Typeface = font,
                Color = parsed ? skColor : SKColors.Black
            };

            var fontMetrics = paint.FontMetrics;
            var height = fontMetrics.Descent - fontMetrics.Ascent;
            var skBitmap = new SKBitmap(
                Convert.ToInt32(Math.Round(paint.MeasureText(text))), 
                Convert.ToInt32(Math.Round(height)));
            var canvas = new SKCanvas(skBitmap);
            canvas.DrawText(text, 0, size - fontMetrics.Descent, paint);

            using var memStream = new MemoryStream();
            skBitmap.Encode(memStream, SKEncodedImageFormat.Png, 100);
            return memStream.ToArray();
        }
    }
}