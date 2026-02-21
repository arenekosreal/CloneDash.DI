using System.Drawing;
using System.Reflection;

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
    private readonly List<Font> Fonts = new();
    private readonly Color DefaultRendererColor = Color.Black;

    public IWindow Window { get; }
    public IRenderer Renderer { get; }
    public ITextEngine TextEngine { get; }
    public IAudioDevice AudioDevice { get; }
    public IRenderable? CurrentRendering { get; set; }
    public bool HasMouse { get => SDL.HasMouse(); }
    public bool HasGamepad { get => SDL.HasGamepad(); }
    public bool HasJoystick { get => SDL.HasJoystick(); }
    public bool HasKeyboard { get => SDL.HasKeyboard(); }

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
            Renderer.Clear();
            Renderer.DrawColor = DefaultRendererColor;
            if (CurrentRendering is not null)
                CurrentRendering.Render();
            Renderer.Present();
        }
    }

    public void Dispose()
    {
        AudioDevice.Dispose();
        Renderer.Dispose();
        Window.Dispose();
        TTF.Quit();
        SDL.Quit();
    }

    public IFont? GetFont(string fontFamilyName, int fontSize, TTF.FontStyleFlags fontStyle, TTF.HintingFlags fontHinting)
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
            IFont font = GetFont(fontPath, fontSize, fontStyle, fontHinting);
            if (font.FamilyName == fontFamilyName)
                return font;
            else
                font.Dispose();
        }
        Logger.LogDebug("No font named {0} in system was found.", fontFamilyName);
        Assembly assemblyOfType = typeof(Sdl).Assembly;
        foreach (string ttfName in assemblyOfType.GetManifestResourceNames().TakeWhile(name => name.EndsWith(".ttf")))
        {
            Logger.LogDebug("Checking font resource {0}", ttfName);
            if (assemblyOfType.GetManifestResourceStream(ttfName) is Stream ttfStream)
            {
                Font font = new(ttfStream, fontSize);
                Logger.LogDebug("Checking font {0} in assembly...", font.FamilyName);
                if (font.FamilyName == fontFamilyName)
                {
                    font.Style = fontStyle;
                    font.Hinting = fontHinting;
                    Fonts.Add(font);
                    return font;
                }
                else
                    font.Dispose();
            }
        }
        Logger.LogWarning("No font named {0} in embedded resources was found.", fontFamilyName);
        return null;
    }

    public IFont GetFont(IPath fontPath, int fontSize, TTF.FontStyleFlags fontStyle, TTF.HintingFlags fontHinting)
    {
        foreach (IFont font in Fonts)
        {
            if (font.Path == fontPath
             && font.Size == fontSize
             && font.Style == fontStyle
             && font.Hinting == fontHinting)
                return font;
        }
        Logger.LogDebug("No font at {0} in cache was found.", fontPath);
        Font newFont = new(fontPath, fontSize);
        newFont.Style = fontStyle;
        newFont.Hinting = fontHinting;
        Fonts.Add(newFont);
        return newFont;
    }

    public IAudio GetWAVAudio(IPath wavFile) => Audio.FromWAV(wavFile);

    public ISurface GetBMPSurface(IPath bmpFile) => Surface.FromBMP(bmpFile);
}
