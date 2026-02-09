using System.Drawing;
using System.Numerics;
using System.Text;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Text : IText
{
    public IntPtr SdlPtr { get; }

    public string String
    {
        set => TTF.SetTextString(SdlPtr, value, Convert.ToUInt32(Encoding.UTF8.GetByteCount(value)));
    }

    public Color Color
    {
        get => TTF.GetTextColor(SdlPtr, out byte r, out byte g, out byte b, out byte a) ? Color.FromArgb(a, r, g, b) : default;
        set => TTF.SetTextColor(SdlPtr, value.R, value.G, value.B, value.A);
    }

    public TTF.Direction Direction { get => TTF.GetTextDirection(SdlPtr); set => TTF.SetTextDirection(SdlPtr, value); }

    public ITextEngine TextEngine
    {
        get => new RendererTextEngine() { SdlPtr = TTF.GetTextEngine(SdlPtr) };
        set => TTF.SetTextEngine(SdlPtr, value.SdlPtr);
    }

    public IFont Font
    {
        get => new Font() { SdlPtr = TTF.GetTextFont(SdlPtr), Path = null };
        set => TTF.SetTextFont(SdlPtr, value.SdlPtr);
    }

    public Vector2 Position
    {
        get => TTF.GetTextPosition(SdlPtr, out int x, out int y) ? new(x, y) : default;
        set => TTF.SetTextPosition(SdlPtr, Convert.ToInt32(value.X), Convert.ToInt32(value.Y));
    }

    public uint Script { get => TTF.GetTextScript(SdlPtr); set => TTF.SetTextScript(SdlPtr, value); }

    public Vector2 Size
    {
        get => TTF.GetTextSize(SdlPtr, out int w, out int h) ? new(w, h) : default;
    }

    public int WrapWidth
    {
        get => TTF.GetTextWrapWidth(SdlPtr, out int wrapWidth) ? wrapWidth : 0;
        set => TTF.SetTextWrapWidth(SdlPtr, value);
    }

    public Text(string text, IFont font, ITextEngine? textEngine = null)
    {
        SdlPtr = TTF.CreateText(textEngine?.SdlPtr ?? IntPtr.Zero, font.SdlPtr, text, (uint)Encoding.UTF8.GetByteCount(text));
    }

    public ValueTask DisposeAsync()
    {
        TTF.DestroyText(SdlPtr);
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> RenderAsync(Vector2 position) =>
        ValueTask.FromResult(TTF.DrawRendererText(SdlPtr, position.X, position.Y));

    /// <inheritdoc />
    public ValueTask<bool> RenderAsync(Vector2 position, ISurface surface) =>
        ValueTask.FromResult(TTF.DrawSurfaceText(SdlPtr, Convert.ToInt32(position.X), Convert.ToInt32(position.Y), surface.SdlPtr));
}
