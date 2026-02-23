using SDL3;

namespace CloneDash.Sdl;

internal readonly struct RendererTextEngine : ITextEngine
{
    public IntPtr SdlPtr { get; }

    public RendererTextEngine(IRenderer renderer) => SdlPtr = TTF.CreateRendererTextEngine(renderer.SdlPtr);

    internal RendererTextEngine(IntPtr existing) => SdlPtr = existing;

    public IText CreateText(IFont font, string text) =>
        new Text(TTF.CreateText(SdlPtr, font.SdlPtr, text, 0));

    public void Dispose() => TTF.DestroyRendererTextEngine(SdlPtr);
}
