using CloneDash.Sdk;
using CloneDash.Sdl;

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SDL3;

namespace CloneDash;

internal sealed class Kanban : IKanban
{
    private readonly ICharacter? CurrentCharacter;
    private readonly ISdl Sdl;
    private readonly ILogger<Kanban> Logger;
    private readonly IStringLocalizer<Kanban> Localizer;
    private readonly IAudioStream Bgm;
    private readonly IOptionsMonitor<UserData> UserData;

    public Kanban(ISdl sdl, ILogger<Kanban> logger, IStringLocalizer<Kanban> localizer,
                  IEnumerable<ICharacter> characters, IOptionsMonitor<UserData> userData)
    {
        (CurrentCharacter, Sdl, Logger, Localizer, UserData) =
        // TODO [#0] Apply user config here.
        (characters.FirstOrDefault(), sdl, logger, localizer, userData);
        Bgm = Sdl.AudioDevice.OpenAudioStream();
        if (CurrentCharacter is null)
            Logger.LogWarning("No character is used in kanban scene.");
        else
            Logger.LogDebug("Using character {0} in kanban scene.", CurrentCharacter.Name);
    }

    public void Render()
    {

    }

    public async Task HandleEventAsync(SDL.Event e, CancellationToken token)
    {

    }

    public void Dispose() => Bgm.Dispose();
}
