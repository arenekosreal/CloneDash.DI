using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Surface : ISurface
{
    public IntPtr SdlPtr { get; internal init; }

    public IPalette Palette
    {
        get => new Palette() { SdlPtr = SDL.GetSurfacePalette(SdlPtr) };
        set => SDL.SetSurfacePalette(SdlPtr, value.SdlPtr);
    }

    public ValueTask<bool> BlitAsync(ISurface to, Rectangle? source = null, Rectangle? destination = null)
    {
        if (source is not null && destination is not null)
        {
            SDL.Rect src = source.Value.ToSdlRect(), dst = destination.Value.ToSdlRect();
            return ValueTask.FromResult(SDL.BlitSurface(SdlPtr, in src, to.SdlPtr, in dst));
        }
        else if (source is not null)
        {
            SDL.Rect src = source.Value.ToSdlRect();
            return ValueTask.FromResult(SDL.BlitSurface(SdlPtr, in src, to.SdlPtr, IntPtr.Zero));
        }
        else if (destination is not null)
        {
            SDL.Rect dst = destination.Value.ToSdlRect();
            return ValueTask.FromResult(SDL.BlitSurface(SdlPtr, IntPtr.Zero, to.SdlPtr, in dst));
        }
        else
            return ValueTask.FromResult(SDL.BlitSurface(SdlPtr, IntPtr.Zero, to.SdlPtr, IntPtr.Zero));
    }

    public ValueTask<bool> ClearAsync(Color color) =>
        ValueTask.FromResult(SDL.ClearSurface(SdlPtr, color.R, color.G, color.B, color.A));

    public ValueTask<ISurface> Convert(SDL.PixelFormat format) =>
        ValueTask.FromResult<ISurface>(new Surface() { SdlPtr = SDL.ConvertSurface(SdlPtr, format) });

    public ValueTask<ISurface> Rotate(float degree) =>
        ValueTask.FromResult<ISurface>(new Surface() { SdlPtr = SDL.RotateSurface(SdlPtr, degree) });

    public ValueTask<bool> Write(Vector2 position, Color color) =>
        ValueTask.FromResult(SDL.WriteSurfacePixel(SdlPtr, System.Convert.ToInt32(position.X), System.Convert.ToInt32(position.Y), color.R, color.G, color.B, color.A));

    public ValueTask DisposeAsync()
    {
        SDL.DestroySurface(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
