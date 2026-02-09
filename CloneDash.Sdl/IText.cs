using System.Drawing;
using System.Numerics;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Text" />.</summary>
public interface IText : ISdlWrapper<IntPtr>
{
    /// <value>The text content.</value>
    public string String { set; }

    /// <value>The color of the text.</value>
    public Color Color { get; set; }

    /// <value>The direction of the text.</value>
    public TTF.Direction Direction { get; set; }

    /// <value>The text engine used by the text.</value>
    public ITextEngine TextEngine { get; set; }

    /// <value>The font used by the text.</value>
    public IFont Font { get; set; }

    /// <value>The position of the text.</value>
    public Vector2 Position { get; set; }

    /// <value>The <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924 code</see> of the text.</value>
    public uint Script { get; set; }

    /// <value>The size of the text.</value>
    public Vector2 Size { get; }

    /// <value>The wrap width of the text.</value>
    public int WrapWidth { get; set; }

    /// <summary>Render the text at <paramref name="position" />.</summary>
    public ValueTask<bool> RenderAsync(Vector2 position);

    /// <summary>Render the text at <paramref name="position" /> on <paramref name="surface" />.</summary>
    public ValueTask<bool> RenderAsync(Vector2 position, ISurface surface);
}
