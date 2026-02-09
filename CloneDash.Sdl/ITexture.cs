namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3/SDL_Texture" />.</summary>
public interface ITexture : ISdlWrapper<IntPtr>
{
    /// <value>The texture's palette.</value>
    public IPalette Palette { get; set; }
}
