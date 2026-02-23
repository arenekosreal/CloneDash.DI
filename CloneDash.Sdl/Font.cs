using System.Drawing;

using PathLib;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Font : IFont
{
    public IntPtr SdlPtr { get; }

    public IPath? Path { get; }

    public TTF.Direction Direction { get => TTF.GetFontDirection(SdlPtr); set => TTF.SetFontDirection(SdlPtr, value); }

    public TTF.HorizontalAlignment WrapAlignment { get => TTF.GetFontWrapAlignment(SdlPtr); set => TTF.SetFontWrapAlignment(SdlPtr, value); }

    public string FamilyName { get => TTF.GetFontFamilyName(SdlPtr); }

    public int Size { get => checked((int)TTF.GetFontSize(SdlPtr)); set => TTF.SetFontSize(SdlPtr, checked(value)); }

    public TTF.FontStyleFlags Style { get => TTF.GetFontStyle(SdlPtr); set => TTF.SetFontStyle(SdlPtr, value); }

    public TTF.HintingFlags Hinting { get => TTF.GetFontHinting(SdlPtr); set => TTF.SetFontHinting(SdlPtr, value); }

    public Font(IPath fontPath, int fontSize) =>
        (SdlPtr, Path) = (TTF.OpenFont(fontPath.ToString()!, fontSize), fontPath);

    public Font(Stream stream, int fontSize, IPath? fontPath = null)
    {
        SDL.IOStreamOwner ioStreamOwner = SDL.IOFromStream(stream);
        (SdlPtr, Path) = (TTF.OpenFontIO(ioStreamOwner.Handle, true, fontSize), fontPath);
    }

    internal Font(IPath? fontPath, IntPtr existing) =>
        (SdlPtr, Path) = (existing, fontPath);

    public void Dispose() => TTF.CloseFont(SdlPtr);

    public bool AddFallbackFont(IFont font) => TTF.AddFallbackFont(SdlPtr, font.SdlPtr);

    public IFont Copy() => new Font(Path, TTF.CopyFont(SdlPtr));

    public void RemoveFallbackFont(IFont font) => TTF.RemoveFallbackFont(SdlPtr, font.SdlPtr);

    public bool GetStringSize(string text, out Size size)
    {
        bool ret = TTF.GetStringSize(SdlPtr, text, 0, out int width, out int height);
        size = new(width, height);
        return ret;
    }

    public bool GetStringSize(string text, int wrapWidth, out Size size)
    {
        bool ret = TTF.GetStringSizeWrapped(SdlPtr, text, 0, wrapWidth, out int width, out int height);
        size = new(width, height);
        return ret;
    }
}
