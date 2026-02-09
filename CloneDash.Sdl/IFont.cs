using PathLib;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Font" />.</summary>
public interface IFont : ISdlWrapper<IntPtr>
{
    /// <value>The path of the font file.</value>
    /// <remarks><c>null</c> if there is no physical file.</remarks>
    public IPath? Path { get; }

    /// <value>The direction of the font.</value>
    public TTF.Direction Direction { get; set; }

    /// <value>The wrap alignment of the font.</value>
    public TTF.HorizontalAlignment WrapAlignment { get; set; }

    /// <value>The family name of the font.</value>
    public string FamilyName { get; }

    /// <value>The size of the font.</value>
    public int Size { get; set; }

    /// <value>The style of the font.</value>
    public TTF.FontStyleFlags Style { get; set; }

    /// <value>The hinting of the font.</value>
    public TTF.HintingFlags Hinting { get; set; }

    /// <summary>Add fonts to fallback list.</summary>
    public ValueTask<bool> AddFallbackFontAsync(IFont font);

    /// <summary>Remove fonts from fallback list.</summary>
    public ValueTask RemoveFallbackFontAsync(IFont font);
}
