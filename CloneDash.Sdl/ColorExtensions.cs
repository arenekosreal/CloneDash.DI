using System.Drawing;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Extended methods for <see cref="Color" />.</summary>
public static class ColorExtensions
{
    /// <summary>Convert <paramref name="color" /> to <see cref="SDL.Color" />.</summary>
    public static SDL.Color ToSdlColor(this Color color) =>
        new() { A = color.A, R = color.R, G = color.G, B = color.B };
}
