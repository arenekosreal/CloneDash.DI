using CloneDash.Sdl;

using Microsoft.Extensions.Logging;
using System.Drawing;

using SDL3;

namespace CloneDash;

internal class MainScene : IMainScene
{
    private readonly IServiceProvider ServiceProvider;
    private readonly ISdl Sdl;
    private readonly ILogger<MainScene> Logger;

    public MainScene(IServiceProvider serviceProvider, ISdl sdl, ILogger<MainScene> logger) =>
        (ServiceProvider, Sdl, Logger) = (serviceProvider, sdl, logger);

    public async ValueTask RenderAsync(CancellationToken token)
    {

    }

    public async Task HandleEventAsync(SDL.Event e, CancellationToken token)
    {

    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
