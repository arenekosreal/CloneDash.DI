using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3/SDL_Window" />.</summary>
public interface IWindow : ISdlWrapper<IntPtr>
{
    /// <value>The content display scale relative to a window's pixel size.</value>
    public float DisplayScale { get; }

    /// <value>The window's flags.</value>
    public SDL.WindowFlags Flags { get; }

    /// <value>The window's icon.</value>
    public ISurface Icon { set; }

    /// <value>The window 's parent.</value>
    public IWindow? Parent { get; set; }

    /// <value>The window's position.</value>
    public Vector2 Position { get; set; }

    /// <value>The window's renderer.</value>
    public IRenderer Renderer { get; }

    /// <value>The window's safe area.</value>
    public Rectangle SafeArea { get; }

    /// <value>The window's size.</value>
    public Size Size { get; set; }

    /// <value>The windows's surface</value>
    public ISurface Surface { get; }

    /// <value>The window's title.</value>
    public string Title { get; set; }

    /// <summary>Show the window.</summary>
    public ValueTask<bool> ShowAsync();
}
