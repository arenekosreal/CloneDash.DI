using System.Drawing;
using System.Numerics;

using PathLib;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Surface : ISurface
{
    public IntPtr SdlPtr { get; }

    public IPalette Palette
    {
        get => new Palette(SDL.GetSurfacePalette(SdlPtr));
        set => SDL.SetSurfacePalette(SdlPtr, value.SdlPtr);
    }

    public Color this[Vector2 position]
    {
        set => SDL.WriteSurfacePixel(SdlPtr,
                                     System.Convert.ToInt32(position.X), System.Convert.ToInt32(position.Y),
                                     value.R, value.G, value.B, value.A);
    }

    public static Surface FromBMP(IPath bmpFile) => new(SDL.LoadBMP(bmpFile.ToString()!));

    internal Surface(IntPtr existing) => SdlPtr = existing;

    public bool Blit(ISurface to, Rectangle? source = null, Rectangle? destination = null)
    {
        if (source is not null && destination is not null)
            return SDL.BlitSurface(SdlPtr, source.Value.ToSdlRect(), to.SdlPtr, destination.Value.ToSdlRect());
        else if (source is not null)
            return SDL.BlitSurface(SdlPtr, source.Value.ToSdlRect(), to.SdlPtr, IntPtr.Zero);
        else if (destination is not null)
            return SDL.BlitSurface(SdlPtr, IntPtr.Zero, to.SdlPtr, destination.Value.ToSdlRect());
        else
            return SDL.BlitSurface(SdlPtr, IntPtr.Zero, to.SdlPtr, IntPtr.Zero);
    }

    public bool Clear(Color color) => SDL.ClearSurface(SdlPtr, color.R, color.G, color.B, color.A);

    public ISurface Convert(SDL.PixelFormat format) => new Surface(SDL.ConvertSurface(SdlPtr, format));

    public ISurface Rotate(float degree) => new Surface(SDL.RotateSurface(SdlPtr, degree));

    public bool Write(Vector2 position, Color color) => SDL.WriteSurfacePixel(SdlPtr, System.Convert.ToInt32(position.X), System.Convert.ToInt32(position.Y), color.R, color.G, color.B, color.A);

    public void Dispose() => SDL.DestroySurface(SdlPtr);
}
