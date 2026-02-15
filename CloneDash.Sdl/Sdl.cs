using System.Drawing;

using CloneDash.Configuration;
using CloneDash.Path;

using Microsoft.Extensions.Logging;

using PathLib;

using SDL3;

namespace CloneDash.Sdl;

internal class Sdl : ISdl
{
    private readonly ILogger<Sdl> Logger;
    private readonly IWritableOptions<SdlConfiguration> Configuration;
    private readonly IPathProvider PathProvider;
    private readonly IAudioDevice AudioDevice;
    private readonly List<Font> Fonts = new();
    private readonly Color DefaultRendererColor = Color.Black;

    public IWindow Window { get; }
    public IRenderer Renderer { get; }
    public ITextEngine TextEngine { get; }
    public IAsyncRenderable? CurrentRendering { get; set; }

    public Sdl(ILogger<Sdl> logger,
               IWritableOptions<SdlConfiguration> configuration,
               IPathProvider pathProvider,
               string appName,
               string appVersion,
               string appId)
    {
        (Logger, Configuration, PathProvider) = (logger, configuration, pathProvider);
        if (!SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Audio))
        {
            Logger.LogError("SDL initialize failed: {0}", SDL.GetError());
            throw new InvalidOperationException("Failed to initialize SDL.");
        }
        if (!TTF.Init())
        {
            Logger.LogError("SDL_ttf initialize failed: {0}", SDL.GetError());
            throw new InvalidOperationException("Failed to initialize SDL_ttf");
        }
        Logger.LogDebug($"Setting SDL metadata: appname={appName} appversion={appVersion} appidentifier={appId}");
        SDL.SetAppMetadata(appName, appVersion, appId);
        SdlConfiguration cfg = Configuration.Value;
        Window = new Window(appName, cfg.Width, cfg.Height, cfg.SdlFlags);
        Renderer = new Renderer(Window);
        TextEngine = new RendererTextEngine(Renderer);
        Logger.LogDebug("Opening audio device...");
        AudioDevice = new AudioDevice();
        Renderer.DrawColor = DefaultRendererColor;
    }

    public async Task RunUntilQuitAsync(CancellationToken token = default)
    {
        bool loop = true;
        while (!token.IsCancellationRequested && loop)
        {
            while (SDL.PollEvent(out SDL.Event e))
            {
                switch ((SDL.EventType)e.Type)
                {
                    case SDL.EventType.Quit:
                        loop = false;
                        break;
                    default:
                        if (CurrentRendering is not null)
                            await CurrentRendering.HandleEventAsync(e, token);
                        break;
                }
            }
            await Renderer.ClearAsync();
            Renderer.DrawColor = DefaultRendererColor;
            if (CurrentRendering is not null)
                await CurrentRendering.RenderAsync(token);
            await Renderer.PresentAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await AudioDevice.DisposeAsync();
        await Renderer.DisposeAsync();
        await Window.DisposeAsync();
        foreach (Font font in Fonts)
            await font.DisposeAsync();
        Fonts.Clear();
        TTF.Quit();
        SDL.Quit();
    }

    public async ValueTask<IFont?> GetFontAsync(string fontFamilyName, int fontSize, TTF.FontStyleFlags fontStyle, TTF.HintingFlags fontHinting)
    {
        foreach (IFont font in Fonts)
        {
            if (font.FamilyName == fontFamilyName
             && font.Size == fontSize
             && font.Style == fontStyle
             && font.Hinting == fontHinting)
                return font;
        }
        Logger.LogDebug("No font named {0} in cache was found.", fontFamilyName);
        foreach (IPath fontPath in PathProvider.EnumerateSystemFonts())
        {
            IFont font = await GetFontAsync(fontPath, fontSize, fontStyle, fontHinting);
            if (font.FamilyName == fontFamilyName)
                return font;
            else
                await font.DisposeAsync();
        }
        Logger.LogWarning("No font named {0} in system was found.", fontFamilyName);
        return null;
    }

    public ValueTask<IFont> GetFontAsync(IPath fontPath, int fontSize, TTF.FontStyleFlags fontStyle, TTF.HintingFlags fontHinting)
    {
        foreach (IFont font in Fonts)
        {
            if (font.Path == fontPath
             && font.Size == fontSize
             && font.Style == fontStyle
             && font.Hinting == fontHinting)
                return ValueTask.FromResult(font);
        }
        Logger.LogDebug("No font at {0} in cache was found.", fontPath);
        Font newFont = new(fontPath, fontSize);
        newFont.Style = fontStyle;
        newFont.Hinting = fontHinting;
        Fonts.Add(newFont);
        return ValueTask.FromResult<IFont>(newFont);
    }

    public ValueTask<IAudio> GetAudioAsync(IPath audioPath) =>
        ValueTask.FromResult<IAudio>(new WAVAudio(audioPath));
}
