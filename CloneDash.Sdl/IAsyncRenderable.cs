using SDL3;

namespace CloneDash.Sdl;

/// <summary>A renderable object.</summary>
public interface IAsyncRenderable : IDisposable
{
    /// <summary>Render this.</summary>
    public void Render();

    /// <summary>Handle events of this.</summary>
    public Task HandleEventAsync(SDL.Event e, CancellationToken token = default);
}
