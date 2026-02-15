using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Texture : ITexture
{
    public IntPtr SdlPtr { get; }

    public IPalette Palette
    {
        get => new Palette(SDL.GetTexturePalette(SdlPtr));
        set => SDL.SetTexturePalette(SdlPtr, value.SdlPtr);
    }

    internal Texture(IntPtr existing) => SdlPtr = existing;

    public ValueTask DisposeAsync()
    {
        SDL.DestroyTexture(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
