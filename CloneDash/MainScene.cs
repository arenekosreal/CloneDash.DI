using System.Drawing;
using System.Numerics;

using CloneDash.Sdk;
using CloneDash.Sdl;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using SDL3;

namespace CloneDash;

internal sealed class MainScene : IMainScene
{
    private static readonly Vector2 EnterTipPositionFacor = new(0.5f, 0.75f);

    private readonly IServiceProvider ServiceProvider;
    private readonly ISdl Sdl;
    private readonly ILogger<MainScene> Logger;
    private readonly IStringLocalizer<MainScene> Localizer;
    private readonly IIllustration? Illustration;
    private readonly IAudioStream BgmStream;
    private readonly IFont UiFont;
    private readonly IText EnterTip;
    private readonly Vector2 EnterTipPosition;

    public MainScene(IServiceProvider serviceProvider,
                     ISdl sdl,
                     ILogger<MainScene> logger,
                     IStringLocalizer<MainScene> localizer,
                     IEnumerable<IIllustration> illustrations,
                     [FromKeyedServices(WellKnownFonts.SansSerif)]
                     IFont uiFont)
    {
        (ServiceProvider, Sdl, Logger, Localizer, UiFont, Illustration) =
        (serviceProvider, sdl, logger, localizer, uiFont, illustrations.RandomOrDefault());
        if (Illustration is IIllustration illustration)
            Logger.LogDebug("Using illustration {0} in main scene.", illustration.Name);
        else
            Logger.LogWarning("No illustration is used in main scene.");
        BgmStream = Sdl.AudioDevice.OpenAudioStream();
        EnterTip = Sdl.TextEngine.CreateText(UiFont, GetLocalizedString(nameof(EnterTip)));
        EnterTip.Font.WrapAlignment = TTF.HorizontalAlignment.Center;
        EnterTip.Font.Style = TTF.FontStyleFlags.Bold;
        EnterTip.Color = Color.White;
        EnterTipPosition = new((Sdl.Window.Size.Width - EnterTip.Size.Width),
                               (Sdl.Window.Size.Height - EnterTip.Size.Height));
        EnterTipPosition *= EnterTipPositionFacor;
    }

    public void Render()
    {
        if (Illustration is IIllustration illustration)
            illustration.Render();
        EnterTip.Render(EnterTipPosition);
    }

    public async Task HandleEventAsync(SDL.Event e, CancellationToken token)
    {
        if (Illustration is IIllustration illustration)
        {
            if (illustration.Bgm is IAudio bgm)
            {
                if (BgmStream.Paused)
                    BgmStream.Paused = false;
                if (BgmStream.Queued == 0)
                    BgmStream.Put(bgm);
            }
            await illustration.HandleEventAsync(e, token);
        }
        // When any key is pressed, and even screen is touched.
        bool enterKanban = false;
        switch ((SDL.EventType)e.Type)
        {
            case SDL.EventType.MouseButtonDown:
                enterKanban = Sdl.HasMouse;
                break;
            case SDL.EventType.GamepadButtonDown:
                enterKanban = Sdl.HasGamepad;
                break;
            case SDL.EventType.JoystickButtonDown:
                enterKanban = Sdl.HasJoystick;
                break;
            case SDL.EventType.KeyDown:
                enterKanban = Sdl.HasKeyboard;
                break;
        }
        if (enterKanban)
        {
            Logger.LogDebug("Entering kanban scene...");
            Sdl.CurrentRendering = ServiceProvider.GetRequiredService<IKanban>();
            BgmStream.Paused = true;
        }
    }

    public void Dispose() => BgmStream.Dispose();

    private string GetLocalizedString(string key) => Localizer[key];
}
