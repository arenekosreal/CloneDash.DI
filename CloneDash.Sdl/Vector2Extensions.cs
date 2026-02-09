using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

internal static class Vector2Extensions
{
    public static SDL.FPoint ToSdlFPoint(this Vector2 point) => new() { X = point.X, Y = point.Y };
}
