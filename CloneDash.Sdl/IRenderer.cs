using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3/SDL_Renderer" />.</summary>
public interface IRenderer : ISdlWrapper<IntPtr>
{
    /// <value>The renderer's name.</value>
    public string Name { get; }

    /// <value>The color used for drawing operations.</value>
    public Color DrawColor { get; set; }

    /// <value>The renderer's output size in pixels.</value>
    public Size OutputSize { get; }

    /// <value>The renderer's safe area.</value>
    public Rectangle SafeArea { get; }

    /// <value>The renderer's window.</value>
    public IWindow Window { get; }

    /// <value>The renderer's target.</value>
    public ITexture Target { get; set; }

    /// <summary>Clear the renderer with drawing color.</summary>
    public ValueTask<bool> ClearAsync();

    /// <summary>Update the screen with any rendering performed since previous call.</summary>
    public ValueTask<bool> PresentAsync();

    /// <summary>Render a debug text.</summary>
    public ValueTask<bool> RenderDebugTextAsync(Vector2 position, string text);

    /// <summary>Render a set of lines.</summary>
    /// <example><c>RenderLinesAsync(p1, p2, p3)</c>
    /// will create lines from p1 -> p2, p2 -> p3.</example>
    public ValueTask<bool> RenderLinesAsync(params IEnumerable<Vector2> points);

    /// <summary>Render a set of points.</summary>
    public ValueTask<bool> RenderPointsAsync(params IEnumerable<Vector2> points);

    /// <summary>Render a set of rectangles.</summary>
    public ValueTask<bool> RenderRectanglesAsync(params IEnumerable<Rectangle> rectangles);

    /// <summary>Render a texture.</summary>
    /// <param name="destination">The rectangle selects which parts of rendering target to render, <c>null</c> for the entire rendering target.</param>
    /// <param name="source">The rectangle selects which parts of <paramref name="texture" /> to render, <c>null</c> for the entire <paramref name="texture" />.</param>
    /// <param name="texture">The texture to render.</param>
    public ValueTask<bool> RenderTextureAsync(ITexture texture, Rectangle? source = null, Rectangle? destination = null);

    /// <summary>Render a texture with rotation and flipping.</summary>
    /// <param name="destination">The rectangle selects which parts of rendering target to render, <c>null</c> for the entire rendering target.</param>
    /// <param name="source">The rectangle selects which parts of <paramref name="texture" /> to render, <c>null</c> for the entire <paramref name="texture" />.</param>
    /// <param name="texture">The texture to render.</param>
    /// <param name="flipMode">How to flip the texture.</param>
    /// <param name="rotationAngle">The angle in degree that indicates the rotation applied to <paramref name="destination" /> in clock wise direction.</param>
    /// <param name="rotationCenter">The point around which <paramref name="destination" /> will be rotated, <c>null</c> means rotation will be around w/2, h/2.</param>
    public ValueTask<bool> RenderTextureAsync(ITexture texture, double rotationAngle = 0, Vector2? rotationCenter = null, SDL.FlipMode flipMode = SDL.FlipMode.None, Rectangle? source = null, Rectangle? destination = null);

    /// <summary>Render a texture with being tiled.</summary>
    /// <param name="destination">The rectangle selects which parts of rendering target to render, <c>null</c> for the entire rendering target.</param>
    /// <param name="source">The rectangle selects which parts of <paramref name="texture" /> to render, <c>null</c> for the entire <paramref name="texture" />.</param>
    /// <param name="scale">The scale used to transform <paramref name="source" /> into <paramref name="destination" />.</param>
    /// <param name="texture">The texture to render.</param>
    public ValueTask<bool> RenderTextureAsync(ITexture texture, float scale = 1, Rectangle? source = null, Rectangle? destination = null);
}
