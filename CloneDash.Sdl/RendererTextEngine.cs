using SDL3;

namespace CloneDash.Sdl;

internal readonly struct RendererTextEngine : ITextEngine
{
    public IntPtr SdlPtr { get; internal init; }

    public RendererTextEngine(IRenderer renderer) => SdlPtr = TTF.CreateRendererTextEngine(renderer.SdlPtr);

    public ValueTask DisposeAsync()
    {
        TTF.DestroyRendererTextEngine(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
