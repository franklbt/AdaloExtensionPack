using SkiaSharp;

namespace AdaloExtensionPack.Core.Fonts;

public interface IFontsService
{
    byte[] RenderText(string text, SKTypeface font, string color, int size);
}