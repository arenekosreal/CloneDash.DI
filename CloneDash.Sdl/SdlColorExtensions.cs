using System.Drawing;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Extended methods for <see cref="SDL.Color" />.</summary>
public static class SdlColorExtensions
{
    /// <summary>Convert <paramref name="color" /> to <see cref="Color" />.</summary>
    public static Color ToStdColor(this SDL.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);
}
