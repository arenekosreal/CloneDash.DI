using SDL3;

namespace CloneDash.Sdl;

/// <summary>A renderable object.</summary>
public interface IAsyncRenderable : IAsyncDisposable
{
    /// <summary>Render this.</summary>
    public ValueTask RenderAsync(CancellationToken token = default);

    /// <summary>Handle events of this.</summary>
    public Task HandleEventAsync(SDL.Event e, CancellationToken token = default);
}
