namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine" />.</summary>
public interface ITextEngine : ISdlWrapper<IntPtr>
{
    /// <summary>Create a <see cref="IText" /> from utf-8 text.</summary>
    public IText CreateText(IFont font, string text);
}
