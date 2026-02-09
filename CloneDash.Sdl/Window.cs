using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Window : IWindow
{
    public IntPtr SdlPtr { get; internal init; }

    public float DisplayScale { get => SDL.GetWindowDisplayScale(SdlPtr); }

    public SDL.WindowFlags Flags { get => SDL.GetWindowFlags(SdlPtr); }

    public ISurface Icon { set => SDL.SetWindowIcon(SdlPtr, value.SdlPtr); }

    public IWindow? Parent
    {
        get => SDL.GetWindowParent(SdlPtr) switch
        {
            0 => null,
            IntPtr value => new Window() { SdlPtr = value }
        };
        set => SDL.SetWindowParent(SdlPtr, value?.SdlPtr ?? IntPtr.Zero);
    }

    public Vector2 Position
    {
        get => SDL.GetWindowPosition(SdlPtr, out int x, out int y) ? new(x, y) : default;
        set => SDL.SetWindowPosition(SdlPtr, Convert.ToInt32(value.X), Convert.ToInt32(value.Y));
    }

    public IRenderer Renderer { get => new Renderer() { SdlPtr = SDL.GetRenderer(SdlPtr) }; }

    public Rectangle SafeArea
    {
        get => SDL.GetWindowSafeArea(SdlPtr, out SDL.Rect rect) ? Rectangle.FromLTRB(rect.X, rect.Y, rect.W, rect.H) : default;
    }

    public Size Size
    {
        get => SDL.GetWindowSizeInPixels(SdlPtr, out int w, out int h) ? new(w, h) : default;
        set => SDL.SetWindowSize(SdlPtr, value.Width, value.Height);
    }

    public ISurface Surface { get => new Surface() { SdlPtr = SDL.GetWindowSurface(SdlPtr) }; }

    public string Title { get => SDL.GetWindowTitle(SdlPtr); set => SDL.SetWindowTitle(SdlPtr, value); }

    public ValueTask<bool> ShowAsync() => ValueTask.FromResult(SDL.ShowWindow(SdlPtr));

    public ValueTask DisposeAsync()
    {
        SDL.DestroyWindow(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
