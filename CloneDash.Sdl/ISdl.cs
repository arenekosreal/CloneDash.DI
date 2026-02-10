using PathLib;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Api requirements for class wraps SDL operations.</summary>
public interface ISdl : IAsyncDisposable
{
    /// <value>The window created by SDL.</value>
    public IWindow Window { get; }

    /// <value>The renderer created by SDL.</value>
    public IRenderer Renderer { get; }

    /// <value>The current rendering object.</value>
    public IAsyncRenderable? CurrentRendering { get; set; }

    /// <summary>Run SDL until quit.</summary>
    /// <param name="token">The <see cref="CancellationToken" /> to cancel rendering.</param>
    /// <remarks>Run this method in main thread.</remarks>
    public Task RunUntilQuitAsync(CancellationToken token = default);

    /// <summary>Get <see cref="IFont" /> which family name is <paramref name="fontFamilyName" /> and matches all other arguments.</summary>
    public ValueTask<IFont?> GetFontAsync(string fontFamilyName, int fontSize, TTF.FontStyleFlags fontStyle, TTF.HintingFlags fontHinting);

    /// <summary>Get <see cref="IFont" /> which file path is  <paramref name="fontPath" /> and matches all other arguments.</summary>
    public ValueTask<IFont> GetFontAsync(IPath fontPath, int fontSize, TTF.FontStyleFlags fontStyle, TTF.HintingFlags fontHinting);

    /// <summary>Get <see cref="IAudio" /> from <paramref name="audioPath" />.</summary>
    public ValueTask<IAudio> GetAudioAsync(IPath audioPath);
}
