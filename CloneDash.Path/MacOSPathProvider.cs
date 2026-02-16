using System.Runtime.Versioning;

using PathLib;

namespace CloneDash.Path;

/// <summary><see cref="IPathProvider" /> implementation for macOS.</summary>
[SupportedOSPlatform("macOS")]
public class MacOSPathProvider : IPathProvider
{
    private readonly string AppId;

    // $HOME
    private readonly IPath Home = new PosixPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

    private readonly IPath Preferences;

    // $HOME/Library/Caches
    private readonly IPath Caches = new PosixPath(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));

    private readonly IPath ApplicationSupport;

    private readonly IPath Frameworks;

    /// <summary>Initialize <see cref="MacOSPathProvider" /> with arguments given.</summary>
    /// <param name="appId">The id of application.</param>
    public MacOSPathProvider(string appId)
    {
        AppId = appId;
        Preferences = Home / "Library" / "Preferences";
        ApplicationSupport = Home / "Library" / "Application Support";
        Frameworks = Home / "Library" / "Frameworks";
    }

    /// <inheritdoc />
    public IPath GetWritableConfigPath(string name) => Preferences / AppId / name;

    /// <inheritdoc />
    public IPath? GetReadableConfigPath(string name)
    {
        IPath configPath = Preferences / AppId / name;
        return configPath.Exists() ? configPath : null;
    }

    /// <inheritdoc />
    public IPath GetWritableDataPath(string name) => ApplicationSupport / AppId / name;

    /// <inheritdoc />
    public IPath? GetReadableDataPath(string name) =>
        new IPath[] { ApplicationSupport, Frameworks }.Select(h => h / AppId / name)
            .FirstOrDefault(p => p.Exists());

    /// <inheritdoc />
    public IPath GetWritableCachePath(string name) => Caches / AppId / name;

    /// <inheritdoc />
    public IPath GetWritableStatePath(string name) => ApplicationSupport / AppId / name;

    /// <inheritdoc />
    public IEnumerable<IPath> EnumerateSystemFonts()
    {
        IPath[] possibleFontsDirs = new[]
        {
            // $HOME/Library/Fonts
            new PosixPath(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)),
            new PosixPath("/Library/Fonts"),
            new PosixPath("/System/Library/Fonts"),
        };
        return possibleFontsDirs.SelectMany(dir =>
        {
            if (dir.IsDir())
                return dir.ListDir("*.ttf", SearchOption.AllDirectories)
                          .TakeWhile(f => !f.IsDir());
            else
                return new IPath[0];
        });
    }
}
