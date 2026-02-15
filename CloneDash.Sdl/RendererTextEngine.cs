using System.Text;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct RendererTextEngine : ITextEngine
{
    public IntPtr SdlPtr { get; internal init; }

    public RendererTextEngine(IRenderer renderer) => SdlPtr = TTF.CreateRendererTextEngine(renderer.SdlPtr);

    public ValueTask<IText> CreateTextAsync(IFont font, string text) =>
        ValueTask.FromResult<IText>(
            new Text() { SdlPtr = TTF.CreateText(SdlPtr,
                                                 font.SdlPtr,
                                                 text,
                                                 Convert.ToUInt32(Encoding.UTF8.GetByteCount(text))) });

    public ValueTask DisposeAsync()
    {
        TTF.DestroyRendererTextEngine(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
