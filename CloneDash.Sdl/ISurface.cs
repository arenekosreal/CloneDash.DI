using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3/SDL_Surface" />.</summary>
public interface ISurface : ISdlWrapper<IntPtr>
{
    /// <value>The surface's palette.</value>
    public IPalette Palette { get; set; }

    /// <summary>Blit the surface to the <paramref name="to" />.</summary>
    /// <param name="destination">The rectangle selects which parts of <paramref name="to" /> to blit.</param>
    /// <param name="to">The blit target.</param>
    /// <param name="source">The rectangle selects which parts to be blited.</param>
    public ValueTask<bool> BlitAsync(ISurface to, Rectangle? source = null, Rectangle? destination = null);

    /// <summary>Clear the surface with <paramref name="color" />.</summary>
    public ValueTask<bool> ClearAsync(Color color);

    /// <summary>Copy an existing surface to a new surface of the specified format.</summary>
    public ValueTask<ISurface> Convert(SDL.PixelFormat format);

    /// <summary>Return a copy of a surface rotated clockwise a number of degrees.</summary>
    public ValueTask<ISurface> Rotate(float degree);

    /// <summary>Write a single pixel to the surface.</summary>
    public ValueTask<bool> Write(Vector2 position, Color color);
}
