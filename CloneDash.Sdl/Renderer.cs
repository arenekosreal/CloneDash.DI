using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Renderer : IRenderer
{
    public IntPtr SdlPtr { get; }

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

    public IWindow Window { get => new Window(SDL.GetRenderWindow(SdlPtr)); }

    public ITexture Target
    {
        get => new Texture(SDL.GetRenderTarget(SdlPtr));
        set => SDL.SetRenderTarget(SdlPtr, value.SdlPtr);
    }

    public Renderer(IWindow window, string? name = null) =>
        SdlPtr = SDL.CreateRenderer(window.SdlPtr, name);

    internal Renderer(IntPtr existing) => SdlPtr = existing;

    public bool Clear() => SDL.RenderClear(SdlPtr);

    public bool Present() => SDL.RenderPresent(SdlPtr);

    public bool RenderDebugText(Vector2 position, string text) =>
        SDL.RenderDebugText(SdlPtr, position.X, position.Y, text);

    public bool RenderLines(params IEnumerable<Vector2> points)
    {
        SDL.FPoint[] pointsToSdl = points.Select(Vector2Extensions.ToSdlFPoint).ToArray();
        return SDL.RenderLines(SdlPtr, pointsToSdl, pointsToSdl.Length);
    }

    public bool RenderPoints(params IEnumerable<Vector2> points)
    {
        SDL.FPoint[] pointsToSdl = points.Select(Vector2Extensions.ToSdlFPoint).ToArray();
        return SDL.RenderPoints(SdlPtr, pointsToSdl, pointsToSdl.Length);
    }

    public bool RenderRectangles(params IEnumerable<Rectangle> rectangles)
    {
        SDL.FRect[] rectanglesToSdl = rectangles.Select(RectangleExtensions.ToSdlFRect).ToArray();
        return SDL.RenderRects(SdlPtr, rectanglesToSdl, rectanglesToSdl.Length);
    }

    public bool RenderTexture(ITexture texture, Rectangle? source = null, Rectangle? destination = null)
    {
        if (source is not null && destination is not null)
            return SDL.RenderTexture(SdlPtr, texture.SdlPtr, source.Value.ToSdlFRect(), destination.Value.ToSdlFRect());
        else if (source is not null)
            return SDL.RenderTexture(SdlPtr, texture.SdlPtr, source.Value.ToSdlFRect(), IntPtr.Zero);
        else if (destination is not null)
            return SDL.RenderTexture(SdlPtr, texture.SdlPtr, IntPtr.Zero, destination.Value.ToSdlFRect());
        else
            return SDL.RenderTexture(SdlPtr, texture.SdlPtr, IntPtr.Zero, IntPtr.Zero);
    }

    public bool RenderTexture(ITexture texture, double rotationAngle = 0, Vector2? rotationCenter = null, SDL.FlipMode flipMode = SDL.FlipMode.None, Rectangle? source = null, Rectangle? destination = null)
    {
        if (rotationCenter is not null && source is not null && destination is not null)
            return SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr,
                                            source.Value.ToSdlFRect(), destination.Value.ToSdlFRect(),
                                            rotationAngle, rotationCenter.Value.ToSdlFPoint(),
                                            flipMode);
        else if (rotationCenter is not null && source is not null)
            return SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr,
                                            source.Value.ToSdlFRect(), IntPtr.Zero,
                                            rotationAngle, rotationCenter.Value.ToSdlFPoint(),
                                            flipMode);
        else if (rotationCenter is not null && destination is not null)
            return SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr,
                                            IntPtr.Zero, destination.Value.ToSdlFRect(),
                                            rotationAngle, rotationCenter.Value.ToSdlFPoint(),
                                            flipMode);
        else if (source is not null && destination is not null)
            return SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr,
                                            source.Value.ToSdlFRect(), destination.Value.ToSdlFRect(),
                                            rotationAngle, IntPtr.Zero,
                                            flipMode);
        else if (source is not null)
            return SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr,
                                            source.Value.ToSdlFRect(), IntPtr.Zero,
                                            rotationAngle, IntPtr.Zero,
                                            flipMode);
        else
            return SDL.RenderTextureRotated(SdlPtr, texture.SdlPtr,
                                            IntPtr.Zero, IntPtr.Zero,
                                            rotationAngle, IntPtr.Zero,
                                            flipMode);
    }

    public bool RenderTexture(ITexture texture, float scaled = 1, Rectangle? source = null, Rectangle? destination = null)
    {
        if (source is not null && destination is not null)
            return SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr,
                                          source.Value.ToSdlFRect(), scaled, destination.Value.ToSdlFRect());
        else if (source is not null)
            return SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr,
                                          source.Value.ToSdlFRect(), scaled, IntPtr.Zero);
        else if (destination is not null)
            return SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr,
                                          IntPtr.Zero, scaled, destination.Value.ToSdlFRect());
        else
            return SDL.RenderTextureTiled(SdlPtr, texture.SdlPtr,
                                          IntPtr.Zero, scaled, IntPtr.Zero);
    }

    public void Dispose() => SDL.DestroyRenderer(SdlPtr);
}
