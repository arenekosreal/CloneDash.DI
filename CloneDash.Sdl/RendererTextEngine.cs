using SDL3;

namespace CloneDash.Sdl;

internal readonly struct RendererTextEngine : ITextEngine
{
    public IntPtr SdlPtr { get; }

    public RendererTextEngine(IRenderer renderer) => SdlPtr = TTF.CreateRendererTextEngine(renderer.SdlPtr);

    internal RendererTextEngine(IntPtr existing) => SdlPtr = existing;

    public IText CreateText(IFont font, string text, uint length = 0) =>
        new Text(TTF.CreateText(SdlPtr, font.SdlPtr, text, length));

    public void Dispose() => TTF.DestroyRendererTextEngine(SdlPtr);
}
