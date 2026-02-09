using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Texture : ITexture
{
    public IntPtr SdlPtr { get; internal init; }

    public IPalette Palette
    {
        get => new Palette() { SdlPtr = SDL.GetTexturePalette(SdlPtr) };
        set => SDL.SetTexturePalette(SdlPtr, value.SdlPtr);
    }

    public ValueTask DisposeAsync()
    {
        SDL.DestroyTexture(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
