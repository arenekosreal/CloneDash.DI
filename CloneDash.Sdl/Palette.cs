using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Palette : IPalette
{
    public IntPtr SdlPtr { get; }

    internal Palette(IntPtr existing) => SdlPtr = existing;

    public void Dispose() => SDL.DestroyPalette(SdlPtr);
}
