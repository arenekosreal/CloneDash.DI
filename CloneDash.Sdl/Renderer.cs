using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Renderer : IRenderer
{
    public IntPtr SdlPtr { get; internal init; }

    public string Name { get => SDL.GetRendererName(SdlPtr) ?? string.Empty; }

    public Color DrawColor
    {
        get => SDL.GetRenderDrawColor(SdlPtr, out byte r, out byte g, out byte b, out byte a) ? Color.FromArgb(a, r, g, b) : default;
        set => SDL.SetRenderDrawColor(SdlPtr, value.R, value.G, value.B, value.A);
    }

    public Size OutputSize
    {
        get => SDL.GetRenderOutputSize(SdlPtr, out int w, out int h) ? new(w, h) : default;
    }

    public Rectangle SafeArea
    {
        get => SDL.GetRenderSafeArea(SdlPtr, out SDL.Rect rect) ? new(rect.X, rect.Y, rect.W, rect.H) : default;
    }

    public IWindow Window { get => new Window() { SdlPtr = SDL.GetRenderWindow(SdlPtr) }; }

    public ITexture Target
    {
        get => new Texture() { SdlPtr = SDL.GetRenderTarget(SdlPtr) };
        set => SDL.SetRenderTarget(SdlPtr, value.SdlPtr);
    }

    public ValueTask<bool> ClearAsync() => ValueTask.FromResult(SDL.RenderClear(SdlPtr));

    public ValueTask<bool> PresentAsync() => ValueTask.FromResult(SDL.RenderPresent(SdlPtr));

    public ValueTask<bool> RenderDebugTextAsync(Vector2 position, string text) =>
        ValueTask.FromResult(SDL.RenderDebugText(SdlPtr, position.X, position.Y, text));

    public ValueTask<bool> RenderLinesAsync(params IEnumerable<Vector2> points)
    {
        SDL.FPoint[] pointsToSdl = points.Select(Vector2Extensions.ToSdlFPoint).ToArray();
        return ValueTask.FromResult(SDL.RenderLines(SdlPtr, pointsToSdl, pointsToSdl.Length));
    }

    public ValueTask<bool> RenderPointsAsync(params IEnumerable<Vector2> points)
    {
        SDL.FPoint[] pointsToSdl = points.Select(Vector2Extensions.ToSdlFPoint).ToArray();
        return ValueTask.FromResult(SDL.RenderPoints(SdlPtr, pointsToSdl, pointsToSdl.Length));
    }

    public ValueTask<bool> RenderRectanglesAsync(params IEnumerable<Rectangle> rectangles)
    {
        SDL.FRect[] rectanglesToSdl = rectangles.Select(RectangleExtensions.ToSdlFRect).ToArray();
        return ValueTask.FromResult(SDL.RenderRects(SdlPtr, rectanglesToSdl, rectanglesToSdl.Length));
    }

    public ValueTask<bool> RenderTextureAsync(ITexture texture, Rectangle? source = null, Rectangle? destination = null)
    {
        if (source is not null && destination is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect(), dst = destination.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTexture(SdlPtr, texture.SdlPtr, in src, in dst));
        }
        else if (source is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTexture(SdlPtr, texture.SdlPtr, in src, IntPtr.Zero));
        }
        else if (destination is not null)
        {
            SDL.FRect dst = destination.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTexture(SdlPtr, texture.SdlPtr, IntPtr.Zero, in dst));
        }
        else
            return ValueTask.FromResult(SDL.RenderTexture(SdlPtr, texture.SdlPtr, IntPtr.Zero, IntPtr.Zero));
    }

    public ValueTask<bool> RenderTextureAsync(ITexture texture, double rotationAngle = 0, Vector2? rotationCenter = null, SDL.FlipMode flipMode = SDL.FlipMode.None, Rectangle? source = null, Rectangle? destination = null)
    {
        if (rotationCenter is not null && source is not null && destination is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect(), dst = destination.Value.ToSdlFRect();
            SDL.FPoint center = rotationCenter.Value.ToSdlFPoint();
            return ValueTask.FromResult(SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr, in src, in dst, rotationAngle, in center, flipMode));
        }
        else if (rotationCenter is not null && source is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect();
            SDL.FPoint center = rotationCenter.Value.ToSdlFPoint();
            return ValueTask.FromResult(SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr, in src, IntPtr.Zero, rotationAngle, in center, flipMode));
        }
        else if (rotationCenter is not null && destination is not null)
        {
            SDL.FRect dst = destination.Value.ToSdlFRect();
            SDL.FPoint center = rotationCenter.Value.ToSdlFPoint();
            return ValueTask.FromResult(SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr, IntPtr.Zero, in dst, rotationAngle, in center, flipMode));
        }
        else if (source is not null && destination is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect(), dst = destination.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr, in src, in dst, rotationAngle, IntPtr.Zero, flipMode));
        }
        else if (source is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr, in src, IntPtr.Zero, rotationAngle, IntPtr.Zero, flipMode));
        }
        else
            return ValueTask.FromResult(SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr, IntPtr.Zero, IntPtr.Zero, rotationAngle, IntPtr.Zero, flipMode));
    }

    public ValueTask<bool> RenderTextureAsync(ITexture texture, float scaled = 1, Rectangle? source = null, Rectangle? destination = null)
    {
        if (source is not null && destination is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect(), dst = destination.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr, in src, scaled, in dst));
        }
        else if (source is not null)
        {
            SDL.FRect src = source.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr, in src, scaled, IntPtr.Zero));
        }
        else if (destination is not null)
        {
            SDL.FRect dst = destination.Value.ToSdlFRect();
            return ValueTask.FromResult(SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr, IntPtr.Zero, scaled, in dst));
        }
        else
            return ValueTask.FromResult(SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr, IntPtr.Zero, scaled, IntPtr.Zero));
    }

    public ValueTask DisposeAsync()
    {
        SDL.DestroyRenderer(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
