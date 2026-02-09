using System.Drawing;

using SDL3;

namespace CloneDash.Sdl;

internal static class RectangleExtensions
{
    public static SDL.FRect ToSdlFRect(this Rectangle rect) => new() { X = rect.X, Y = rect.Y, H = rect.Height, W = rect.Width };

    public static SDL.Rect ToSdlRect(this Rectangle rect) => new() { X = rect.X, Y = rect.Y, H = rect.Height, W = rect.Width };
}
