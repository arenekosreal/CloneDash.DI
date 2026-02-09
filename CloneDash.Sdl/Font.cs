using PathLib;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Font : IFont
{
    public IntPtr SdlPtr { get; internal init; }

    public IPath? Path { get; internal init; }

    public TTF.Direction Direction { get => TTF.GetFontDirection(SdlPtr); set => TTF.SetFontDirection(SdlPtr, value); }

    public TTF.HorizontalAlignment WrapAlignment { get => TTF.GetFontWrapAlignment(SdlPtr); set => TTF.SetFontWrapAlignment(SdlPtr, value); }

    public string FamilyName { get => TTF.GetFontFamilyName(SdlPtr); }

    public int Size { get => checked((int)TTF.GetFontSize(SdlPtr)); set => TTF.SetFontSize(SdlPtr, checked(value)); }

    public TTF.FontStyleFlags Style { get => TTF.GetFontStyle(SdlPtr); set => TTF.SetFontStyle(SdlPtr, value); }

    public TTF.HintingFlags Hinting { get => TTF.GetFontHinting(SdlPtr); set => TTF.SetFontHinting(SdlPtr, value); }

    public Font(IPath fontPath, int fontSize) =>
        (SdlPtr, Path) = (TTF.OpenFont(fontPath.ToString()!, fontSize), fontPath);

    public ValueTask DisposeAsync()
    {
        TTF.CloseFont(SdlPtr);
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> AddFallbackFontAsync(IFont font) =>
        ValueTask.FromResult(TTF.AddFallbackFont(SdlPtr, font.SdlPtr));

    public ValueTask RemoveFallbackFontAsync(IFont font)
    {
        TTF.RemoveFallbackFont(SdlPtr, font.SdlPtr);
        return ValueTask.CompletedTask;
    }
}
